using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Talabat.Core.Entities;
using Talabat.Core.Repository.Contract;
using Talabat.Repository.Data;

namespace TalabatAPIs.Controllers
{

    public class ProductsController : BaseApiController
    {
        private readonly IGenericRepository<Product> _productsRepo;

        public ProductsController(IGenericRepository<Product> productsRepo)
        {
            _productsRepo = productsRepo;
        }
        // /api/Products
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
        {
            var Products = await _productsRepo.GetAllAsync();

            return Ok(Products);
        }

        // /api/Products/1
        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            var product = await _productsRepo.GetAsync(id);
            if(product is null)
                return NotFound(new {Message="Not Found",StatusCode=404});
            return Ok(product);
        }

    }
}
