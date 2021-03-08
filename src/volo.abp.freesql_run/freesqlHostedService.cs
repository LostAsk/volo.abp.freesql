using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Volo.Abp;
using Volo.Abp.Data;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.FreeSql;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.FreeSql;
using FreeSql.DatabaseModel;
using FreeSql.DataAnnotations;
using System.ComponentModel;
namespace volo.abp.freesql_run
{




    [Table(Name = "TestModel2")]
    public class TestModel : Entity<int> , ISoftDelete
    { 
        [Column(IsIdentity =true,IsPrimary =true)]
        public override int Id { get;  protected set; }
        [Description("测试啊")]
        public string a { get; set; }

        public int b { get; set; }
        [Column(MapType = typeof(string))]
        public TesEnum? TesEnum { get; set; }
        public bool IsDeleted { get; set; }
    }

    public enum TesEnum { 
        金,木,水
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




    public class freesqlHostedService : IHostedService
    {
        private readonly IAbpApplicationWithExternalServiceProvider _application;
        private readonly IServiceProvider _serviceProvider;
        private readonly HelloWorldService _helloWorldService;

        public freesqlHostedService(
            IAbpApplicationWithExternalServiceProvider application,
            IServiceProvider serviceProvider,
            HelloWorldService helloWorldService)
        {
            _application = application;
            _serviceProvider = serviceProvider;
            _helloWorldService = helloWorldService;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            _application.Initialize(_serviceProvider);

            _helloWorldService.SayHello();
            await _helloWorldService.FreeSqlTestAsync();
            //return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _application.Shutdown();

            return Task.CompletedTask;
        }
    }
}
