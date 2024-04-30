using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Talabat.APIs.DTOs;
using Talabat.APIs.Errors;
using Talabat.APIs.Extensions;
using Talabat.Core.Entities.Identity;
using Talabat.Core.Services.Contract;

namespace Talabat.APIs.Controllers
{
	
	public class AccountController : BaseApiController
	{
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly SignInManager<ApplicationUser> _signInManager;
		private readonly IAuthService _authService;

		public AccountController(UserManager<ApplicationUser> userManager , SignInManager<ApplicationUser> signInManager ,IAuthService authService )
        {
			_userManager = userManager;
			_signInManager = signInManager;
			_authService = authService;
		}

		[HttpPost("login")] // POST /api/Account/login

		public async Task<ActionResult<UserDto>> Login(LoginDto model)
		{
			var user = await _userManager.FindByEmailAsync(model.Email);
			if(user is  null)
			{
				return Unauthorized(new ApiResponse(401, "Invalid Login"));
			}
			var result = await _signInManager.CheckPasswordSignInAsync(user, model.Password,false);

			if (!result.Succeeded)
				return Unauthorized(new ApiResponse(401, "Invalid Login"));


			return Ok(new UserDto()
			{
				DisplayName = user.DisplayName,
				Email = user.Email,
				Token =  await _authService.CreateTokenAsync(user , _userManager),
			});

		}

		[HttpPost("register")] // POST /api/Account/register

		public async Task<ActionResult<UserDto>> Register(RegisterDto model)
		{
			var user = new ApplicationUser()
			{
				DisplayName = model.DisplayName,
				Email = model.Email,
				PhoneNumber = model.Phone,
				UserName = model.Email.Split("@")[0],

			};

			var result = await _userManager.CreateAsync(user, model.Password);
			if (!result.Succeeded)
				return BadRequest(new ApiValidationErrorResponse() { Errors =result.Errors.Select(E => E.Description)});

			return Ok(new UserDto()
			{
				DisplayName = user.DisplayName,
				Email = user.Email,
				Token = await _authService.CreateTokenAsync(user, _userManager),
			});

		}

		[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
		[HttpGet]// GET: /api/Account

		public async Task<ActionResult<UserDto>> GetCurrentUser()
		{
			var email = User.FindFirstValue(ClaimTypes.Email);

			var user = await _userManager.FindByEmailAsync(email);

			return Ok(new UserDto()
			{
				DisplayName = user.DisplayName,
				Email = user.Email,
				Token = await _authService.CreateTokenAsync(user, _userManager)
			});
		}

		[Authorize(AuthenticationSchemes =JwtBearerDefaults.AuthenticationScheme)]
		[HttpGet("address")] // GET : /api/account/address

		public async Task<ActionResult<Address>> GetUserAddress()
		{

			var user = await _userManager.FindUserWithAddress(User);
			return Ok(user.Addrees);
		}


    }
}
