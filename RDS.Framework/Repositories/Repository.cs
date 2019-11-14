using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using RDS.Core;
using RDS.Core.Entities;
using RDS.Core.Entities.Base;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Z.EntityFramework.Plus;

namespace RDS.Framework.Repositories
{
    public class Repository<T> : IRepository<T> where T : BaseEntity
    {
        #region Insert/Update/Delete
        public IDbContextTransaction BeginTransaction()
        {
            return Context.Database.BeginTransaction();
        }

        public async Task DeleteAsync(T entity)
        {
            DbSet.Remove(entity);
            await Context.SaveChangesAsync();
        }

        public async Task DeleteAsync(IEnumerable<T> entities)
        {
            DbSet.RemoveRange(entities);
            await Context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Expression<Func<T, bool>> predicate)
        {
            await DbSet.Where(predicate).DeleteAsync();
        }

        public Task<Dictionary<string, object>> GetLocalizableTranslation(int entityId, string language)
        {
            throw new NotImplementedException();
        }

        public Task<Dictionary<int, Dictionary<string, object>>> GetLocalizableTranslation(IEnumerable<int> entityIds, string language)
        {
            throw new NotImplementedException();
        }

        public async Task InsertAsync(T entity)
        {
            await DbSet.AddAsync(entity);
            await Context.SaveChangesAsync();
        }

        public async Task InsertAsync(IEnumerable<T> entities)
        {
            await DbSet.AddRangeAsync(entities);
            await Context.SaveChangesAsync();
        }

        public Task InsertUpdateLocalizableTranslation(int entityId, Dictionary<string, Dictionary<string, object>> localizableData)
        {
            throw new NotImplementedException();
        }

        public Task InsertUpdateLocalizableTranslation(IEnumerable<(int entityId, Dictionary<string, Dictionary<string, object>> localizableData)> localizableDatas)
        {
            throw new NotImplementedException();
        }

        public Task InsertUpdateLocalizableTranslation(T entity, string language)
        {
            throw new NotImplementedException();
        }

        public Task LoadLocalizableTranslation(T entity, string language, bool overrideEvenIfLocalizaEmpty)
        {
            throw new NotImplementedException();
        }

        public Task LoadLocalizableTranslation(IEnumerable<T> entities, string language, bool overrideEvenIfLocalizaEmpty)
        {
            throw new NotImplementedException();
        }

        public IQueryable<T> Query()
        {
            return DbSet;
        }

        public async Task UpdateAsync(T entity)
        {
            DbSet.Update(entity);
            await Context.SaveChangesAsync();
        }

        public async Task UpdateAsync(IEnumerable<T> entities)
        {
            DbSet.UpdateRange(entities);
            await Context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Expression<Func<T, bool>> predicate, Expression<Func<T, T>> updateFactory)
        {
            await DbSet.Where(predicate).UpdateAsync(updateFactory);
        }
        #endregion


        #region contructor

        public Repository(RDSContext context)
        {
            Context = context;
            DbSet = Context.Set<T>();
        }

        private readonly RDSContext Context;

        private readonly DbSet<T> DbSet;


        #endregion


    }
}
