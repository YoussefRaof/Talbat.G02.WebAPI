using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections;
using Talabat.APIs.DTOs;
using Talabat.APIs.Errors;
using Talabat.Core.Entities;
using Talabat.Core.Repositories.Contract;
using Talabat.Core.Specifications;
using Talabat.Core.Specifications.ProductSpec;

namespace Talabat.APIs.Controllers
{

	public class ProductsController : BaseApiController
	{
		private readonly IGenericRepository<Product> _productsReop;
		private readonly IMapper _mapper;
		private readonly IGenericRepository<ProductBrand> _brandRepo;
		private readonly IGenericRepository<ProductCategory> _categoryRepo;

		public ProductsController
			(IGenericRepository<Product> productsReop, IMapper mapper,
			IGenericRepository<ProductBrand> brandRepo, IGenericRepository<ProductCategory> categoryRepo)
		{
			_productsReop = productsReop;
			_mapper = mapper;
			_brandRepo = brandRepo;
			_categoryRepo = categoryRepo;
		}

		[HttpGet]
		public async Task<ActionResult<IReadOnlyList<ProductToReturnDto>>> GetProducts(string sort)
		{
			var spec = new ProductWithBrandAndCategorySpecifications(sort);
			var products = await _productsReop.GetAllWithSpecsAsync(spec);
				return Ok(_mapper.Map<IReadOnlyList<Product>, IEnumerable<ProductToReturnDto>>(products));
		}

		// baseurl/api/products/1

		[ProducesResponseType(typeof(ProductToReturnDto), StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
		[HttpGet("{id}")]
		public async Task<ActionResult<ProductToReturnDto>> GetProduct(int id)
		{
			var spec = new ProductWithBrandAndCategorySpecifications(id);
			var product = await _productsReop.GetWithSpec(spec);
			if (product is null)
				return NotFound(new ApiResponse(404)); //404

			return Ok(_mapper.Map<Product, ProductToReturnDto>(product)); //200
		}

		[HttpGet("brands")]  //GET : /api/products/brands

		public async Task<ActionResult<IReadOnlyList<ProductBrand>>> GetBrands()
		{
			var brands = await _brandRepo.GetAllAsync();

			return Ok(brands);
		}
			
		[HttpGet("categories")] //GET: /api/products/categories

		public async Task<ActionResult<IReadOnlyList<ProductCategory>>> GetCategories()
		{
			var categories = await _categoryRepo.GetAllAsync();
			return Ok(categories);
		}


	}
}
