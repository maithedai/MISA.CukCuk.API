using Microsoft.AspNetCore.Http;
using MISA.ApplicationCore.Entities;
using MISA.ApplicationCore.Enums;
using MISA.ApplicationCore.Interfaces;
using MISA.ApplicationCore.Services;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using System.Threading;

namespace MISA.ApplicationCore
{
    public class CustomerService: BaseService<Customer>, ICustomerService
    {
        ICustomerRepository _customerRepository;

        #region Constructor
        public CustomerService(ICustomerRepository customerRepository) :base(customerRepository)
        {
            _customerRepository = customerRepository;
        }
        #endregion

        #region Method

        public override ServiceResult Add(Customer entity)
        {
            ServiceResult serviceResult = new ServiceResult();
            //Validate thông tin:
            var isValidate = true;
            //check trùng mã khách hàng
            var customerDuplicate = _customerRepository.GetCustomerByCode(entity.CustomerCode);
            if(customerDuplicate != null)
            {
                isValidate = false;
            }   
            //logic hàm validate:
            if (isValidate == true)
            {
                return base.Add(entity);
            } 
            else
            {
                return serviceResult;
            }
        }

        public IEnumerable<Customer> GetCustomerPaging(int limit, int offset)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Customer> GetCustomerByGroup(Guid groupId)
        {
            throw new NotImplementedException();
        }

        public override ServiceResult Import(IFormFile formFile, CancellationToken cancellationToken)
        {
            return base.Import(formFile, cancellationToken);
        }

        #endregion
    }
}
