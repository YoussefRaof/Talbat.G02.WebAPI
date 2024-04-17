﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections;
using Talabat.Core.Entities;
using Talabat.Core.Repositories.Contract;

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
			var products= await _productsReop.GetAllAsync();
			return Ok(products);
		}

		// baseurl/apiProducts/1

		[HttpGet("{id}")]
		public async Task<ActionResult<Product>> GetProduct(int id)
		{
			var product = await _productsReop.GetAsync(id);
			if (product is null)
				return NotFound(new { Message = "Not Found" , StatusCode = 404}); //404

			return Ok(product); //200
		}

			
    }
}
