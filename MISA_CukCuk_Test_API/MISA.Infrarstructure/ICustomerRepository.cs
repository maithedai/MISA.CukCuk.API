using MISA.Infrarstructure.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace MISA.Infrarstructure
{
    public interface ICustomerRepository
    {
        IEnumerable<Customer> GetCustomers();
        Customer GetCustomerById(Guid customerId);
        int AddCustomer(Customer customer);
        int UpdateCustomer(Customer customer);
        int DeleteCustomer(Guid customerId);
    }
}
