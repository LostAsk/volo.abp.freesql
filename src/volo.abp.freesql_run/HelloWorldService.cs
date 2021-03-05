using System;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Uow;
using FreeSql;
using System.Data.Common;

namespace volo.abp.freesql_run
{
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
}
