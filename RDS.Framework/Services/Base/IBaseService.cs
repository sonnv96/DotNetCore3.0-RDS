using RDS.Core.Entities.Base;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RDS.Framework.Services.Base
{
    public interface IBaseService<T> where T : IBaseEntity 
    {
        #region Synchronous
       
        Task<bool> CheckExistAsync(Func<T, bool> fieldCheck, int? entityId = null);
        #endregion
    }
}
