using RDS.Core.Entities.Base;
using RDS.Framework.Helpers;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RDS.Framework.Services.Base
{
    public interface IBaseService<T> where T : IBaseEntity
    {
        #region Synchronous
        T GetById(int id);
        List<T> GetByIds(List<int> ids);
        void Insert(T enity);
        void Insert(List<T> entities);
        void Update(T entity);
        void Update(List<T> entities);
        void Delete(T entity);
        void Delete(IEnumerable<T> entities);
        List<T> GetAll();
        IPagedList<T> GetAll(int pageIndex = 0, int pageSize = int.MaxValue);
        bool CheckExist(Func<T, bool> fieldCheck, int? entityId = null);
        T FindOne(Func<T, bool> where = null);
        List<T> FindMany(Func<T, bool> where = null,
          Dictionary<Func<T, object>, bool> orderByAsc = null);
        #endregion

        #region Asynchronous
        Task<T> GetByIdAsync(int id);
        Task<List<T>> GetByIdsAsync(List<int> ids);
        Task InsertAsync(T enity);
        Task InsertAsync(List<T> entities);
        Task UpdateAsync(T entity);
        Task UpdateAsync(List<T> entities);
        Task DeleteAsync(T entity);
        Task DeleteAsync(List<T> entities);
        Task<List<T>> GetAllAsync();
        Task<IPagedList<T>> GetAllAsync(int pageIndex = 0, int pageSize = int.MaxValue);
        Task<bool> CheckExistAsync(Func<T, bool> fieldCheck, int? entityId = null);
        Task<T> FindOneAsync(Func<T, bool> where = null);
        Task<List<T>> FindManyAsync(Func<T, bool> where = null,
            Dictionary<Func<T, object>, bool> orderByAsc = null);
        #endregion
    }
}
