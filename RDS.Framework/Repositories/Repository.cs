using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using RDS.Core;
using RDS.Core.Entities;
using RDS.Core.Entities.Base;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Dynamic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Z.EntityFramework.Plus;

namespace RDS.Framework.Repositories
{
    public partial class Repository<T> : IRepository<T> where T : class, IBaseEntity
    {
        #region Fields

        protected readonly RDSContext _context;

        protected readonly DbSet<T> _dbSet;

        #endregion

        #region Ctor

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="context">Object context</param>
        public Repository(RDSContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        #endregion

        #region Utilities


        /// <summary>
        /// 
        /// </summary>
        /// <param name="where"></param>
        /// <param name="keys"></param>
        /// <param name="orderByAsc"></param>
        /// <returns></returns>
        protected IEnumerable<T> GetQueryFromFilter(Func<T, bool> where = null,
          Dictionary<Func<T, object>, bool> orderByAsc = null)
        {
            var query = this.Table;
            if (where != null)
                query = query.Where(where);
            if (orderByAsc != null && orderByAsc.Any())
            {
                query = orderByAsc.First().Value
                    ? query.OrderBy(orderByAsc.First().Key)
                    : query.OrderByDescending(orderByAsc.First().Key);
                orderByAsc.Remove(orderByAsc.First().Key);
                query = orderByAsc.Aggregate(query, (current, order) => order.Value
                    ? (current as IOrderedEnumerable<T>).ThenBy(order.Key)
                    : (current as IOrderedEnumerable<T>).ThenByDescending(order.Key));
            }
            return query;
        }
        #endregion

        #region Methods

        #region Synchronous
        /// <summary>
        /// Get entity by identifier
        /// </summary>
        /// <param name="id">Identifier</param>
        /// <returns>Entity</returns>
        public virtual T GetById(int id)
        {
            return this._dbSet.Find(id);
        }
        /// <summary>
        /// Insert entity
        /// </summary>
        /// <param name="entity">Entity</param>
        public virtual void Insert(T entity)
        {
            try
            {
                if (entity == null)
                    throw new ArgumentNullException("entity");

                this._dbSet.Add(entity);

                this._context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        /// <summary>
        /// Insert entities
        /// </summary>
        /// <param name="entities">Entities</param>
        public virtual void Insert(IEnumerable<T> entities)
        {
            try
            {
                if (entities == null)
                    throw new ArgumentNullException("entities");

                foreach (var entity in entities)
                    this._dbSet.Add(entity);

                this._context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        /// <summary>
        /// Update entity
        /// </summary>
        /// <param name="entity">Entity</param>
        public virtual void Update(T entity)
        {
            try
            {
                if (entity == null)
                    throw new ArgumentNullException("entity");

                this._context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        /// <summary>
        /// Update entities
        /// </summary>
        /// <param name="entities">Entities</param>
        public virtual void Update(IEnumerable<T> entities)
        {
            try
            {
                if (entities == null)
                    throw new ArgumentNullException("entities");

                this._context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        /// <summary>
        /// Delete entity
        /// </summary>
        /// <param name="entity">Entity</param>
        public virtual void Delete(T entity)
        {
            try
            {
                if (entity == null)
                    throw new ArgumentNullException("entity");

                this._dbSet.Remove(entity);

                this._context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        /// <summary>
        /// Delete entities
        /// </summary>
        /// <param name="entities">Entities</param>
        public virtual void Delete(IEnumerable<T> entities)
        {
            try
            {
                if (entities == null)
                    throw new ArgumentNullException("entities");


                //foreach (var entity in entities)
                ((DbSet<T>)this._dbSet).RemoveRange(entities);

                this._context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public virtual IEnumerable<T> FindMany(Func<T, bool> where = null,
        Dictionary<Func<T, object>, bool> orderByAsc = null,
        int? skip = null, int? take = null)
        {
            var query = GetQueryFromFilter(where, orderByAsc);
            if (skip != null)
                query = query.Skip(skip.Value);
            if (take != null)
                query = query.Take(take.Value);
            return query;
        }

        /// <summary>
        /// Gets a table
        /// </summary>
        public virtual IEnumerable<T> Table
        {
            get
            {
                return this._dbSet;
            }
        }

        /// <summary>
        /// Gets a table with "no tracking" enabled (EF feature) Use it only when you load record(s) only for read-only operations
        /// </summary>
        public virtual IEnumerable<T> TableNoTracking
        {
            get
            {
                return this._dbSet.AsNoTracking();
            }
        }

        public virtual IQueryable<T> TableQueryable
        {
            get
            {
                return this._dbSet.AsQueryable();
            }
        }
        #endregion

        #region Asynchronous

        public IQueryable<T> Query()
        {
            return _dbSet;
        }
        /// <summary>
        /// Get entity by identifier async
        /// </summary>
        /// <param name="id">Identifier</param>
        /// <returns>Entity</returns>
        public virtual async Task<T> GetByIdAsync(int id)
        {
            return await Task.FromResult(_dbSet.Find(id));
        }
        /// <summary>
        /// Insert entity async
        /// </summary>
        /// <param name="entity">Entity</param>
        public async Task InsertAsync(T entity)
        {
            try
            {
                if (entity == null)
                    throw new ArgumentNullException("entity");

                this._dbSet.Add(entity);
                try
                {
                    await this._context.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    var mess = ex.Message;
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        /// <summary>
        /// Insert entities async
        /// </summary>
        /// <param name="entities">Entities</param>
        public virtual async Task InsertAsync(IEnumerable<T> entities)
        {
            try
            {
                if (entities == null)
                    throw new ArgumentNullException("entities");

                foreach (var entity in entities)
                    this._dbSet.Add(entity);

                await this._context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        /// <summary>
        /// Update entity async
        /// </summary>
        /// <param name="entity">Entity</param>
        public virtual async Task UpdateAsync(T entity)
        {
            try
            {
                if (entity == null)
                    throw new ArgumentNullException("entity");

                await this._context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        /// <summary>
        /// Update entities async
        /// </summary>
        /// <param name="entities">Entities</param>
        public virtual async Task UpdateAsync(IEnumerable<T> entities)
        {
            try
            {
                if (entities == null)
                    throw new ArgumentNullException("entities");

                await this._context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        /// <summary>
        /// Delete entity async
        /// </summary>
        /// <param name="entity">Entity</param>
        public virtual async Task DeleteAsync(T entity)
        {
            try
            {
                if (entity == null)
                    throw new ArgumentNullException("entity");

                this._dbSet.Remove(entity);

                await this._context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        /// <summary>
        /// Delete entities async
        /// </summary>
        /// <param name="entities">Entities</param>
        public virtual async Task DeleteAsync(IEnumerable<T> entities)
        {
            try
            {
                if (entities == null)
                    throw new ArgumentNullException("entities");

                foreach (var entity in entities)
                    this._dbSet.Remove(entity);

                await this._context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion

        #endregion
        
    }
}