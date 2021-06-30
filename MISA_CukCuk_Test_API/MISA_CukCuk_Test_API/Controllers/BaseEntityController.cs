using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MISA.ApplicationCore.Entities;
using MISA.ApplicationCore.Interfaces;
using MySqlConnector;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MISA_CukCuk_Test_API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class BaseEntityController<TEntity> : ControllerBase
    {
        IBaseService<TEntity> _baseService;
        public BaseEntityController(IBaseService<TEntity> baseService)
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
        public IActionResult Post(TEntity entity)
        {   
            var rowAffects = _baseService.Add(entity);
            return Ok(rowAffects);
        }

        // PUT api/<CustomerController>/
        [HttpPut("{id}")]
        public IActionResult Put([FromRoute]string id, [FromBody] TEntity entity)
        {
            var keyProperty = entity.GetType().GetProperty($"{typeof(TEntity).Name}Id");
            if(keyProperty.PropertyType == typeof(Guid))
            {
                keyProperty.SetValue(entity, Guid.Parse(id));                    
            }
            else if(keyProperty.PropertyType == typeof(int))
            {
                keyProperty.SetValue(entity, int.Parse(id));
            }    
            else
            {
                keyProperty.SetValue(entity, id);
            }
            var rowAffects = _baseService.Update(entity);
            return Ok(rowAffects);
        }

        // DELETE api/<CustomerController>/5
        [HttpDelete("{id}")]
        public IActionResult Delete(Guid id)
        {
            var res = _baseService.Delete(id);
            return Ok(res);
        }

        [HttpPost("import")]
        public async Task<DemoResponse<List<MISA.ApplicationCore.Entities.Customer>>> Import(IFormFile formFile, CancellationToken cancellationToken)
        {
            if (formFile == null || formFile.Length <= 0)
            {
                return DemoResponse<List<MISA.ApplicationCore.Entities.Customer>>.GetResult(-1, "formfile is empty");
            }

            if (!Path.GetExtension(formFile.FileName).Equals(".xlsx", StringComparison.OrdinalIgnoreCase))
            {
                return DemoResponse<List<MISA.ApplicationCore.Entities.Customer>>.GetResult(-1, "Not Support file extension");
            }

            var list = new List<MISA.ApplicationCore.Entities.Customer>();

            using (var stream = new MemoryStream())
            {
                await formFile.CopyToAsync(stream, cancellationToken);

                using (var package = new ExcelPackage(stream))
                {
                    ExcelWorksheet worksheet = package.Workbook.Worksheets[0];
                    var rowCount = worksheet.Dimension.Rows;

                    for (int row = 3; row <= rowCount; row++)
                    {
                        var entity = new MISA.ApplicationCore.Entities.Customer();
                        entity.CustomerCode = worksheet.Cells[row, 1].Value.ToString().Trim();
                        entity.FullName = worksheet.Cells[row, 2].Value.ToString().Trim();
                        entity.PhoneNumber = worksheet.Cells[row, 5].Value.ToString().Trim();
                        if (worksheet.Cells[row, 6].Value != null)
                        {
                            entity.DateOfBirth = DateTime.Parse(worksheet.Cells[row, 6].Value.ToString().Trim(), CultureInfo.InvariantCulture);

                        }

                        entity.CompanyName = worksheet.Cells[row, 7].Value.ToString().Trim();
                        entity.CompanyTaxCode = worksheet.Cells[row, 8].Value.ToString().Trim();
                        if (worksheet.Cells[row, 9].Value != null)
                        {
                            entity.Email = worksheet.Cells[row, 9].Value.ToString().Trim();
                        }

                        entity.Aderss = worksheet.Cells[row, 10].Value.ToString().Trim();
                        entity.Note = worksheet.Cells[row, 11].Value.ToString().Trim();

                        list.Add(entity);
                    }
                }
            }

            // add list to db ..  
            // here just read and return  

            return DemoResponse<List<MISA.ApplicationCore.Entities.Customer>>.GetResult(0, "OK", list);
        }
    }
}
