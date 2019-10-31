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

            // config swagger
            // Register the Swagger generator, defining 1 or more Swagger documents
            //    services.AddSwaggerGen(c =>
            //    {
            //        c.SwaggerDoc("v1", new OpenApiInfo { Title = "Diamond API", Version = "v1" });
            //        // Swagger 2.+ support
            //        c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            //        {
            //            Description =
            //"JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 12345abcdef\"",
            //            Name = "Authorization",
            //            In = ParameterLocation.Header,
            //            Type = SecuritySchemeType.ApiKey,
            //            Scheme = "Bearer"
            //        });

            //        c.AddSecurityRequirement(new OpenApiSecurityRequirement()
            //        {
            //            {
            //                new OpenApiSecurityScheme
            //                {
            //                    Reference = new OpenApiReference
            //                    {
            //                        Type = ReferenceType.SecurityScheme,
            //                        Id = "Bearer"
            //                    },
            //                    Scheme = "oauth2",
            //                    Name = "Bearer",
            //                    In = ParameterLocation.Header,

            //                },
            //                new List<string>()
            //            }
            //        });

            //        //c.OperationFilter<ExamplesOperationFilter>();

            //        // [Description] on Response properties
            //        //options.OperationFilter<DescriptionOperationFilter>();

            //        // Adds an Upload button to endpoints which have [AddSwaggerFileUploadButton]
            //        //c.OperationFilter<AddFileParamTypesOperationFilter>();

            //        // Adds "(Auth)" to the summary so that you can see which endpoints have Authorization
            //        c.OperationFilter<AppendAuthorizeToSummaryOperationFilter>();

            //        // UseFullTypeNameInSchemaIds replacement for .NET Core
            //        c.CustomSchemaIds(x => x.FullName);

            //        // Set the comments path for the Swagger JSON and UI.
            //        var xmlFile = "swagger-json.xml";
            //        var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
            //        c.IncludeXmlComments(xmlPath);
            //    });


            services.AddSwaggerGen(c =>
                {
                    c.SwaggerDoc("v1", new OpenApiInfo {
                        Version = "v1",
                        Title = "ToDo API",
                        Description = "A simple example ASP.NET Core Web API",
                        TermsOfService = new Uri("https://example.com/terms"),
                        Contact = new OpenApiContact
                        {
                            Name = "Shayne Boyer",
                            Email = string.Empty,
                            Url = new Uri("https://twitter.com/spboyer"),
                        },
                        License = new OpenApiLicense
                        {
                            Name = "Use under LICX",
                            Url = new Uri("https://example.com/license"),
                        }
                    });

                    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                    c.IncludeXmlComments(xmlPath);
                });


            //services.AddSwaggerGen(c =>
            //{
            //    c.SwaggerDoc("v1", new Info
            //    {
            //        Version = "v1",
            //        Title = "ToDo API",
            //        Description = "A simple example ASP.NET Core Web API",
            //    });

            //    // Set the comments path for the Swagger JSON and UI.
            //    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            //    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
            //    c.IncludeXmlComments(xmlPath);
            //});

            //services.AddSwaggerGen(c =>
            //{
            //    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Diamond API", Version = "v1" });
            //});

            // add cors
            //services.AddCors(options =>
            //{
            //    options.AddPolicy("AllowAll",
            //        builder => builder.AllowAnyOrigin()
            //        .AllowAnyMethod()
            //        .AllowAnyHeader()
            //        .AllowCredentials());
            //});

            // add cors
            //services.AddCors(options =>
            //{
            //    options.AddPolicy("AllowOrigin",
            //        builder => builder.AllowAnyOrigin()
            //        .AllowAnyMethod()
            //        .AllowAnyHeader());
            //});

            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder => builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader());
            });


            //services.AddMvc(options =>
            //{
            //    options.Filters.Add(new ProducesAttribute("application/json"));
            //})
            //.SetCompatibilityVersion(CompatibilityVersion.Version_3_0)
            //.AddDataAnnotationsLocalization()
            //.AddNewtonsoftJson();

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
