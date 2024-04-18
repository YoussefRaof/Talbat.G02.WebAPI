using Microsoft.AspNetCore.Builder;

namespace Talabat.APIs.Extensions
{
	public static class SwaggerServicesExtenstion
	{
		public static IServiceCollection AddSwaggerServices(this IServiceCollection services)
		{
			services.AddEndpointsApiExplorer();
			services.AddSwaggerGen();

			return services;
		}


		public static WebApplication UseSwaggerMiddlewares(this WebApplication app)
		{
			app.UseSwagger();
			app.UseSwaggerUI();

			return app;
		}
	}
}
