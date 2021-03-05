using System;
using System.Collections.Generic;
using System.Text;
using FreeSql;
using Volo.Abp.EntityFrameworkCore;
using System.Collections.Concurrent;
namespace Volo.Abp.FreeSql
{
    /// <summary>
    /// 根据TDbContext对应的FreeSql的配置
    /// 最后在配置EF DbContext层 添加AbpFreeSqlOption配置
    /// </summary>
    public class AbpFreeSqlOption
    {
        private ConcurrentDictionary<Type,List< Action<IFreeSql>>> FreeSqlDic { get; } = new ConcurrentDictionary<Type,List<Action<IFreeSql>>>();

        public void ConfigureFreeSql<TDbContext>(Action<IFreeSql> action) where TDbContext : IAbpEfCoreDbContext {

            Check.NotNull(action, nameof(action));
            var list = FreeSqlDic.GetOrAdd(typeof(TDbContext), () => new List<Action<IFreeSql>>());
            list.Add(action);
        }


        internal List<Action<IFreeSql>> GetFreeAction<TDbContext>() where TDbContext : IAbpEfCoreDbContext {
            
            var list = FreeSqlDic.GetOrAdd(typeof(TDbContext), () => new List<Action<IFreeSql>>());
            return list;
        }

        internal List<Action<IFreeSql>> GetFreeAction(Type type)
        {
            
            var list = FreeSqlDic.GetOrAdd(type, () => new List<Action<IFreeSql>>());
            return list;
        }
    }
}
