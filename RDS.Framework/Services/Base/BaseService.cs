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
        #region Insert/Update/Delete
        public IDbContextTransaction BeginTransaction()
        {
            return Context.Database.BeginTransaction();
        }

        public virtual T GetById(int id)
        {
            //see some suggested performance optimization (not tested)
            //http://stackoverflow.com/questions/11686225/dbset-find-method-ridiculously-slow-compared-to-singleordefault-on-id/11688189#comment34876113_11688189
            return DbSet.Find(id);
        }
        //public async Task FindOne(int id)
        //{
        //    await DbSet<>.Find(id);
        //}

        #endregion


        #region contructor

        public BaseService(RDSContext context)
        {
            Context = context;
            DbSet = Context.Set<T>();
        }

        private readonly RDSContext Context;

        private readonly DbSet<T> DbSet;


        #endregion


    }
}
