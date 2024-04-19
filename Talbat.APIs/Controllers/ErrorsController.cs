using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Talabat.APIs.Errors;

namespace Talabat.APIs.Controllers
{
	[Route("errors/{Code}")]
	[ApiController]
	[ApiExplorerSettings(IgnoreApi = true)]
	public class ErrorsController : ControllerBase
	{
		public ActionResult Error(int Code)
		{
			if (Code == 400)
				return BadRequest(new ApiResponse(400));
			else if(Code == 401)
				return Unauthorized(new ApiResponse(401));
			else if(Code == 404)
				return NotFound(new ApiResponse(404));

			else
			return StatusCode(Code);

		}
	}
}
