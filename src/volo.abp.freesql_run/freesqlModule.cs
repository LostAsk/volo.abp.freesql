using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using Volo.Abp.Autofac;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.FreeSql;
using Volo.Abp.Modularity;

namespace volo.abp.freesql_run
{
    /// <summary>
    /// 引入依赖AbpFreeSqlModule (AbpAutofacModule只是主程序运行该模块需要注入)
    /// </summary>
    [DependsOn(
        typeof(AbpAutofacModule),
        typeof(AbpFreeSqlModule)
    )]
    public class freesqlModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            ///无关代码，只是在这个模块当作运行模块而已
            var configuration = context.Services.GetConfiguration();
            var hostEnvironment = context.Services.GetSingletonInstance<IHostEnvironment>();
            ////添加TestDbContext注册到服务
            context.Services.AddAbpDbContext<TestDbContext>(options =>
            {
                ///取消EF通用仓储绑定到TestDbContext
                options.AddDefaultRepositories(false);
            });
            ///DbContext配置
            Configure<AbpDbContextOptions>(option =>
            {
                ///默认TestDbContext用SqlServer
                option.UseSqlServer();
            });
            ///Freesql配置
            Configure<AbpFreeSqlOption>(opt =>
            {
                ///TestDbContext对应的Freesql初始化Action
                ///注意Action会放到AbpFreeSqlOption列表上，因此多次 ConfigureFreeSql<TestDbContext>配置是追加Action，不是覆盖原有的Action
                opt.ConfigureFreeSql<TestDbContext>((freesql =>
                {
                    freesql.Aop.CommandBefore += (_, e) => Console.WriteLine(e.Command.CommandText);

                }));
            });
            ///无关代码，只是在这个模块当作运行模块需要而已
            context.Services.AddHostedService<freesqlHostedService>();
        }


    }
}
