using IdentityServer.API2.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace IdentityServer.API2.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {

        [HttpGet]
        public IActionResult GetEmployees()
        {
            var employeeList = new List<Employee>
            {
                new (){ Id = 1, Name ="Ayşe",LastName="Temel",Age=32},
                new (){ Id = 2, Name ="Ali",LastName="Yılmaz",Age=32},
                new (){ Id = 3, Name ="Fatma",LastName="Taş",Age=32},
                new (){ Id = 4, Name ="Mehmet",LastName="Yıldırım",Age=32},
                new (){ Id = 5, Name ="Ahmet",LastName="Sezgin",Age=32},
            };
            return Ok(employeeList);
        }
    }
}
