using FreeSql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Entities;

namespace Volo.Abp.Domain.FreeSql
{

	public interface IFreeSqlRepository: ITransientDependency
	{
		IDbConnection DbConnection { get; }

		IDbTransaction DbTransaction { get; }

		ISelect<TEntity> Select<TEntity>() where TEntity : class;

		IInsert<TEntity> Insert<TEntity>() where TEntity : class;

		IUpdate<TEntity> Update<TEntity>() where TEntity : class;

		IInsertOrUpdate<TEntity> InsertOrUpdate<TEntity>() where TEntity : class;

		IDelete<TEntity> Delete<TEntity>() where TEntity : class;
	}
}
