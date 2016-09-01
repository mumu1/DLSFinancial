/************************************
 * 描述：尚未添加描述
 * 作者：林永恒
 * 日期：2015/11/11 14:05:25  
*************************************/

using System.ComponentModel.DataAnnotations.Schema;

namespace BEYON.Component.Data.Configurations.Member
{
    internal partial class UserGroupConfiguration
    {
        partial void UserGroupConfigurationAppend()
        {
            this.Property(t => t.Id).HasColumnName("Id").HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            this.HasMany(g => g.Users).WithMany(u => u.UserGroups);
            this.HasMany(g => g.Roles).WithMany(r => r.UserGroups);
        }


    }
}
