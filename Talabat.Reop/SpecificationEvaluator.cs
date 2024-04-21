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

			if(spec.Criteria is not null) // //_dbcontext.Set<Product>().Where( // P=> True & True);
				query = query.Where(spec.Criteria); 

			if(spec.OrderBy is not null) 
				query = query.OrderBy(spec.OrderBy);//_dbcontext.Set<Product>().Where( // P=> True & True).OrderBy(P=>P.Name);

			else if(spec.OrderByDesc is not null) // P => P.Price
				query = query.OrderByDescending(spec.OrderByDesc);

			if(spec.IsPaginationEnabled)
				query = query.Skip(spec.Skip).Take(spec.Take);//_dbcontext.Set<Product>().Where(P=> True & True).OrderBy(P=>P.Name).Skip(5).Take(5);


			//_dbcontext.Set<Product>().Where( P=>P.BrandId =2 & True).OrderBy(P =>P.Name)
			//Includes
			//A. P => P.Brand
			//B. P => P.Category
			//
			query = spec.Includes.Aggregate(query,(CurrentQuery , IncludeExpression) =>CurrentQuery.Include(IncludeExpression));
			//_dbcontext.Set<Product>().Where(P=> True & True).OrderBy(P=>P.Name).Skip(5).Take(5).Include(P=>P.Brand).(P=>P.Category);


			return query;
		}
	}
}
