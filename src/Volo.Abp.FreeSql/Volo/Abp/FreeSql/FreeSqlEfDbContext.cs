using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.DependencyInjection;
using FreeSql;
using System.Collections.Concurrent;
namespace Volo.Abp.FreeSql
{
    /// <summary>
    /// FreeSql的EF上下文<para></para>
    /// (主要是根据AbpFreeSqlOption和TDbContext进行freesql初始化)<para></para>
    /// 不建议直接操作freesql实例
    /// </summary>
    /// <typeparam name="TDbContext"></typeparam>
    public abstract class FreeSqlEfDbContext<TDbContext> : AbpDbContext<TDbContext> where TDbContext : Microsoft.EntityFrameworkCore.DbContext
    {
        protected FreeSqlEfDbContext(DbContextOptions<TDbContext> options) : base(options) { 
        }
        private static ConcurrentDictionary<Type,bool> set_dic= new ConcurrentDictionary<Type, bool>();
        private static object lockss=new object();
        public override void Initialize(AbpEfCoreDbContextInitializationContext initializationContext)
        {
            lock (lockss) {
                var is_set = set_dic.GetOrAdd(this.GetType(), () => false);
                if (!is_set)
                {
                    base.Initialize(initializationContext);
                    var opt = initializationContext.UnitOfWork.ServiceProvider.GetService<IOptions<AbpFreeSqlOption>>();
                    var actions = opt.Value.GetFreeAction(this.GetType());
                    var free=this.Database.GetDbConnection().GetIFreeSql();
                    foreach (var action in actions) {
                        action(free);
                    }
                    set_dic[this.GetType()] = true;
                }
            }
        }
    }
}
