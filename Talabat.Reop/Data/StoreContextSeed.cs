﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Talabat.Core.Entities;
using Talabat.Core.Entities.Order_Aggregate;

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

			if (_dbContext.Products.Count() == 0)
			{
				var productsData = File.ReadAllText("../Talabat.Reop/Data/DataSeed/products.json");
				var products = JsonSerializer.Deserialize<List<Product>>(productsData);

				if (products?.Count > 0)
				{
					foreach (var product in products)
					{
						_dbContext.Set<Product>().Add(product);
					}
					await _dbContext.SaveChangesAsync();
				}
			}

			if (!_dbContext.DeliveryMethods.Any() )
			{
				var deliveryMethodsData = File.ReadAllText("../Talabat.Reop/Data/DataSeed/delivery.json");
				var deliveryMethods = JsonSerializer.Deserialize<List<DeliveryMethod>>(deliveryMethodsData);

				if (deliveryMethods?.Count > 0)
				{
					foreach (var deliveryMethod in deliveryMethods)
					{
						_dbContext.Set<DeliveryMethod>().Add(deliveryMethod);
					}
					await _dbContext.SaveChangesAsync();
				}
			}


		}
    }
}
