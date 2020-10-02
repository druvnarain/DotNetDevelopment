using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Infrastructure.Database;
using Core.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Core.Interfaces;
using Core.Specifications;
using API.Dtos;
using AutoMapper;
using API.Errors;
using Microsoft.AspNetCore.Http;
using API.Helpers;

//note: controllers are created by each session
namespace API.Controllers
{
    public class ProductsController : BaseApiController
    {
        //private readonly IProductRepository _repo;      replaced by genericrepository
        private readonly IGenericRepository<Product> _productsRepo;
        private readonly IGenericRepository<ProductBrand> _brandsRepo;
        private readonly IGenericRepository<ProductType> _typesRepo;
        private readonly IMapper _mapper;

        public ProductsController(IGenericRepository<Product> productsRepo,
        IGenericRepository<ProductBrand> brandsRepo, IGenericRepository<ProductType> typesRepo,
        IMapper mapper)
        {
            _mapper = mapper;
            _typesRepo = typesRepo;
            _brandsRepo = brandsRepo;
            _productsRepo = productsRepo;
            //_repo = repo;    replaced with generic repos
            //repo is injected into controller with scoped lifetime
        }

        /*
        Before Dto
        [HttpGet]
        public async Task<ActionResult<List<Product>>> GetProducts()
        {
            //Generic ListAllAsync inherits Product type from _productsRepo
            //var products = await _productsRepo.ListAllAsync();
            var spec = new ProductsWithTypesAndBrandsSpecification();
            var products = await _productsRepo.ListAsync(spec);
            return Ok(products);
        }*/

        /* Before automapper
        [HttpGet]
        public async Task<ActionResult<List<ProductToReturnDto>>> GetProducts()
        {
            //Generic ListAllAsync inherits Product type from _productsRepo
            //var products = await _productsRepo.ListAllAsync();
            var spec = new ProductsWithTypesAndBrandsSpecification();
            var products = await _productsRepo.ListAsync(spec);
            return products.Select(product => new ProductToReturnDto
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                PictureUrl = product.PictureUrl,
                Price = product.Price,
                ProductBrand = product.ProductBrand.Name,
                ProductType = product.ProductType.Name
            }).ToList();
        } */

        // Replaced with new class for parameters
        // [HttpGet]
        // public async Task<ActionResult<IReadOnlyList<ProductToReturnDto>>> GetProducts(string sort,
        // int? brandId, int? typeId)
        // {
        //     var spec = new ProductsWithTypesAndBrandsSpecification(sort, brandId, typeId);
        //     var products = await _productsRepo.ListAsync(spec);
        //     return Ok(_mapper.Map<IReadOnlyList<Product>, IReadOnlyList<ProductToReturnDto>>(products));
        // }

        //Since we replaced the native data types with an object, controller cant tell from query string how
        //it fits in object. Use [FromQuery]
        //Note changed Task...<IReadOnlyList<ProductToReturnDto>>> to Task...<Pagination<ProductToReturnDto>>> 
        [HttpGet]
        public async Task<ActionResult<Pagination<ProductToReturnDto>>> GetProducts(
            [FromQuery]ProductSpecParams productParams)
        {
            var spec = new ProductsWithTypesAndBrandsSpecification(productParams);
            var countSpec = new ProductWithFiltersForCountSpecification(productParams);
            var totalItems = await _productsRepo.CountAsync(countSpec);
            var products = await _productsRepo.ListAsync(spec);
            var data = _mapper.Map<IReadOnlyList<Product>, IReadOnlyList<ProductToReturnDto>>(products);
            return Ok(new Pagination<ProductToReturnDto>(productParams.PageIndex, productParams.PageSize, totalItems, data));
        }


        /* Before Dto
        https://localhost:5001/api/products/2 would return this
         whatever number in url is passed into int id
        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            //return await _productsRepo.GetByIdAsync(id);
            //return Ok(product);
            var spec = new ProductsWithTypesAndBrandsSpecification(id);
            return await _productsRepo.GetEntityWithSpec(spec);
        }   */

        /*Without automapper
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductToReturnDto>> GetProduct(int id)
        {
            var spec = new ProductsWithTypesAndBrandsSpecification(id);
            var product = await _productsRepo.GetEntityWithSpec(spec);
            return new ProductToReturnDto
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                PictureUrl = product.PictureUrl,
                Price = product.Price,
                ProductBrand = product.ProductBrand.Name,
                ProductType = product.ProductType.Name

            };
        } */

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)] //for swagger, method could return ok
        //for swagger, method could return not found. typeof(ApiReponse) formats error value that is returned
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)] 
        public async Task<ActionResult<ProductToReturnDto>> GetProduct(int id)
        {
            var spec = new ProductsWithTypesAndBrandsSpecification(id);
            var product = await _productsRepo.GetEntityWithSpec(spec);

            if(product == null) return NotFound(new ApiResponse(404));
            return _mapper.Map<Product, ProductToReturnDto>(product);
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