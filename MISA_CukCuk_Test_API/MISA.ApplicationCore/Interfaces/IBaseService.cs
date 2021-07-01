using Microsoft.AspNetCore.Http;
using MISA.ApplicationCore.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace MISA.ApplicationCore.Interfaces
{
    public interface IBaseService<TEntity>
    {
        /// <summary>
        /// Lấy toàn bộ
        /// </summary>
        /// <returns></returns>
        IEnumerable<TEntity> GetEntities();
        TEntity GetEntityById(Guid entityId);
        ServiceResult Add(TEntity entity);
        ServiceResult Update(TEntity entity);
        ServiceResult Delete(Guid entityId);
        ServiceResult Import(IFormFile formFile, CancellationToken cancellationToken);
    }
}
