using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;
using Talabat.Core.Repositories.Contract;
using Talabat.Core.Specifications;
using Talabat.Reop.Data;

namespace Talabat.Reop
{
	public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
	{
		private readonly StoreContext _dbContext;

		public GenericRepository(StoreContext dbContext) // Ask CLR To Creating Object From StoreContext Implicitly
		{
			_dbContext = dbContext;
		}

		public async Task<T?> GetAsync(int id)
		{

			//if (typeof(T) == typeof(Product))
			//	return await _dbContext.Set<Product>().Where(P => P.Id == id).Include(P => P.Brand).Include(P => P.Category).FirstOrDefaultAsync() as T;

			return await _dbContext.Set<T>().FindAsync(id);
		}
		public async Task<IReadOnlyList<T>> GetAllAsync()
		{
			//if (typeof(T) == typeof(Product))
			//	return (IEnumerable<T>)await _dbContext.Set<Product>().OrderBy(P =>P.Name).Include(P => P.Brand).Include(P => P.Category).ToListAsync();
			return await _dbContext.Set<T>().AsNoTracking().ToListAsync();
		}

		public async Task<IReadOnlyList<T>> GetAllWithSpecsAsync(ISpecifications<T> spec)
		{
			return await ApplySpecifications(spec).ToListAsync();
		}

		public async Task<T?> GetWithSpec(ISpecifications<T> spec)
		{
			return await ApplySpecifications(spec).FirstOrDefaultAsync();
		}

		private IQueryable<T> ApplySpecifications(ISpecifications<T> spec) 
		{
			return SpecificationEvaluator<T>.GetQuery(_dbContext.Set<T>(), spec);
		}
	}
}
