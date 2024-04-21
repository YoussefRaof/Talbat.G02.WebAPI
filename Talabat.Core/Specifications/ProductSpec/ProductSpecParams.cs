using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Talabat.Core.Specifications.ProductSpec
{
	public class ProductSpecParams
	{
		private const int MaxPageSize = 10;
		private int pagesize=5;

		public int PageSize
		{
			get { return pagesize; }
			set { pagesize=value > MaxPageSize ? MaxPageSize : value  ; }
		}

		public int PageIndex { get; set; } = 1;



        public string? Sort { get; set; }
        public int? BrandId { get; set; }
        public int? CategoryId { get; set; }
    }
}
