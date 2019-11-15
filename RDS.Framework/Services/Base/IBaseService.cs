using RDS.Core.Entities.Base;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RDS.Framework.Services.Base
{
    public interface IBaseService<T> where T : BaseEntity
    {
        Task<T> FindOne(int id);
    }
}
