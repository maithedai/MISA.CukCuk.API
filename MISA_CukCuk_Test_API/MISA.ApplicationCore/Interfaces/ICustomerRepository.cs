using MISA.ApplicationCore.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace MISA.ApplicationCore.Interfaces
{
    /// <summary>
    /// Interface danh mục khách hàng
    /// </summary>
    /// CreateBy: MTDAI 27.06.2021
    public interface ICustomerRepository
    {
        /// <summary>
        /// Lấy danh sách khách hàng
        /// </summary>
        /// <returns>Danh sách khách hàng</returns>
        /// CreateBy: MTDAI 27.06.2021
        IEnumerable<Customer> GetCustomers();
        Customer GetCustomerById(Guid customerId);
        int AddCustomer(Customer customer);
        int UpdateCustomer(Customer customer);
        int DeleteCustomer(Guid customerId);
        Customer GetCustomerByCode(string customerCode);
    }
}
