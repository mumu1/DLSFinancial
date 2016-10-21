/************************************
 * 描述：尚未添加描述
 * 作者：林永恒
 * 日期：2015/11/11 13:59:27  
*************************************/

using System.ComponentModel.DataAnnotations.Schema;

namespace BEYON.Component.Data.Configurations.Member
{
    internal partial class UserConfiguration
    {
        partial void UserConfigurationAppend()
        {
            this.HasKey(c => c.Id);
            this.Property(c => c.UserName).HasColumnName("UserName").HasMaxLength(20);
            this.Property(c => c.Password).HasMaxLength(32);
            this.Property(c => c.TrueName).HasMaxLength(20);
            //this.Property(c => c.Email).HasMaxLength(50);
            //this.Property(c => c.Address).HasMaxLength(300);
            //this.Property(c => c.Gender).HasMaxLength(10);
            //this.Property(c => c.Title).HasMaxLength(50);
            //this.Property(c => c.Department).HasMaxLength(100);
            //this.Property(c => c.CertificateID).HasMaxLength(50);

            //this.ToTable("User");
            this.Property(t => t.Id).HasColumnName("Id").HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
        }
    }

}
