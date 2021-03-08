using System;
using FreeSql;
using volo.abp.freesql_run;
namespace freesql_table
{
    class Program
    {
        static void Main(string[] args)
        {
            IFreeSql fsql = new FreeSql.FreeSqlBuilder()
             .UseConnectionString(FreeSql.DataType.SqlServer, "Server=localhost;Database=test;Trusted_Connection=True;MultipleActiveResultSets=true")
             .UseAutoSyncStructure(true) //自动同步实体结构到数据库
             .Build(); //请务必定义成 Singleton 单例模式

           var sql= fsql.CodeFirst.GetComparisonDDLStatements<TestModel>();
            fsql.CodeFirst.SyncStructure<TestModel>();



        }
    }
}
