using Dapper;
using Microsoft.AspNetCore.Mvc;
using MISA.ApplicationCore.Interfaces;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MISA_CukCuk_Test_API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class BaseEtityController<TEntity> : ControllerBase
    {
        IBaseService<TEntity> _baseService;
        public BaseEtityController(IBaseService<TEntity> baseService)
        {
            _baseService = baseService;
        }
        // GET: api/<CustomerController>
        [HttpGet]
        public IActionResult Get()
        {
            var entities = _baseService.GetEntities();
            return Ok(entities);
        }

        // GET api/<CustomerController>/5
        [HttpGet("{id}")]
        public IActionResult Get(string id)
        {
            var entity = _baseService.GetEntityById(Guid.Parse(id));

            return Ok(entity);
        }

        // POST api/<CustomerController>
        [HttpPost]
        public IActionResult InsertCustomer(TEntity entity)
        {
            /// Validate dữ liệu:
            /// check trống mã:

            var rowAffects = _baseService.Add(entity);
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
