using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;
using Talabat.Core.Entities.Order_Aggregate;
using Talabat.Core.Repositories.Contract;
using Talabat.Core.UnitOfWork.Contract;
using Talabat.Reop.Data;

namespace Talabat.Reop
{
	public class UnitOfWork : IUnitOfWork
	{
		private readonly StoreContext _dbContext;

		private Hashtable _repositories;

		public UnitOfWork(StoreContext  dbContext) // Ask Clr For Creating Object StoreContext impilcitly 
        {
			_dbContext = dbContext;
			_repositories = new Hashtable();
		}
		public IGenericRepository<TEntity> Repository<TEntity>() where TEntity : BaseEntity
		{
			var key = typeof(TEntity).Name;

			if (! _repositories.ContainsKey(key))
			{
				var repo = new GenericRepository<TEntity>(_dbContext)  ;
				_repositories.Add(key, repo) ; 
			}
			return _repositories[key] as IGenericRepository<TEntity>;

		}
		public async Task<int> CompleteAsync()
			=> await _dbContext.SaveChangesAsync();

		public async ValueTask DisposeAsync()
			=> await _dbContext.DisposeAsync();

	}
}
