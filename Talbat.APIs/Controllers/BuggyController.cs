using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Talabat.APIs.Errors;
using Talabat.Reop.Data;

namespace Talabat.APIs.Controllers
{
	public class BuggyController : BaseApiController
	{
		private readonly StoreContext _dbContext;

		public BuggyController(StoreContext dbContext)
        {
			_dbContext = dbContext;
		}

		[HttpGet("notfound")] // BaseUrl/api/buggy/notfound
		public ActionResult GetNotFoundRequest()
		{
			var product = _dbContext.Products.Find(100);
			if(product is null) 
				return NotFound(new ApiResponse(404));

			return Ok(product);
		}

		[HttpGet("servererror")] // /api/buggy/servererror

		public ActionResult GetServerError() 
		{
			var product = _dbContext.Products.Find(100);

			var ProductToReturnDto = product.ToString();
			return Ok(ProductToReturnDto);

		}

		[HttpGet("badrequest")] // /api/buggy/badrequest

		public ActionResult GetBadRequest()
		{
			return BadRequest(new ApiResponse(400));
		}

		[HttpGet("badrequest/{id}")] // /api/buggy/badrequest/five

		public ActionResult GetBadRequest(int id) // Validation Error	
		{
			return Ok() ;
		}

		[HttpGet("unauthorized")]
		public ActionResult GetUnauthorizedError()
		{
			return Unauthorized(new ApiResponse(401));
		}


	}
}
