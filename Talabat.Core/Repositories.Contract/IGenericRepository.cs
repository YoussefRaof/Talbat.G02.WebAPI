using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;
using Talabat.Core.Specifications;

namespace Talabat.Core.Repositories.Contract
{
	public interface IGenericRepository<T> where T : BaseEntity  
	{
		Task<T?> GetAsync(int id);

		Task<IReadOnlyList<T>> GetAllAsync();

		Task<IReadOnlyList<T>> GetAllWithSpecsAsync(ISpecifications<T> spec);
		Task<T?> GetWithSpec (ISpecifications<T> spec);

		Task<int> GetCountAsync(ISpecifications<T> spec);
		void Add(T Entity);
		void Update(T Entity);
		void Delete(T Entity);

	}
}
