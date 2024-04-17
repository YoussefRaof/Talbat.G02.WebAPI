using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Talabat.Core.Entities
{
	public class Product : BaseEntity
	{
        public string  Name { get; set; }
        public string Description { get; set; }
        public string PictureUrl { get; set; }
        public decimal Price { get; set; }

        //[ForeignKey(nameof(Product.Brand))]
        public int BrandId { get; set; } // Forgein Key Column => ProductBrand

        public virtual ProductBrand Brand { get; set; } // Navigational Property [One]

		//[ForeignKey(nameof(Product.Category))]
		public int CategoryId { get; set; } // Forgein Key Column => ProductCategory
		public virtual ProductCategory Category { get; set; } // Navigational Property [One]

	}
}
