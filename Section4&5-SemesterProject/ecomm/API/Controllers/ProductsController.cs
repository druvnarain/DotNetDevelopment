using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Infrastructure.Database;
using Core.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
//note: controllers are created by each session
namespace API.Controllers
{
    [ApiController]
    [Route("api/[Controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly StoreContext _context;
        public ProductsController(StoreContext context)
        {
            //dbcontext is injected into controller with scoped lifetime
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<Product>>> GetProducts()
        {
            var products = await _context.Products.ToListAsync();
            return Ok(products);
        }

        //https://localhost:5001/api/products/2 would return this
        // whatever number in url is passed into int id
        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            return await _context.Products.FindAsync(id);
            //return Ok(product);
        }
    }
}