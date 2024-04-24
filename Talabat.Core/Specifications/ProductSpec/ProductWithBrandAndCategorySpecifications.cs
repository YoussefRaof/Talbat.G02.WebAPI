using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;

namespace Talabat.Core.Specifications.ProductSpec
{
	public class ProductWithBrandAndCategorySpecifications : BaseSpecifications<Product>
	{
        //This Contructor Will Be Used To Create Object That Will Be Used To Get All Products 
        public ProductWithBrandAndCategorySpecifications(ProductSpecParams specParams) :
            base (P =>
                (!specParams.BrandId.HasValue || P.BrandId == specParams.BrandId.Value) &&
                (!specParams.CategoryId.HasValue || P.CategoryId == specParams.CategoryId.Value )
               )
        {
            Includes.Add(P => P.Brand);
            Includes.Add(P => P.Category);

            if(!string.IsNullOrEmpty(specParams.Sort))
            {
                switch (specParams.Sort)
                {
                    case "priceAsc":
                        AddOrderBy(P => P.Price);
                        break;
                    case "priceDesc":
                        AddOrderByDesc(P => P.Price);
                        break;

                    default:
                        AddOrderBy(P =>P.Name);
                    break;

                }
            }
            else
                AddOrderBy(P => P.Name);

            //Products = 18 ~ 20   4pages   5 5 5 3
            //PageSize = 5
            //PageIndex = 2

            ApplyPagination(((specParams.PageIndex - 1) * specParams.PageSize), specParams.PageSize);
            
        }

        // This Constructor Will Be Used To Create Object That Will Be Used To Get Specific Product
        public ProductWithBrandAndCategorySpecifications(int id) :base(P => P.Id == id)
        {
			Includes.Add(P => P.Brand);
			Includes.Add(P => P.Category);

		}
    }
}
