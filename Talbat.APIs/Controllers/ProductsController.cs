using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections;
using Talabat.Core.Entities;
using Talabat.Core.Repositories.Contract;
using Talabat.Core.Specifications;
using Talabat.Core.Specifications.ProductSpec;

namespace Talabat.APIs.Controllers
{

	public class ProductsController : BaseApiController
	{
		private readonly IGenericRepository<Product> _productsReop;

		public ProductsController(IGenericRepository<Product> productsReop)
        {
			_productsReop = productsReop;
		}

		[HttpGet]
		public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
		{
			var spec = new ProductWithBrandAndCategorySpecifications();
			var products= await _productsReop.GetAllWithSpecsAsync(spec);
			return Ok(products);
		}

		// baseurl/apiProducts/1

		[HttpGet("{id}")]
		public async Task<ActionResult<Product>> GetProduct(int id)
		{
			var spec = new ProductWithBrandAndCategorySpecifications( id);
			var product = await _productsReop.GetWithSpec(spec);
			if (product is null)
				return NotFound(new { Message = "Not Found" , StatusCode = 404}); //404

			return Ok(product); //200
		}

			
    }
}
