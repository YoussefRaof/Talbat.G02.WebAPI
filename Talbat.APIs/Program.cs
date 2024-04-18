using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Text.Json;
using Talabat.APIs.Errors;
using Talabat.APIs.Helpers;
using Talabat.APIs.Middlewares;
using Talabat.Core.Entities;
using Talabat.Core.Repositories.Contract;
using Talabat.Reop;
using Talabat.Reop.Data;

namespace Talabat.APIs
{
	public class Program
	{
		public static async Task Main(string[] args)
		{
			var webApplicationBuilder = WebApplication.CreateBuilder(args);

			#region Configure Services

			// Add services to the DI container.

			webApplicationBuilder.Services.AddControllers(); // Register Web API Services In DI Container
															 // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
			webApplicationBuilder.Services.AddEndpointsApiExplorer();
			webApplicationBuilder.Services.AddSwaggerGen();

			//webApplicationBuilder.Services.AddScoped<IGenericRepository<Product>, GenericRepository<Product>>();
			//webApplicationBuilder.Services.AddScoped<IGenericRepository<ProductBrand>, GenericRepository<ProductBrand>>();
			//webApplicationBuilder.Services.AddScoped<IGenericRepository<ProductCategory>, GenericRepository<ProductCategory>>();
			webApplicationBuilder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

			webApplicationBuilder.Services.AddDbContext<StoreContext>(options =>
			{
				options.UseSqlServer(webApplicationBuilder.Configuration.GetConnectionString("DefaultConnection"));
			});

			var Url = webApplicationBuilder.Configuration["ApiBaseUrl"];

			webApplicationBuilder.Services.AddAutoMapper(M => M.AddProfile(new MappingProfiles(Url)));
			//webApplicationBuilder.Services.AddAutoMapper(typeof(MappingProfiles));

			webApplicationBuilder.Services.Configure<ApiBehaviorOptions>(options =>
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


			#endregion

			var app = webApplicationBuilder.Build();

			using var scope = app.Services.CreateScope();

			var services = scope.ServiceProvider;

			var _dbContext = services.GetRequiredService<StoreContext>();
			// Ask CLR For Creating Object From DbContext Explicitly

			var loggerFactory = services.GetRequiredService<ILoggerFactory>();

			var logger = loggerFactory.CreateLogger<Program>();
			try
			{
				await _dbContext.Database.MigrateAsync(); // Update-Database 
				await StoreContextSeed.SeedAsync(_dbContext);
			}
			catch (Exception ex)
			{
				logger.LogError(ex, "An Error Occured During Applying Migration");

				Console.WriteLine(ex);
			}



			#region Configure Kestral Middlewares

			app.UseMiddleware<ExceptionMiddleware>();


			

			// Configure the HTTP request pipeline.
			if (app.Environment.IsDevelopment())
			{
				app.UseSwagger();
				app.UseSwaggerUI();
			}
			app.UseStatusCodePagesWithReExecute("/errors/{0}");

			app.UseHttpsRedirection();
			app.UseStaticFiles();


			//app.UseRouting();
			//app.UseEndpoints(endpoints =>
			//{
			//	endpoints.MapControllerRoute(

			//		name: "default",
			//		pattern: "{controller}/{action}/{id?}"
			//		);
			//	endpoints.MapControllers();	

			//});

			app.MapControllers();
			#endregion

			app.Run();
		}
	}
}
