using System.Collections.Generic;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RDS.Core;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using RDS.Framework.AutoMapper;
using Microsoft.OpenApi.Models;
using RDS.Framework.Services;
using RDS.Framework.Services.Users;
using RDS.Framework.Repositories;
using Swashbuckle.AspNetCore.Filters;
using System.IO;
using System;
using System.Reflection;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.Extensions.Primitives;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using RDS.Framework.Helpers;

namespace RDS.API
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        bool useInMemoryProvider = false;

        public IConfiguration Configuration { get; }
        public IHostEnvironment Environment { get; }

        public Startup(IConfiguration configuration, IHostEnvironment environment)
        {
            Configuration = configuration;
            Environment = environment;

            var builder = new ConfigurationBuilder()
                 .SetBasePath(environment.ContentRootPath)
                 .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                 .AddJsonFile($"appsettings.{environment.EnvironmentName}.json", reloadOnChange: true, optional: true)
                 .AddEnvironmentVariables();

            Configuration = builder.Build();
        }


        public void ConfigureServices(IServiceCollection services)
        {
            // config db, because .net core 3.0 not suport Microsoft.AspNetCore 2.7, should have to installed each package
            string sqlConnectionString = Configuration.GetConnectionString("DefaultConnection");
            try
            {
                useInMemoryProvider = bool.Parse(Configuration["AppSettings:InMemoryProvider"]);
            }
            catch { }

            services.AddDbContext<RDSContext>(options =>
            {
                switch (useInMemoryProvider)
                {
                    case true:
                        options.UseInMemoryDatabase("");
                        break;
                    default:
                        options.UseSqlServer(sqlConnectionString,
                            b => b.MigrationsAssembly("RDS.Core"));
                        break;
                }
            });

            //services.AddTransient<IUserService, UserService>();
            services.AddTransient(typeof(IRepository<>), typeof(Repository<>));
            services.ResgiterServices(Configuration);

            //config swagger
            // Register the Swagger generator, defining 1 or more Swagger documents
                services.AddSwaggerGen(c =>
                {
                    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Diamond API", Version = "v1" });
                    // Swagger 2.+ support
                    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                    {
                        Description =
            "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 12345abcdef\"",
                        Name = "Authorization",
                        In = ParameterLocation.Header,
                        Type = SecuritySchemeType.ApiKey,
                        Scheme = "Bearer"
                    });

            

                    c.AddSecurityRequirement(new OpenApiSecurityRequirement()
                    {
                        {
                            new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                {
                                    Type = ReferenceType.SecurityScheme,
                                    Id = "Bearer"
                                },
                                Scheme = "oauth2",
                                Name = "Bearer",
                                In = ParameterLocation.Header,

                            },
                            new string[] { }
                        }
                    });


                });


           

            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder => builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader());
            });


           
            services.AddControllers()
                .AddNewtonsoftJson();


            // Auto Mapper Configurations
            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MappingProfile());
            });

            IMapper mapper = mappingConfig.CreateMapper();
            services.AddSingleton(mapper);

            services.AddMvc();

            services
          .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
          .AddJwtBearer(options =>
          {
              options.RequireHttpsMetadata = false;
              options.SaveToken = true;
              options.SecurityTokenValidators.Clear();
              options.SecurityTokenValidators.Add(new JwtSecurityTokenHandler
              {
                  InboundClaimTypeMap = new Dictionary<string, string>()
              });
              options.TokenValidationParameters = new TokenValidationParameters()
              {
                  ValidateIssuerSigningKey = true,
                  IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Configuration["BearerJwt:JwtKey"])),
                  ValidateIssuer = false,
                   //ValidIssuer = Configuration["BearerJwt:JwtIssuer"],
                   ValidateAudience = false,
                   //ValidAudience = Configuration["BearerJwt:JwtAudience"],
                   ValidateLifetime = true,
                  ClockSkew = TimeSpan.Zero,

                   //Now, every time you get the user’s name through the User.Identity.Name property, it will look for the value of the name claim on the user, and return the correct value for the user’s name.
                   NameClaimType = JwtRegisteredClaimNames.UniqueName
              };

              options.Events = new JwtBearerEvents
              {
                   //"hack" for signalr
                   OnMessageReceived = context =>
                  {
                      if (context.Request.Query.TryGetValue("access_token", out StringValues token) && !string.IsNullOrEmpty(token))
                          context.Token = token;

                      return Task.CompletedTask;
                  },

                   // Skip the default logic.
                   OnChallenge = context =>
                  {
                      context.HandleResponse();
                      context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                      context.Response.ContentType = "application/json";
                      var json = ResponseHelper.FailJson(StatusCodes.Status401Unauthorized.ToString(), "Unauthorized");
                      return context.Response.WriteAsync(JsonConvert.SerializeObject(json), Encoding.UTF8);
                  }
              };
          });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();

            }

            //app.UseRouting();

            //app.UseEndpoints(endpoints =>
            //{
            //    endpoints.MapGet("/", async context =>
            //    {
            //        await context.Response.WriteAsync("Hello World!");
            //    });
            //});

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseCors("CorsPolicy");

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });


            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
                c.RoutePrefix = string.Empty;
            });
        }

    }
}
