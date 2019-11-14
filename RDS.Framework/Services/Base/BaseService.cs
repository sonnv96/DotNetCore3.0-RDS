using RDS.Core.Entities.Base;
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

        protected abstract string PatternKey { get; }
        #region Synchronous
        
        public virtual async Task<bool> CheckExistAsync(Func<T, bool> fieldCheck, int? entityId = null)
        {
            var query = _tRepository.Query();
            if (entityId.HasValue)
            {
                query = query.Where(x => x.Id != entityId);
            }
            return await Task.FromResult(query.Any(fieldCheck));
        }

      
        #endregion

    }
}
