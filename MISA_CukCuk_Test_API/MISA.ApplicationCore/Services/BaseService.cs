using Microsoft.AspNetCore.Http;
using MISA.ApplicationCore.Entities;
using MISA.ApplicationCore.Interfaces;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MISA.ApplicationCore.Services
{
    public class BaseService<TEntity> : IBaseService<TEntity> where TEntity: BaseEntity
    {
        IBaseRepository<TEntity> _baseRepository;
        ServiceResult _serviceResult;
        public BaseService(IBaseRepository<TEntity> baseRepository)
        {
            _baseRepository = baseRepository;
            _serviceResult = new ServiceResult() { MISACode = Enums.MISACode.Success };
        }
        public virtual ServiceResult Add(TEntity entity)
        {
            entity.EntityState = Enums.EntityState.AddNew;
            ServiceResult serviceResult = new ServiceResult();
            // Thực hiện validate
            var isValidate = Validate(entity);
            if(isValidate == true)
            {
                _serviceResult.Data = _baseRepository.Add(entity);
                _serviceResult.MISACode = Enums.MISACode.IsValid;
                return _serviceResult;
            }
            else
            {
                _serviceResult.Data = _baseRepository.Add(entity);
                _serviceResult.Messenger = "Dữ liệu không hợp lệ, vui lòng kiểm tra lại";
                _serviceResult.MISACode = Enums.MISACode.NotValid;
                return _serviceResult;
            }
            
        }

        public ServiceResult Delete(Guid entityId)
        {
            _serviceResult.Data = _baseRepository.Delete(entityId);
            return _serviceResult;
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
            entity.EntityState = Enums.EntityState.Update;
            var isValidate = Validate(entity);
            if (isValidate == true)
            {
                _serviceResult.Data = _baseRepository.Update(entity);
                _serviceResult.MISACode = Enums.MISACode.IsValid;
                return _serviceResult;
            }
            else 
            {
                return _serviceResult;
            }
        }



        private bool Validate(TEntity entity)
        {
            var mesArrayError = new List<string>();
            var isValidate = true;
            ServiceResult serviceResult = new ServiceResult();
            //Đọc các property:
            var properties = entity.GetType().GetProperties();
            foreach(var property in properties)
            {
                var propertyValue = property.GetValue(entity);
                var displayName = property.GetCustomAttributes(typeof(DisplayNameAttribute), true);
                //Kiểm tra xem có attr cần phải validate ko:
                if (property.IsDefined(typeof(Required), false))
                {
                    //Check bắt buộc nhập:
                    
                    if(propertyValue == null)
                    {
                        isValidate = false;
                        mesArrayError.Add($"Thông tin {displayName} không được phép để trống");
                        _serviceResult.MISACode = Enums.MISACode.NotValid;
                        _serviceResult.Messenger = "Dữ liệu không hơp lệ";
                    }
                }
                if(property.IsDefined(typeof(CheckDuplicate), false))
                {
                    //check trùng dữ liệu
                    var propertyName = property.Name;
                    var entityDuplicate = _baseRepository.GetEntityByProperty(entity, property);
                    if(entityDuplicate != null)
                    {
                        isValidate = false;
                        mesArrayError.Add($"Thông tin {displayName} đã tồn tại trên hệ thống");
                        _serviceResult.MISACode = Enums.MISACode.NotValid;
                        _serviceResult.Messenger = "Dữ liệu không hơp lệ";
                    }
                }
            }
            serviceResult.Data = mesArrayError;
            return isValidate;
        }
    }
}
