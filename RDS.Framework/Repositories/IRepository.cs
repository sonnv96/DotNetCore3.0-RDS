using Microsoft.EntityFrameworkCore.Storage;
using RDS.Core.Entities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace RDS.Framework.Repositories
{
    public partial interface IRepository<T> where T : class, IBaseEntity
    {
        #region Synchronous
        /// <summary>
        /// Get entity by identifier
        /// </summary>
        /// <param name="id">Identifier</param>
        /// <returns>Entity</returns>
        T GetById(int id);
        /// <summary>
        /// Insert entity
        /// </summary>
        /// <param name="entity">Entity</param>
        void Insert(T entity);
        /// <summary>
        /// Insert entities
        /// </summary>
        /// <param name="entities">Entities</param>
        void Insert(IEnumerable<T> entities);
        /// <summary>
        /// Update entity
        /// </summary>
        /// <param name="entity">Entity</param>
        void Update(T entity);
        /// <summary>
        /// Update entities
        /// </summary>
        /// <param name="entities">Entities</param>
        void Update(IEnumerable<T> entities);
        /// <summary>
        /// Delete entity
        /// </summary>
        /// <param name="entity">Entity</param>
        void Delete(T entity);
        /// <summary>
        /// Delete entities
        /// </summary>
        /// <param name="entities">Entities</param>
        void Delete(IEnumerable<T> entities);
        /// <summary>
        /// Gets a table
        /// </summary>
        IEnumerable<T> Table { get; }

        /// <summary>
        /// Gets a table with "no tracking" enabled (EF feature) Use it only when you load record(s) only for read-only operations
        /// </summary>
        IEnumerable<T> TableNoTracking { get; }
        /// <summary>
        /// Gets a table with "Queryable" 
        /// </summary>
        IQueryable<T> TableQueryable { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="where"></param>
        /// <param name="orderByAsc"></param>
        /// <param name="skip"></param>
        /// <param name="take"></param>
        /// <returns></returns>
        IEnumerable<T> FindMany(Func<T, bool> where = null,
       Dictionary<Func<T, object>, bool> orderByAsc = null,
       int? skip = null, int? take = null);

        #endregion
        #region Asynchronous
        /// <summary>
        /// Get entity by identifier async
        /// </summary>
        /// <param name="id">Identifier</param>
        /// <returns>Entity</returns>
        Task<T> GetByIdAsync(int id);
        /// <summary>
        /// Insert entity async
        /// </summary>
        /// <param name="entity">Entity</param>
        Task InsertAsync(T entity);
        /// <summary>
        /// Insert entity async
        /// </summary>
        /// <param name="entities">entities</param>
        Task InsertAsync(IEnumerable<T> entities);
        /// <summary>
        /// Update entity async
        /// </summary>
        /// <param name="entity">Entity</param>
        Task UpdateAsync(T entity);
        /// <summary>
        /// Update entities async
        /// </summary>
        /// <param name="entities">Entities</param>
        Task UpdateAsync(IEnumerable<T> entities);
        /// <summary>
        /// Delete entity async
        /// </summary>
        /// <param name="entity">Entity</param>
        Task DeleteAsync(T entity);
        /// <summary>
        /// Delete entities async
        /// </summary>
        /// <param name="entities">Entities</param>
        Task DeleteAsync(IEnumerable<T> entities);
        IQueryable<T> Query();

        #endregion

    }
}