using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Talabat.Core.Entities;
using Talabat.Core.Repository.Contract;
using Talabat.Core.Specifications.Product_Specs;
using Talabat.Repository.Data;
using TalabatAPIs.Dtos;
using TalabatAPIs.Errors;
using TalabatAPIs.Helper;

namespace TalabatAPIs.Controllers
{

    public class ProductsController : BaseApiController
    {
        private readonly IGenericRepository<Product> _productsRepo;
        private readonly IMapper _mapper;
        private readonly IGenericRepository<ProductBrand> _brandsRepo;
        private readonly IGenericRepository<ProductCategory> _categoryRepo;

        public ProductsController(IGenericRepository<Product> productsRepo,IMapper mapper,IGenericRepository<ProductBrand> BrandsRepo,IGenericRepository<ProductCategory> CategoryRepo)
        {
            _productsRepo = productsRepo;
            _mapper = mapper;
            _brandsRepo = BrandsRepo;
            _categoryRepo = CategoryRepo;
        }
        // /api/Products
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<Pagination<ProductToReturnDto>>> GetProducts([FromQuery]ProductSpecParams specParams)
        {
            var spec = new ProductWithBrandAndCategorySpecifications(specParams);
            
            var Products = await _productsRepo.GetAllWithSpecAsync(spec);
            
            var data = _mapper.Map<IReadOnlyList<Product>, IReadOnlyList<ProductToReturnDto>>(Products);
           
            var countspec = new ProductsWithFilterationForCountSpecifications(specParams);
            
            var count = await _productsRepo.GetCountAsync(countspec);
            
            return Ok(new Pagination<ProductToReturnDto>(specParams.PageIndex,specParams.PageSize,count,data));
        }

        // /api/Products/1
        [ProducesResponseType(typeof(ProductToReturnDto),StatusCodes.Status200OK)]
        [ProducesErrorResponseType(typeof(ApiResponse))]
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductToReturnDto>> GetProduct(int id)
        {
            var spec = new ProductWithBrandAndCategorySpecifications(id);

            var products = await _productsRepo.GetWithSpecAsync(spec);
            if(products is null)
                return NotFound(new ApiResponse(404));
            return Ok(_mapper.Map<Product, ProductToReturnDto>(products));
        } 
        [HttpGet("brands")]
        public async Task<ActionResult<IReadOnlyList<ProductBrand>>> GetBrands()
        {
            var brands = await _brandsRepo.GetAllAsync();
            return Ok(brands);
        } 
        [HttpGet("categories")]
        public async Task<ActionResult<IReadOnlyList<ProductCategory>>> GetCategories()
        {
            var categories = await _categoryRepo.GetAllAsync();
            return Ok(categories);
        }

    }
}
