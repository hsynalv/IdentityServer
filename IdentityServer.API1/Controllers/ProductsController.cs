using IdentityServer.API1.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace IdentityServer.API1.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        [Authorize(Policy = "ReadProduct")]
        [HttpGet]
        public IActionResult GetProducts()
        {
            var productList = new List<Product>
            {
                new () { Id = 1, Name = "Kalem", Price=100,Stock=15},
                new () { Id = 2, Name = "Silgi", Price=50,Stock=15},
                new () { Id = 3, Name = "Defter", Price=250,Stock=15},
                new () { Id = 4, Name = "Kitap", Price=150,Stock=15}
            };

            return Ok(productList);
        }

        [Authorize(Policy = "UpdateOrCreate")]
        [HttpPut("{id}")]
        public IActionResult UpdateProduct(int id)
        {
            return Ok($"ID değeri {id} olan product güncellenmiştir.");
        }

        [Authorize(Policy = "UpdateOrCreate")]
        [HttpPost]
        public IActionResult CreateProduct(Product product)
        {
            return Ok(product);
        }
    }
}
