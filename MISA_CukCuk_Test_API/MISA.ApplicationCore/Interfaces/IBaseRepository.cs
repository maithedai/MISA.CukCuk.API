using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace MISA.ApplicationCore.Interfaces
{
    public interface IBaseRepository<TEntity>
    {
        /// <summary>
        /// Lấy toàn bộ
        /// </summary>
        /// <returns></returns>
        IEnumerable<TEntity> GetEntities();
        TEntity GetEntityById(Guid employeeId);
        int Add(TEntity employee);
        int Update(TEntity employee);
        int Delete(Guid employeeId);
        TEntity GetEntityByProperty(TEntity entity, PropertyInfo property);
    }
}
