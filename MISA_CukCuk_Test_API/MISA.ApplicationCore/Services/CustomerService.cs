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
            ServiceResult serviceResult = new ServiceResult();

            if (formFile == null || formFile.Length <= 0)
            {
                return serviceResult;
            }

            if (!Path.GetExtension(formFile.FileName).Equals(".xlsx", StringComparison.OrdinalIgnoreCase))
            {
                return serviceResult;
            }

            var list = new List<Customer>();

            using (var stream = new MemoryStream())
            {
                formFile.CopyToAsync(stream, cancellationToken);
                using (var package = new ExcelPackage(stream))
                {
                    ExcelWorksheet worksheet = package.Workbook.Worksheets[0];
                    var rowCount = worksheet.Dimension.Rows;

                    for (int row = 3; row <= rowCount; row++)
                    {
                        var customer = new MISA.ApplicationCore.Entities.Customer();
                        customer.CustomerCode = worksheet.Cells[row, 1].Value.ToString().Trim();
                        customer.FullName = worksheet.Cells[row, 2].Value.ToString().Trim();
                        customer.PhoneNumber = worksheet.Cells[row, 5].Value.ToString().Trim();
                        if (worksheet.Cells[row, 6].Value != null)
                        {
                            customer.DateOfBirth = DateTime.ParseExact(worksheet.Cells[row, 6].Value.ToString(), "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                        }

                        customer.CompanyName = worksheet.Cells[row, 7].Value.ToString().Trim();
                        customer.CompanyTaxCode = worksheet.Cells[row, 8].Value.ToString().Trim();
                        if (worksheet.Cells[row, 9].Value != null)
                        {
                            customer.Email = worksheet.Cells[row, 9].Value.ToString().Trim();
                        }

                        customer.Aderss = worksheet.Cells[row, 10].Value.ToString().Trim();
                        customer.Note = worksheet.Cells[row, 11].Value.ToString().Trim();


                        base.Validate(customer);
                        list.Add(customer);

                    }
                }
            }
            serviceResult.Data = list;

            return serviceResult;
        }

        #endregion
    }
}
