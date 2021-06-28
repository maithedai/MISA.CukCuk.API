using MISA.ApplicationCore.Entities;
using MISA.ApplicationCore.Enums;
using MISA.ApplicationCore.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace MISA.ApplicationCore
{
    public class CustomerService: ICustomerService
    {
        ICustomerRepository _customerRepository;

        #region Constructor
        public CustomerService(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        public ServiceResult UpdateCustomer(Customer customer)
        {
            throw new NotImplementedException();
        }

        public ServiceResult AddCustomer(Customer customer)
        {
            throw new NotImplementedException();
        }

        public ServiceResult DeleteCustomer(Guid customerId)
        {
            throw new NotImplementedException();
        }

        public Customer GetCustomerById(Guid customerId)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region Method

        // Lấy danh sách khách hàng:
        public IEnumerable<Customer> GetCustomers()
        {
            var customers = _customerRepository.GetCustomers();
            return customers;
        }

        //Thêm mới khách hàng:
        public ServiceResult InsertCustomer(Customer customer)
        {
            var serviceResult = new ServiceResult();

            /// Validate dữ liệu:
            /// Check trường bắt buộc nhập, Nếu dữ liệu chưa hợp lệ thì mô tả lỗi

            var customerCode = customer.CustomerCode;
            if (string.IsNullOrEmpty(customerCode))
            {
                var msg = new
                {
                    devMsg = new
                    {
                        FieldName = "CustomerCode",
                        msg = "Mã khách hàng không được để trống",
                    },
                    userMsg = "Mã khách hàng không được để trống",
                    code = 900,
                };

                serviceResult.MISACode = MISACode.NotValid; 
                serviceResult.Messenger = "Mã khách hàng không được phép để trống";
                serviceResult.Data = msg;
                return serviceResult;
            }

            //Check trùng mã:
            var res = _customerRepository.GetCustomerByCode(customerCode);
            if (res != null)
            {
                var msg = new
                {
                    devMsg = new
                    {
                        FieldName = "CustomerCode",
                        msg = "Mã khách hàng đã tồn tại",
                    },
                    userMsg = "Mã khách hàng đã tồn tại",
                    code = MISACode.NotValid,
                };

                serviceResult.MISACode = MISACode.NotValid;
                serviceResult.Messenger = "Mã khách hàng không được phép để trống";
                serviceResult.Data = msg;
                return serviceResult;
            }

            //Nếu dữ liệu hợp lệ thì thêm mới
            var rowAffects = _customerRepository.AddCustomer(customer);
            serviceResult.MISACode = MISACode.IsValid;
            serviceResult.Messenger = "Thêm thành công";
            serviceResult.Data = rowAffects;
            return serviceResult;
        }

        //Sửa khách hàng:

        //Xóa khách hàng:

        #endregion
    }
}
