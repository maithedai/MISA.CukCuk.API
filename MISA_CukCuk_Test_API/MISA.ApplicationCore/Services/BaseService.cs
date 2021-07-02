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

        public virtual ServiceResult Import(IFormFile formFile, CancellationToken cancellationToken)
        {
            if (formFile == null || formFile.Length <= 0)
            {
                return ServiceResult.GetResult(Enums.MISACode.NotValid, "");
            }
            if (!Path.GetExtension(formFile.FileName).Equals(".xlsx", StringComparison.OrdinalIgnoreCase))
            {
                return ServiceResult.GetResult(Enums.MISACode.NotValid, "");
            }
            var list = new List<TEntity>();
            using (var stream = new MemoryStream())
            {
                formFile.CopyToAsync(stream, cancellationToken);
                using (var package = new ExcelPackage(stream))
                {
                    ExcelWorksheet worksheet = package.Workbook.Worksheets[0];
                    var rowCount = worksheet.Dimension.Rows;
                    var colCount = worksheet.Dimension.Columns;
                    for (int row = 3; row <= rowCount; row++)
                    {
                        var entity = (TEntity)Activator.CreateInstance(typeof(TEntity), new object[] { });
                        this.BuildObject(worksheet, row, colCount, ref entity);
                        list.Add(entity);
                    }
                }
            }
            ServiceResult result = new ServiceResult();
            result.Data = list;
            return result;
        }
        private void BuildObject(ExcelWorksheet worksheet, int row, int total_col, ref TEntity entity)
        {
            //Duyệt các cột trong file excel
            for (int col = 1; col <= total_col; ++col)
            {
                //Check validate trong tệp
                //Map dữ liệu trong excel với entity
                var cellValue = worksheet.Cells[row, col].Value;
                var colName = worksheet.Cells[2, col].Value;
                this.MapExcelToEntity(ref cellValue, ref entity, colName.ToString());
            }
        }
        /// <summary>
        /// Map dữ liệu từ excel với entity
        /// tdanh 6.21
        /// </summary>
        /// <param name="cellValue"></param>
        /// <param name="entity"></param>
        /// <param name="colName"></param>
        private void MapExcelToEntity(ref object cellValue, ref TEntity entity, string colName)
        {
            var properties = entity.GetType().GetProperties();
            foreach (var property in properties)
            {
                //Kiểm tra xem property đó có attribute ExcelName ko
                if (property.IsDefined(typeof(ColumnName), false))
                {
                    var attribute = property.GetCustomAttributes(typeof(ColumnName), true)[0];
                    var excelName = (attribute as ColumnName).excelName;
                    if (colName == excelName.ToString() && cellValue != null)
                    {
                        if (property.PropertyType == typeof(DateTime?))
                        {
                            property.SetValue(entity, DateTime.ParseExact(cellValue.ToString(), "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture));
                        }
                        else
                        {
                            property.SetValue(entity, cellValue.ToString());
                        }
                    }
                }
            }
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



        public bool Validate(TEntity entity)
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
