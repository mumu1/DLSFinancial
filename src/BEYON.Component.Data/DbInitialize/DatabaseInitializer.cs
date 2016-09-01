/************************************
 * 描述：尚未添加描述
 * 作者：林永恒
 * 日期：2015/9/8 11:15:26  
*************************************/

using System.Data.Entity;
using BEYON.Component.Data.EF;
using BEYON.Component.Data.Migrations;

namespace BEYON.Component.Data.DbInitialize
{
    /// <summary>
    /// 数据库初始化操作类
    /// </summary>
    public static class DatabaseInitializer
    {
        public static void Initialize()
        {
            //启用自动迁移数据库配置
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<EFDbContext, Configuration>());
        }
    }
}
