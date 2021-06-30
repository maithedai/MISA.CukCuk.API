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
using Microsoft.AspNetCore.Http;
using System.Threading;
using System.IO;
using OfficeOpenXml;
using System.Globalization;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MISA_CukCuk_Test_API.Controllers
{
    public class CustomersController : BaseEntityController<Customer>
    {
        IBaseService<Customer> _baseService;
        public CustomersController(IBaseService<Customer> baseService) : base(baseService)
        {
            _baseService = baseService;
        }     
    }
}
