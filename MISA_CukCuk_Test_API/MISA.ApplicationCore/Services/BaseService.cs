using MISA.ApplicationCore.Entities;
using MISA.ApplicationCore.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace MISA.ApplicationCore.Services
{
    public abstract class BaseService<TEntity> : IBaseService<TEntity>
    {
        IBaseRepository<TEntity> _baseRepository;
        public BaseService(IBaseRepository<TEntity> baseRepository)
        {
            _baseRepository = baseRepository;
        }
        public virtual ServiceResult Add(TEntity entity)
        {
            ServiceResult serviceResult = new ServiceResult();
            // Thực hiện validate
            var isValidate = Validate(entity);
            if(isValidate == true)
            {
                serviceResult.Data = _baseRepository.Add(entity);
                serviceResult.MISACode = Enums.MISACode.IsValid;
                return serviceResult;
            }
            else
            {
                serviceResult.Data = _baseRepository.Add(entity);
                serviceResult.Messenger = "Dữ liệu không hợp lệ, vui lòng kiểm tra lại";
                serviceResult.MISACode = Enums.MISACode.NotValid;
                return serviceResult;
            }
            
        }

        public ServiceResult Delete(Guid entityId)
        {
            return _baseRepository.Delete(entityId);
        }

        public IEnumerable<TEntity> GetEntities()
        {
            return _baseRepository.GetEntities();
        }

        public TEntity GetEntityById(Guid entityId)
        {
            return _baseRepository.GetEntityById(entityId);
        }

        public ServiceResult Update(TEntity entity)
        {
            return _baseRepository.Update(entity);
        }
        private bool Validate(TEntity entity)
        {
            var isValidate = true;
            //Đọc các property:
            var properties = entity.GetType().GetProperties();
            foreach(var property in properties)
            {
                //Kiểm tra xem có attr cần phải validate ko:
                if(property.IsDefined(typeof(Required), false))
                {
                    //Check bắt buộc nhập:
                    var propertyValue = property.GetValue(entity);
                    if(propertyValue == null)
                    {
                        isValidate = false;
                    }
                }
                if(property.IsDefined(typeof(CheckDuplicate), false))
                {
                    //check trùng dữ liệu
                    var propertyName = property.Name;
                    var entityDuplicate = _baseRepository.GetEntityByProperty(property.Name, property.GetValue(entity));
                    if(entityDuplicate != null)
                    {
                        isValidate = false;
                    }
                }
            }
            return isValidate;
        }
    }
}
