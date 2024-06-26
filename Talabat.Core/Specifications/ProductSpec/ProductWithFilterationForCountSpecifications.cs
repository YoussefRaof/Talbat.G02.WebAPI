﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;

namespace Talabat.Core.Specifications.ProductSpec
{
	public class ProductWithFilterationForCountSpecifications : BaseSpecifications<Product>
	{
		public ProductWithFilterationForCountSpecifications(ProductSpecParams specParams) :
			base(P =>
					 (!specParams.BrandId.HasValue || P.BrandId == specParams.BrandId.Value) &&
				(!specParams.CategoryId.HasValue || P.CategoryId == specParams.CategoryId.Value)

			)
		{

		}
	}
}
