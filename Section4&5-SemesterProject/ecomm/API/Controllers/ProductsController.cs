using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Infrastructure.Database;
using Core.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Core.Interfaces;
using Core.Specifications;

//note: controllers are created by each session
namespace API.Controllers
{
    [ApiController]
    [Route("api/[Controller]")]
    public class ProductsController : ControllerBase
    {
        //private readonly IProductRepository _repo;      replaced by genericrepository
        private readonly IGenericRepository<Product> _productsRepo;
        private readonly IGenericRepository<ProductBrand> _brandsRepo;
        private readonly IGenericRepository<ProductType> _typesRepo;

        public ProductsController(IGenericRepository<Product> productsRepo,
        IGenericRepository<ProductBrand> brandsRepo, IGenericRepository<ProductType> typesRepo)
        {
            _typesRepo = typesRepo;
            _brandsRepo = brandsRepo;
            _productsRepo = productsRepo;
            //_repo = repo;    replaced with generic repos
            //repo is injected into controller with scoped lifetime
        }

    [HttpGet]
    public async Task<ActionResult<List<Product>>> GetProducts()
    {
        //Generic ListAllAsync inherits Product type from _productsRepo
        //var products = await _productsRepo.ListAllAsync();
        var spec = new ProductsWithTypesAndBrandsSpecification();
        var products = await _productsRepo.ListAsync(spec);
        return Ok(products);
    }

    //https://localhost:5001/api/products/2 would return this
    // whatever number in url is passed into int id
    [HttpGet("{id}")]
    public async Task<ActionResult<Product>> GetProduct(int id)
    {
        //return await _productsRepo.GetByIdAsync(id);
        //return Ok(product);
        var spec = new ProductsWithTypesAndBrandsSpecification(id);
        return await _productsRepo.GetEntityWithSpec(spec);
    }

    //When wrapping IReadOnlyList with ActionResult, must return with Ok(...)
    [HttpGet("brands")]
    public async Task<ActionResult<IReadOnlyList<ProductBrand>>> GetProductBrands()
    {
        return Ok(await _brandsRepo.ListAllAsync());
    }

    [HttpGet("types")]
    public async Task<ActionResult<IReadOnlyList<ProductType>>> GetProductTypes()
    {
        return Ok(await _typesRepo.ListAllAsync());
    }
}
}