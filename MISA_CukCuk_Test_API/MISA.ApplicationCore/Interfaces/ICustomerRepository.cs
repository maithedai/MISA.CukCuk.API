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
    public interface ICustomerRepository:IBaseRepository<Customer>
    {
        /// <summary>
        /// Lấy thông tin khách hàng theo mã khách hàng
        /// </summary>
        /// <param name="customerCode"></param>
        /// <returns></returns>
        Customer GetCustomerByCode(string customerCode);
    }
}
