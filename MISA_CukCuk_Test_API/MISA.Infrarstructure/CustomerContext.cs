using Dapper;
using MISA.ApplicationCore.Entities;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace MISA.Infrarstructure
{
    public class CustomerContext
    {
        #region Methods

        /// <summary>
        /// Lấy toàn bộ danh sách khách hàng
        /// </summary>
        /// <returns>Danh sách khách hàng</returns>
        /// CreateBy: MTDAI 26.06.2021
        public IEnumerable<Customer> GetCustomers()
        {
            //Kết nối tới CSDL:
            var connectionString = "User Id=dev;Host=47.241.69.179;Port=3306;Database='MISACukCuk_Demo';Password=12345678;Character Set=utf8";
            IDbConnection dbConnection = new MySqlConnection(connectionString);

            //Khởi tạo commandText:
            var customers = dbConnection.Query<Customer>("Proc_GetCustomers", commandType: CommandType.StoredProcedure);

            //Trả về dữ liệu:
            return customers;  
        }

        //Lấy thông tin khách hàng theo mã khách hàng:


        /// <summary>
        /// Thêm mới khách hàng:
        /// </summary>
        /// <param name="customer">Object khách hàng</param>
        /// <returns>Số bản ghi bị ảnh hưởng</returns>
        /// CreatedBy MTDAI 26.06.2021
        public int InsertCustomer(Customer customer)
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

        /// <summary>
        /// Lấy khách hàng theo mã khách hàng
        /// </summary>
        /// <param name="customerCode">Mã khách hàng </param>
        /// <returns>Object khách hàng đầu tiên lấy được</returns>
        /// CreateBy: MTDAI 26.06.2021
        public Customer GetCustomerByCode(string customerCode)
        {
            var connectionString = "User Id=dev;Host=47.241.69.179;Port=3306;Database='MISACukCuk_Demo';Password=12345678;Character Set=utf8";
            IDbConnection dbConnection = new MySqlConnection(connectionString);
            var res = dbConnection.Query<Customer>("Proc_GetCustomerByCode", new { CustomerCode = customerCode }, commandType: CommandType.StoredProcedure).FirstOrDefault();
            return res;
        }

        //Sửa thông tin khách hàng:

        //Xóa thông tin khách hàng theo khóa chính:

        #endregion
    }
}
