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

		public ProductsController(IGenericRepository<Product> productsReop,IMapper mapper)
        {
			_productsReop = productsReop;
			_mapper = mapper;
		}

		[HttpGet]
		public async Task<ActionResult<IEnumerable<ProductToReturnDto>>> GetProducts()
		{
			var spec = new ProductWithBrandAndCategorySpecifications();
			var products= await _productsReop.GetAllWithSpecsAsync(spec);
			return Ok(_mapper.Map<IEnumerable<Product>,IEnumerable<ProductToReturnDto>>(products));
		}

		// baseurl/api/products/1

		[ProducesResponseType(typeof(ProductToReturnDto),StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(ApiResponse),StatusCodes.Status404NotFound)]
		[HttpGet("{id}")]
		public async Task<ActionResult<ProductToReturnDto>> GetProduct(int id)
		{
			var spec = new ProductWithBrandAndCategorySpecifications( id);
			var product = await _productsReop.GetWithSpec(spec);
			if (product is null)
				return NotFound(new ApiResponse(404)); //404

			return Ok(_mapper.Map<Product,ProductToReturnDto>(product)); //200
		}

			
    }
}
