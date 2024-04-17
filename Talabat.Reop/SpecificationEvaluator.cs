using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;
using Talabat.Core.Specifications;

namespace Talabat.Reop
{
	internal static class SpecificationEvaluator<TEntity> where TEntity : BaseEntity
	{
		public static IQueryable<TEntity> GetQuery(IQueryable<TEntity> inputQuery , ISpecifications<TEntity> spec) 
		{
			var query = inputQuery; //_dbcontext.Set<Product>();

			if(spec.Criteria is not null) // P => P.Id ==1
				query = query.Where(spec.Criteria); // query = _dbcontext.Set<Product>().Where( P => P.Id ==1);

			//Includes
			//A. P => P.Brand
			//B. P => P.Category
			//
			spec.Includes.Aggregate(query,(CurrentQuery , IncludeExpression) =>CurrentQuery.Include(IncludeExpression));
			// query = _dbcontext.Set<Product>().Where( P => P.Id ==1).Include( P => P.Brand).Include( P => P.Category);


			return query;
		}
	}
}
