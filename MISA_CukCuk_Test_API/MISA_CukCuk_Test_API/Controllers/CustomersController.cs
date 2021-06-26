using Microsoft.AspNetCore.Mvc;
using MISA_CukCuk_Test_API.Models;
using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data;
using MySqlConnector;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MISA_CukCuk_Test_API.Controllers
{
    [Route("api/v1/customers")]
    [ApiController]
    public class CustomersController : ControllerBase
    {     
        // GET: api/<CustomerController>
        [HttpGet]
        public IActionResult Get()
        {
            var connectionString = "User Id=dev;Host=47.241.69.179;Port=3306;Database='MISACukCuk_Demo';Password=12345678;Character Set=utf8";
            IDbConnection dbConnection = new MySqlConnection(connectionString);
            var customers = dbConnection.Query<Customer>("Proc_GetCustomers", commandType: CommandType.StoredProcedure);
            return Ok(customers);
        }

        // GET api/<CustomerController>/5
        [HttpGet("{id}")]
        public IActionResult Get(string id)
        {
            var connectionString = "User Id=dev;Host=47.241.69.179;Port=3306;Database='MISACukCuk_Demo';Password=12345678;Character Set=utf8";
            IDbConnection dbConnection = new MySqlConnection(connectionString);
            var customers = dbConnection.Query<Customer>("Proc_GetCustomerById", new { CustomerId = id }, commandType: CommandType.StoredProcedure);
             
            return Ok(customers);
        }

        // POST api/<CustomerController>
        [HttpPost]
        public IActionResult Post(Customer customer)
        {
            /// Validate dữ liệu:
            /// check trống mã:

            var customerCode = customer.CustomerCode;
            if(string.IsNullOrEmpty(customerCode))
            {
                var msg = new
                {
                    devMsg = new
                    {
                        FieldName = "CustomerCode",
                        msg = "Mã khách hàng không được để trống",
                        Code = 999,
                    },
                };
                return BadRequest(msg);
            }

            //Check trùng mã:
            var connectionString = "User Id=dev;Host=47.241.69.179;Port=3306;Database='MISACukCuk_Demo';Password=12345678;Character Set=utf8";
            IDbConnection dbConnection = new MySqlConnection(connectionString);
            var res = dbConnection.Query   ("Proc_InsertCustomer", new { CustomerCode = customerCode }, commandType: CommandType.StoredProcedure);

            var properties = customer.GetType().GetProperties();
            var parmaster = new DynamicParameters();
            foreach(var property in properties)
            {
                var propertyName = property.Name;
                var propertyValue = property.GetValue(customer);
                var propertyType = property.PropertyType;
                if(propertyType == typeof(Guid) || propertyType == typeof(Guid?))
                {
                    parmaster.Add($"{propertyName}", propertyValue, DbType.String);
                }
                else
                {
                    parmaster.Add($"{propertyName}", propertyValue);
                }
            }

            var connectionString = "User Id=dev;Host=47.241.69.179;Port=3306;Database='MISACukCuk_Demo';Password=12345678;Character Set=utf8";
            IDbConnection dbConnection = new MySqlConnection(connectionString);
            var rowAffects = dbConnection.Execute("Proc_InsertCustomer", parmaster, commandType: CommandType.StoredProcedure);

            return Ok(rowAffects);
        }

        // PUT api/<CustomerController>/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] string value)
        {
            return Ok();
        }

        // DELETE api/<CustomerController>/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            return Ok();
        }
    }
}
