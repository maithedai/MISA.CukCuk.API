using Dapper;
using Microsoft.Extensions.Configuration;
using MISA.ApplicationCore.Entities;
using MISA.ApplicationCore.Interfaces;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;

namespace MISA.Infrarstructure
{
    public class CustomerRepository : ICustomerRepository
    {
        #region DECLARE
        IConfiguration _configuration;
        string _connectionString = string.Empty;
        IDbConnection _dbConnection = null;
        #endregion

        public CustomerRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString("MISACukCukConnectionString");
            _dbConnection = new MySqlConnection(_connectionString);
        }
        public int AddCustomer(Customer customer)
        {
            //Khởi tạo kết nối với db:


            //Xử lý các kiểu dữ liệu (mapping data)
            var parmaster = MappingDbType(customer);

            //Thực thi command Text
            var rowAffects = _dbConnection.Execute("Proc_InsertCustomer", parmaster, commandType: CommandType.StoredProcedure);

            //Trả về số bản ghi thêm mới đc
            return rowAffects;
        }

        public int DeleteCustomer(Guid customerId)
        {
            var res = _dbConnection.Execute("Proc_DeleteCustomerById", new { CustomerId = customerId }, commandType: CommandType.StoredProcedure);
            return res;
        }

        public IEnumerable<Customer> GetCustomers()
        {
            // Kết nối tới CSDL:

            // Khởi tạo các commandText:
            var customers = _dbConnection.Query<Customer>("Proc_GetCustomers", commandType: CommandType.StoredProcedure);

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

            // Khởi tạo các commandText:
            var customers = _dbConnection.Query<Customer>("Proc_GetCustomerById", new { CustomerId = customerId }, commandType: CommandType.StoredProcedure).FirstOrDefault();

            // Trả về dữ liệu
            return customers;
        }

        public int UpdateCustomer(Customer customer)
        {
            //Khởi tạo kết nối với db:
            

            //Xử lý các kiểu dữ liệu (mapping data)
            var parmaster = MappingDbType(customer);

            //Thực thi command Text
            var rowAffects = _dbConnection.Execute("Proc_UpdateCustomer", parmaster, commandType: CommandType.StoredProcedure);

            //Trả về số bản ghi thêm mới đc
            return rowAffects;
        }

        Customer ICustomerRepository.GetCustomerById(Guid customerId)
        {
            throw new NotImplementedException();
        }

        IEnumerable<Customer> ICustomerRepository.GetCustomers()
        {
            return _dbConnection.Query<Customer>("Proc_GetCustomers", commandType: CommandType.StoredProcedure);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        private DynamicParameters MappingDbType<TEntity>(TEntity entity)
        {
            var properties = entity.GetType().GetProperties();
            var parmaster = new DynamicParameters();
            foreach (var property in properties)
            {               
                var propertyName = property.Name;
                var propertyValue = property.GetValue(entity);
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
            return parmaster;
        }
    }
}
