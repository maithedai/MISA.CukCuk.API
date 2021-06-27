using Microsoft.AspNetCore.Mvc;
using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data;
using MySqlConnector;
using MISA.ApplicationCore;
using MISA.ApplicationCore.Interfaces;
using MISA.ApplicationCore.Entities;
using MISA.ApplicationCore.Enums;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MISA_CukCuk_Test_API.Controllers
{
    [Route("api/v1/customers")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        ICustomerService _customerService;
        public CustomersController(ICustomerService customerService)
        {
            _customerService = customerService;
        }
        // GET: api/<CustomerController>
        [HttpGet]
        public IActionResult Get()
        {
            var customers = _customerService.GetCustomers();
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
        public IActionResult InsertCustomer(Customer customer)
        {
            /// Validate dữ liệu:
            /// check trống mã:

            var serviceResult = _customerService.AddCustomer(customer);

            if (serviceResult.MISACode == MISACode.NotValid)
                return BadRequest(serviceResult.Data);
            if (serviceResult.MISACode == MISACode.IsValid && (int)serviceResult.Data > 0)
                return Created("Thành công", customer);
            else
                return NoContent();
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
