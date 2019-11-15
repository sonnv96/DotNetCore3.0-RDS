using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using RDS.Core;
using RDS.Core.Entities.Base;
using RDS.Framework.Helpers;
using RDS.Framework.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDS.Framework.Services.Base
{
    public abstract class BaseService<T> : IBaseService<T> where T : class, IBaseEntity
    {
        protected readonly IRepository<T> _tRepository;

        public BaseService(IRepository<T> tRepository)
        {
            this._tRepository = tRepository;
        }

        #region Synchronous
        public virtual void Insert(T entity)
        {
            if (entity == null)
                throw new ArgumentNullException("enity");

            _tRepository.Insert(entity);
        }

        public virtual void Insert(List<T> entities)
        {
            foreach (var entity in entities)
            {
                if (entity == null)
                    throw new ArgumentNullException("enity");
            }

            _tRepository.Insert(entities);

        }

        public virtual void Update(T entity)
        {
            if (entity == null)
                throw new ArgumentNullException("enity");

            _tRepository.Update(entity);
        }

        public virtual void Update(List<T> entities)
        {
            foreach (var entity in entities)
            {
                if (entity == null)
                    throw new ArgumentNullException("enity");
            }

            _tRepository.Update(entities);

        }

        public virtual void Delete(T entity)
        {
            if (entity == null)
                throw new ArgumentNullException("enity");

            _tRepository.Delete(entity);

        }

        public virtual void Delete(IEnumerable<T> entities)
        {
            foreach (var entity in entities)
            {
                if (entity == null)
                    throw new ArgumentNullException("enity");
            }

            _tRepository.Delete(entities);

        }

        public virtual T GetById(int id)
        {
            if (id == 0)
                return default(T);

            return _tRepository.GetById(id);
        }

        public virtual List<T> GetByIds(List<int> ids)
        {
            return _tRepository.Table.Where(t => ids.Contains(t.Id)).ToList();
        }

        public virtual List<T> GetAll()
        {
            return _tRepository.Table.ToList();
        }

        public virtual IPagedList<T> GetAll(int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var query = _tRepository.Table.OrderBy(x => x.Id);
            return new PagedList<T>(query, pageIndex, pageSize);
        }

        public virtual bool CheckExist(Func<T, bool> fieldCheck, int? entityId = null)
        {
            var query = _tRepository.Table;
            if (entityId.HasValue)
            {
                query = query.Where(x => x.Id != entityId);
            }
            return query.Any(fieldCheck);
        }

        public virtual T FindOne(Func<T, bool> where = null)
        {
            var result = _tRepository.FindMany(where, take: 1);

            return result.FirstOrDefault();
        }

        public virtual List<T> FindMany(Func<T, bool> where = null,
            Dictionary<Func<T, object>, bool> orderByAsc = null)
        {
            return _tRepository.FindMany(where, orderByAsc: orderByAsc).ToList();
        }
        #endregion
        #region Asynchronous
        public virtual async Task InsertAsync(T entity)
        {
            if (entity == null)
                throw new ArgumentNullException("enity");

            await _tRepository.InsertAsync(entity);
        }

        public virtual async Task InsertAsync(List<T> entities)
        {
            foreach (var entity in entities)
            {
                if (entity == null)
                    throw new ArgumentNullException("enity");
            }

            await _tRepository.InsertAsync(entities);
        }

        public virtual async Task UpdateAsync(T entity)
        {
            if (entity == null)
                throw new ArgumentNullException("enity");

            await _tRepository.UpdateAsync(entity);
        }

        public virtual async Task UpdateAsync(List<T> entities)
        {
            foreach (var entity in entities)
            {
                if (entity == null)
                    throw new ArgumentNullException("enity");
            }

            await _tRepository.UpdateAsync(entities);
        }

        public virtual async Task DeleteAsync(T entity)
        {
            if (entity == null)
                throw new ArgumentNullException("enity");

            await _tRepository.DeleteAsync(entity);
        }

        public virtual async Task DeleteAsync(List<T> entities)
        {
            foreach (var entity in entities)
            {
                if (entity == null)
                    throw new ArgumentNullException("enity");
            }

            await _tRepository.DeleteAsync(entities);
        }

        public virtual async Task<T> GetByIdAsync(int id)
        {
            if (id == 0)
                return default(T);
            return await _tRepository.GetByIdAsync(id);

        }

        public virtual Task<List<T>> GetByIdsAsync(List<int> ids)
        {
            return Task.FromResult(_tRepository.Table.Where(t => ids.Contains(t.Id)).ToList());
        }

        public virtual async Task<List<T>> GetAllAsync()
        {

            return await Task.FromResult(_tRepository.Table.ToList());
        }

        public virtual async Task<IPagedList<T>> GetAllAsync(int pageIndex = 0, int pageSize = int.MaxValue)
        {

            var query = await Task.FromResult(_tRepository.Table);
            return await Task.FromResult(new PagedList<T>(query, pageIndex, pageSize) as IPagedList<T>);
        }

        public virtual async Task<bool> CheckExistAsync(Func<T, bool> fieldCheck, int? entityId = null)
        {
            var query = _tRepository.Table;
            if (entityId.HasValue)
            {
                query = query.Where(x => x.Id != entityId);
            }
            return await Task.FromResult(query.Any(fieldCheck));
        }

        public virtual async Task<T> FindOneAsync(Func<T, bool> where = null)
        {
            var result = await Task.FromResult(_tRepository.FindMany(where, take: 1));
            return result.FirstOrDefault();
        }

        public virtual async Task<List<T>> FindManyAsync(Func<T, bool> where = null,
            Dictionary<Func<T, object>, bool> orderByAsc = null)
        {
            return await Task.FromResult(_tRepository.FindMany(where, orderByAsc: orderByAsc).ToList());
        }
        #endregion

    }
}
