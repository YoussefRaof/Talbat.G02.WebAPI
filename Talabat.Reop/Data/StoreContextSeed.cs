using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Talabat.Core.Entities;

namespace Talabat.Reop.Data
{
    public class StoreContextSeed
    {
        public async static Task SeedAsync(StoreContext _dbContext)
        {
            if ( !_dbContext.ProductBrands.Any() )
            {
                var brandsData = File.ReadAllText("../Talabat.Reop/Data/DataSeed/brands.json");
                var brands = JsonSerializer.Deserialize<List<ProductBrand>>(brandsData);

                if (brands?.Count > 0)
                {
                    foreach (var brand in brands)
                    {
                        _dbContext.Set<ProductBrand>().Add(brand);
                    }
                    await _dbContext.SaveChangesAsync();
                } 
            }


			if (_dbContext.ProductCategories.Count() == 0 )
			{
				var categoriesData = File.ReadAllText("../Talabat.Reop/Data/DataSeed/categories.json");
				var categories = JsonSerializer.Deserialize<List<ProductCategory>>(categoriesData);

				if (categories?.Count > 0)
				{
					foreach (var category in categories)
					{
						_dbContext.Set<ProductCategory>().Add(category);
					}
					await _dbContext.SaveChangesAsync();
				}
			}

		}
    }
}
