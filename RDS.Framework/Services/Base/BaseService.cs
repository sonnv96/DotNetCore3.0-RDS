using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using RDS.Core;
using RDS.Core.Entities.Base;
using RDS.Framework.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDS.Framework.Services.Base
{
    public class BaseService<T> : IBaseService<T> where T : BaseEntity
    {
        
        #region contructor

        public BaseService(RDSContext context)
        {
            Context = context;
            DbSet = Context.Set<T>();
        }

        private readonly RDSContext Context;

        private readonly DbSet<T> DbSet;


        #endregion
        #region Insert/Update/Delete

        public async Task<T> FindOne(int id)
        {

            return await DbSet.FirstOrDefaultAsync(x => x.Id == id);
        }

        #endregion




    }
}
