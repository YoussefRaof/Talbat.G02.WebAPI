using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Talabat.APIs.Helpers;
using Talabat.Core.Repositories.Contract;
using Talabat.Reop.Data;
using Talabat.Reop;
using System;
using Talabat.APIs.Errors;
using Talabat.Reop._Identity;
using Microsoft.EntityFrameworkCore;
using Talabat.Core.Services.Contract;
using Talabat.Service.AuthService;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Talabat.Core.UnitOfWork.Contract;
using Talabat.Service.OrderService;
using Talabat.Service.ProductService;

namespace Talabat.APIs.Extensions
{
	public static class ApplicationServicesExtension
	{
		public static IServiceCollection AddApplicationServices(this IServiceCollection services)
		{
			services.AddScoped(typeof(IOrderService),typeof(OrderService));	

			services.AddScoped(typeof(IProductService) , typeof(ProductService));

			services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
			services.AddScoped(typeof(IUnitOfWork), typeof(UnitOfWork));
			services.AddScoped(typeof(IBasketRepository), typeof(BasketRepository));
			

			services.Configure<ApiBehaviorOptions>(options =>
			{
				options.InvalidModelStateResponseFactory = (actioncontext) =>
				{
					var errors = actioncontext.ModelState.Where(P => P.Value.Errors.Count > 0)
														 .SelectMany(P => P.Value.Errors)
														 .Select(E => E.ErrorMessage)
														 .ToList();

					var response = new ApiValidationErrorResponse()
					{
						Errors = errors
					};

					return new BadRequestObjectResult(response);
				};
			});

			



			return services;

			
			 

		}
		public static IServiceCollection AddAuthService(this IServiceCollection services, IConfiguration configuration)
		{

			services.AddAuthentication(options =>
			{
				options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
				options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
			})
			.AddJwtBearer( options =>
			{
				options.TokenValidationParameters = new TokenValidationParameters()
				{
					ValidateIssuer = true,
					ValidIssuer = configuration["JWT:ValidIssuer"],
					ValidateAudience = true,
					ValidAudience = configuration["JWT:ValidAudience"],
					ValidateIssuerSigningKey = true,
					IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:AuthKey"] ?? string.Empty)),
					ValidateLifetime = true,
					ClockSkew = TimeSpan.Zero
				};
			});
			services.AddScoped(typeof(IAuthService), typeof(AuthService));

			return services;

		}
	}
}
