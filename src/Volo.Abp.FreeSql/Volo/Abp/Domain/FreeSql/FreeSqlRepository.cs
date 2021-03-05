using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Uow;
using FreeSql.Extensions;
using FreeSql;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories;
using System.Threading.Tasks;
using System.Threading;
using Volo.Abp.EntityFrameworkCore.DependencyInjection;

namespace Volo.Abp.Domain.FreeSql
{ 
	public class FreeSqlRepository<TDbContext, TEntity, TKey> : FreeSqlRepository<TDbContext>, IFreeSqlRepository, IUnitOfWorkEnabled
		where TDbContext : IEfCoreDbContext
		where TEntity : class,IEntity<TKey>
		where TKey : class
	{
		public FreeSqlRepository(IDbContextProvider<TDbContext> dbContextProvider) : base(dbContextProvider)
		{
		}

		

	}

	public class FreeSqlRepository<TDbContext> : IFreeSqlRepository, IUnitOfWorkEnabled
		where TDbContext : IEfCoreDbContext
	{
		private readonly IDbContextProvider<TDbContext> _dbContextProvider;

		public FreeSqlRepository(IDbContextProvider<TDbContext> dbContextProvider)
		{
			_dbContextProvider = dbContextProvider;
		}

		public IDbConnection DbConnection =>_dbContextProvider.GetDbContext().Database.GetDbConnection();

		public IDbTransaction DbTransaction =>
			_dbContextProvider.GetDbContext().Database.CurrentTransaction?.GetDbTransaction();

		//https://github.com/dotnetcore/FreeSql/issues/267
		/// <summary>
		/// 不建议直接操作freesql实例
		/// </summary>
		protected IFreeSql FreeSql => DbConnection.GetIFreeSql();

		public ISelect<TEntity> Select<TEntity>() where TEntity:class
		{
			return _dbContextProvider.GetDbContext().Database.CurrentTransaction == null ?
				   _dbContextProvider.GetDbContext().Database.GetDbConnection().Select<TEntity>()
				:  _dbContextProvider.GetDbContext().Database.CurrentTransaction.GetDbTransaction().Select<TEntity>();
		}

		public IInsert<TEntity> Insert<TEntity>() where TEntity : class
		{
			return _dbContextProvider.GetDbContext().Database.CurrentTransaction == null ?
				_dbContextProvider.GetDbContext().Database.GetDbConnection().Insert<TEntity>()
				: _dbContextProvider.GetDbContext().Database.CurrentTransaction.GetDbTransaction().Insert<TEntity>();
		}

		public IUpdate<TEntity> Update<TEntity>() where TEntity : class
		{
			return _dbContextProvider.GetDbContext().Database.CurrentTransaction == null ?
				_dbContextProvider.GetDbContext().Database.GetDbConnection().Update<TEntity>()
				: _dbContextProvider.GetDbContext().Database.CurrentTransaction.GetDbTransaction().Update<TEntity>();
		}

		public IInsertOrUpdate<TEntity> InsertOrUpdate<TEntity>() where TEntity : class
		{
			return _dbContextProvider.GetDbContext().Database.CurrentTransaction == null ?
				_dbContextProvider.GetDbContext().Database.GetDbConnection().InsertOrUpdate<TEntity>()
				: _dbContextProvider.GetDbContext().Database.CurrentTransaction.GetDbTransaction().InsertOrUpdate<TEntity>();
		}

		public IDelete<TEntity> Delete<TEntity>() where TEntity : class
		{
			return _dbContextProvider.GetDbContext().Database.CurrentTransaction == null ?
				_dbContextProvider.GetDbContext().Database.GetDbConnection().Delete<TEntity>()
				: _dbContextProvider.GetDbContext().Database.CurrentTransaction.GetDbTransaction().Delete<TEntity>();
		}


	}
}
