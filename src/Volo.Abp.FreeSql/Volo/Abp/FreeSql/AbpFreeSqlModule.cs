using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Modularity;
using System;
using Volo.Abp;
using System.Linq;
using System.Timers;
using Microsoft.Extensions.Options;
using Volo.Abp.Domain;
using Volo.Abp.EntityFrameworkCore;

namespace Volo.Abp.FreeSql
{
    [DependsOn(
    typeof(AbpDddDomainModule),
    typeof(AbpEntityFrameworkCoreModule))]
    public class AbpFreeSqlModule : AbpModule
    {
    }
}