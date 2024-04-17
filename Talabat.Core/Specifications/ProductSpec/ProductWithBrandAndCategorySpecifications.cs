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
        public ProductWithBrandAndCategorySpecifications() : base()
        {
            Includes.Add(P => P.Brand);
            Includes.Add(P => P.Category);
            
        }

        // This Constructor Will Be Used To Create Object That Will Be Used To Get Specific Product
        public ProductWithBrandAndCategorySpecifications(int id) :base(P => P.Id == id)
        {
			Includes.Add(P => P.Brand);
			Includes.Add(P => P.Category);

		}
    }
}
