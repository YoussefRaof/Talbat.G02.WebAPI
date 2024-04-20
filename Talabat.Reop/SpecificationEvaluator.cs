﻿using Microsoft.EntityFrameworkCore;
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

			if(spec.Criteria is not null) // 
				query = query.Where(spec.Criteria); 

			if(spec.OrderBy is not null) 
				query = query.OrderBy(spec.OrderBy);

			else if(spec.OrderByDesc is not null) // P => P.Price
				query = query.OrderByDescending(spec.OrderByDesc);


			//query _dbcontext.Set<Product>().OrderBy(P =>P.Name);
			//Includes
			//A. P => P.Brand
			//B. P => P.Category
			//
			query = spec.Includes.Aggregate(query,(CurrentQuery , IncludeExpression) =>CurrentQuery.Include(IncludeExpression));
			// query = _dbcontext.Set<Product>().OrderByDescending(P =>P.Name).Include( P => P.Brand).Include( P => P.Category);


			return query;
		}
	}
}
