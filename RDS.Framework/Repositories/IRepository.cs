using Microsoft.EntityFrameworkCore.Storage;
using RDS.Core.Entities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace RDS.Framework.Repositories
{
    public interface IRepository<T> where T : BaseEntity
    {
        IQueryable<T> Query();

        IDbContextTransaction BeginTransaction();

        Task InsertAsync(T entity);

        Task InsertAsync(IEnumerable<T> entities);

        Task UpdateAsync(T entity);

        Task UpdateAsync(IEnumerable<T> entities);

        Task UpdateAsync(Expression<Func<T, bool>> predicate, Expression<Func<T, T>> updateFactory);

        Task DeleteAsync(T entity);

        Task DeleteAsync(IEnumerable<T> entities);

        Task DeleteAsync(Expression<Func<T, bool>> predicate);

        Task LoadLocalizableTranslation(T entity, string language, bool overrideEvenIfLocalizaEmpty);

        Task LoadLocalizableTranslation(IEnumerable<T> entities, string language, bool overrideEvenIfLocalizaEmpty);

        Task<Dictionary<string, object>> GetLocalizableTranslation(int entityId, string language);

        Task<Dictionary<int, Dictionary<string, object>>> GetLocalizableTranslation(IEnumerable<int> entityIds, string language);

        Task InsertUpdateLocalizableTranslation(int entityId, Dictionary<string, Dictionary<string, object>> localizableData);

        Task InsertUpdateLocalizableTranslation(IEnumerable<(int entityId, Dictionary<string, Dictionary<string, object>> localizableData)> localizableDatas);

        Task InsertUpdateLocalizableTranslation(T entity, string language);
    }
}
