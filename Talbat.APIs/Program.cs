using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using StackExchange.Redis;
using System.Net;
using System.Text;
using System.Text.Json;
using Talabat.APIs.Errors;
using Talabat.APIs.Extensions;
using Talabat.APIs.Helpers;
using Talabat.APIs.Middlewares;
using Talabat.Core.Entities;
using Talabat.Core.Entities.Identity;
using Talabat.Core.Repositories.Contract;
using Talabat.Reop;
using Talabat.Reop._Identity;
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


			webApplicationBuilder.Services.AddSwaggerServices();

			//webApplicationBuilder.Services.AddScoped<IGenericRepository<Product>, GenericRepository<Product>>();
			//webApplicationBuilder.Services.AddScoped<IGenericRepository<ProductBrand>, GenericRepository<ProductBrand>>();
			//webApplicationBuilder.Services.AddScoped<IGenericRepository<ProductCategory>, GenericRepository<ProductCategory>>();


			var Url = webApplicationBuilder.Configuration["ApiBaseUrl"];
			webApplicationBuilder.Services.AddAutoMapper(M => M.AddProfile(new MappingProfiles(Url)));
			//webApplicationBuilder.Services.AddAutoMapper(typeof(MappingProfiles));

			webApplicationBuilder.Services.AddDbContext<StoreContext>(options =>
			{
				options.UseSqlServer(webApplicationBuilder.Configuration.GetConnectionString("DefaultConnection"));
			});

			webApplicationBuilder.Services.AddDbContext<ApplicationIdentityDbContext>(options =>
			{
				options.UseSqlServer(webApplicationBuilder.Configuration.GetConnectionString("IdentityConnection"));
			});




			webApplicationBuilder.Services.AddSingleton<IConnectionMultiplexer>((serviceprovider) =>
			{
				var connection = webApplicationBuilder.Configuration.GetConnectionString("Redis");
				return ConnectionMultiplexer.Connect(connection);
			});

			webApplicationBuilder.Services.AddApplicationServices();


			webApplicationBuilder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
			{
				//options.Password.RequiredUniqueChars = 2;
				//options.Password.RequireDigit = true;
				//options.Password.RequireLowercase = true;
				//options.Password.RequireUppercase = true;
			})
				.AddEntityFrameworkStores<ApplicationIdentityDbContext>();

			webApplicationBuilder.Services.AddAuthService(webApplicationBuilder.Configuration);





			#endregion

			var app = webApplicationBuilder.Build();

			#region Update Database & DataSeeding
			using var scope = app.Services.CreateScope();

			var services = scope.ServiceProvider;

			var _dbContext = services.GetRequiredService<StoreContext>();



			var _identity = services.GetRequiredService<ApplicationIdentityDbContext>();// Ask CLR For Creating Object From ApplicationIdentityDbContext Explicitly
																						// Ask CLR For Creating Object From DbContext Explicitly

			var loggerFactory = services.GetRequiredService<ILoggerFactory>();

			var logger = loggerFactory.CreateLogger<Program>();
			try
			{
				await _dbContext.Database.MigrateAsync(); // Update-Database 
				await StoreContextSeed.SeedAsync(_dbContext);

				var _usermanager = services.GetRequiredService<UserManager<ApplicationUser>>();
				await ApplicationIdentityContextDataSeed.SeedUsersAsync(_usermanager);
				await _identity.Database.MigrateAsync();
			}
			catch (Exception ex)
			{
				logger.LogError(ex, "An Error Occured During Applying Migration");

				Console.WriteLine(ex);
			}
			#endregion



			#region Configure Kestral Middlewares

			app.UseMiddleware<ExceptionMiddleware>();

			app.UseAuthorization();
			app.UseAuthentication();


			// Configure the HTTP request pipeline.
			if (app.Environment.IsDevelopment())
			{
				app.UseSwaggerMiddlewares();
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
