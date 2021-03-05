FreeSql是一款优秀的Orm框架,首先感谢FreeSql的作者,引入Volo.Abp.FreeSql 作为扩展模块,集成到Abp.VNext中.方便应用.
使用者所做任何事情也与本作者无关。
觉得好用请给个赞

FreeSql与Abp Venxt的设计模式不一致，导致FreeSql某些功能不能在Abp Venxt应用

须知:

每个 SqlConnection GetFreeSql() 返回的 IFreeSql 实例相同；即 同一个TDbContext类型 的多个DbContext实例 返回的 IFreeSql 实例相同

可以对 fsql 设置 Aop 事件，比如监视 SQL；

IFreeSql 的ICodeFirst 不可用

IFreeSql 自身的成员 IDbFirst、Transaction 不可用；


## 安装

> dotnet add package AbpVNext.FreeSql

## 使用

```csharp
    ///在一个模块引入Volo.Abp.FreeSql


    public class TestModel { 
        [Column(IsIdentity =true,IsPrimary =true)]
        public int Id { get;  set; }
        public string a { get; set; }
        public int b { get; set; }
    }

    
    /// <summary>
    /// 定义Ef的DbContext 继承FreeSqlEfDbContext<TDbContext>
    /// </summary>
    [ConnectionStringName("Default")]
    public class TestDbContext : FreeSqlEfDbContext<TestDbContext> {
        public TestDbContext(DbContextOptions<TestDbContext> options) : base(options) { }
    }

    /// <summary>
    /// 定义仓储接口 继承 IFreeSqlRepository 有Freesql的增删改查功能
    /// </summary>
    public interface ITestRepository : IFreeSqlRepository { 
    
    }

    /// <summary>
    /// 接口实现，继承FreeSqlRepository<TDbContext>抽象类,默认实现了FreeSql的增删改查功能
    /// </summary>
    public class TestRepository : FreeSqlRepository<TestDbContext>, ITestRepository
    {
        public TestRepository(IDbContextProvider<TestDbContext> dbContextProvider) : base(dbContextProvider)
        {
        }
    }

    /// <summary>
    /// 使用仓储示例
    /// </summary>
    public class HelloWorldService : ITransientDependency
    {
        private ITestRepository TestRepository;

        private IUnitOfWorkManager UnitOfWorkManager;
        public HelloWorldService(IUnitOfWorkManager unitOfWorkManager,ITestRepository testRepository) {
            TestRepository = testRepository;
            UnitOfWorkManager = unitOfWorkManager;
        }
        /// <summary>
        /// 使用仓储
        /// </summary>
        /// <returns></returns>
        public async Task FreeSqlTestAsync()
        {
            using (var uow = UnitOfWorkManager.Begin(new AbpUnitOfWorkOptions { IsTransactional = true, IsolationLevel = System.Data.IsolationLevel.Serializable }))
            {
                var list = await TestRepository.Select<TestModel>().ToListAsync();
            }
        }
        public void SayHello()
        {
            Console.WriteLine("Hello World!");
        }


    }

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

```

