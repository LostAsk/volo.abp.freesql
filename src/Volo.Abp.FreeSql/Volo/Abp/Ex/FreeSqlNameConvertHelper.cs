using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FreeSql;
namespace Volo.Abp.FreeSql.Volo.Abp.Ex
{
    /// <summary>
    /// Freesql实体属性与数据库字段Map规则助手
    /// </summary>
    public static class FreeSqlNameConvertHelper
    {
        private static string PascalCaseToUnderScore(string str) => string.Concat(str.Select((x, i) => i > 0 && char.IsUpper(x) ? "_" + x.ToString() : x.ToString()));
        /// <summary>
        /// 将字符串转换为小写
        /// <para></para>
        /// BigApple -> bigapple
        /// </summary>
        public static void EntiryPropertyNameToLower(this IFreeSql freeSql) {
            freeSql.Aop.ConfigEntity += (_, e) => e.ModifyResult.Name = e.EntityType.Name.ToLower();
            freeSql.Aop.ConfigEntityProperty += (_, e) => e.ModifyResult.Name = e.Property.Name.ToLower();
            freeSql.CodeFirst.IsSyncStructureToLower = true;
        }
        /// <summary>
        /// 将字符串转换为大写
        /// <para></para>
        /// BigApple -> BIGAPPLE
        /// </summary>
        public static void EntiryPropertyNameToUpper(this IFreeSql freeSql)
        {
            freeSql.Aop.ConfigEntity += (_, e) => e.ModifyResult.Name = e.EntityType.Name.ToUpper();
            freeSql.Aop.ConfigEntityProperty += (_, e) => e.ModifyResult.Name = e.Property.Name.ToUpper();
            freeSql.CodeFirst.IsSyncStructureToUpper = true;
        }
        /// <summary>
        /// 将帕斯卡命名字符串转换为下划线分隔字符串
        /// <para></para>
        /// BigApple -> Big_Apple
        /// </summary>
        public static void EntiryPropertyNamePascalCaseToUnderscore(this IFreeSql freeSql)
        {
            freeSql.Aop.ConfigEntity += (_, e) => e.ModifyResult.Name = PascalCaseToUnderScore(e.EntityType.Name);
            freeSql.Aop.ConfigEntityProperty += (_, e) => e.ModifyResult.Name = PascalCaseToUnderScore(e.Property.Name);
       
        }
        /// <summary>
        /// 将帕斯卡命名字符串转换为下划线分隔字符串，且转换为全小写
        /// <para></para>
        /// BigApple -> big_apple
        /// </summary>
        public static void EntiryPropertyNamePascalCaseToUnderscoreWithLower(this IFreeSql freeSql)
        {
            freeSql.Aop.ConfigEntity += (_, e) => e.ModifyResult.Name = PascalCaseToUnderScore(e.EntityType.Name).ToLower();
            freeSql.Aop.ConfigEntityProperty += (_, e) => e.ModifyResult.Name = PascalCaseToUnderScore(e.Property.Name).ToLower();
        }
        /// <summary>
        /// 将帕斯卡命名字符串转换为下划线分隔字符串，且转换为全大写
        /// <para></para>
        /// BigApple -> BIG_APPLE
        /// </summary>
        public static void EntiryPropertyNamePascalCaseToUnderscoreWithUpper(this IFreeSql freeSql)
        {
            freeSql.Aop.ConfigEntity += (_, e) => e.ModifyResult.Name = PascalCaseToUnderScore(e.EntityType.Name).ToUpper();
            freeSql.Aop.ConfigEntityProperty += (_, e) => e.ModifyResult.Name = PascalCaseToUnderScore(e.Property.Name).ToUpper();
        }
    }
}
