using Dapper;
using MISA.ApplicationCore.Entities;
using MISA.ApplicationCore.Interfaces;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace MISA.Infrarstructure
{
    public class CustomerRepository : ICustomerRepository
    {
        public int AddCustomer(Customer customer)
        {
            //Khởi tạo kết nối với db:
            var connectionString = "User Id=dev;Host=47.241.69.179;Port=3306;Database='MISACukCuk_Demo';Password=12345678;Character Set=utf8";
            var dbConnection = new MySqlConnection(connectionString);
            var properties = customer.GetType().GetProperties();
            var parmaster = new DynamicParameters();

            //Xử lý các kiểu dữ liệu (mapping data)
            foreach (var property in properties)
            {
                var propertyName = property.Name;
                var propertyValue = property.GetValue(customer);
                var propertyType = property.PropertyType;
                if (propertyType == typeof(Guid) || propertyType == typeof(Guid?))
                {
                    parmaster.Add($"{propertyName}", propertyValue, DbType.String);
                }
                else
                {
                    parmaster.Add($"{propertyName}", propertyValue);
                }
            }

            //Thực thi command Text
            var rowAffects = dbConnection.Execute("Proc_InsertCustomer", parmaster, commandType: CommandType.StoredProcedure);

            //Trả về số bản ghi thêm mới đc
            return rowAffects;
        }

        public int AddCustomer(Customer customer)
        {
            throw new NotImplementedException();
        }

        public int DeleteCustomer(Guid customerId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Customer> GetCustomer()
        {
            // Kết nối tới CSDL:
            var connectionString = "User Id=dev;Host=47.241.69.179;Port=3306;Database='MISACukCuk_Demo';Password=12345678;Character Set=utf8";
            IDbConnection dbConnection = new MySqlConnection(connectionString);

            // Khởi tạo các commandText:
            var customers = dbConnection.Query<Customer>("Proc_GetCustomers", commandType: CommandType.StoredProcedure);

            // Trả về dữ liệu
            return customers;
        }

        public Customer GetCustomerByCode(string customerCode)
        {
            throw new NotImplementedException();
        }

        public Customer GetCustomerById(Guid customerId)
        {
            // Kết nối tới CSDL:
            var connectionString = "User Id=dev;Host=47.241.69.179;Port=3306;Database='MISACukCuk_Demo';Password=12345678;Character Set=utf8";
            IDbConnection dbConnection = new MySqlConnection(connectionString);

            // Khởi tạo các commandText:
            var customers = dbConnection.Query<Customer>("Proc_GetCustomerById", new { CustomerId = customerId }, commandType: CommandType.StoredProcedure).FirstOrDefault();

            // Trả về dữ liệu
            return customers;
        }

        public IEnumerable<Customer> GetCustomers()
        {
            throw new NotImplementedException();
        }

        public int UpdateCustomer(Customer customer)
        {
            //Khởi tạo kết nối với db:
            var connectionString = "User Id=dev;Host=47.241.69.179;Port=3306;Database='MISACukCuk_Demo';Password=12345678;Character Set=utf8";
            var dbConnection = new MySqlConnection(connectionString);
            var properties = customer.GetType().GetProperties();
            var parmaster = new DynamicParameters();

            //Xử lý các kiểu dữ liệu (mapping data)
            foreach (var property in properties)
            {
                var propertyName = property.Name;
                var propertyValue = property.GetValue(customer);
                var propertyType = property.PropertyType;
                if (propertyType == typeof(Guid) || propertyType == typeof(Guid?))
                {
                    parmaster.Add($"{propertyName}", propertyValue, DbType.String);
                }
                else
                {
                    parmaster.Add($"{propertyName}", propertyValue);
                }
            }

            //Thực thi command Text
            var rowAffects = dbConnection.Execute("Proc_UpdateCustomer", parmaster, commandType: CommandType.StoredProcedure);

            //Trả về số bản ghi thêm mới đc
            return rowAffects;
        }

        public int UpdateCustomer(Customer customer)
        {
            throw new NotImplementedException();
        }

        Customer ICustomerRepository.GetCustomerById(Guid customerId)
        {
            throw new NotImplementedException();
        }

        IEnumerable<Customer> ICustomerRepository.GetCustomers()
        {
            throw new NotImplementedException();
        }
    }
}
