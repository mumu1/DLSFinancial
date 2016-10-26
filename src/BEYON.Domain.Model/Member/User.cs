/************************************
 * 描述：尚未添加描述
 * 作者：林永恒
 * 日期：2016/5/15 14:27:22  
*************************************/

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BEYON.Component.Tools;

namespace BEYON.Domain.Model.Member
{
    [Description("用户信息")]
    public class User : EntityBase<int>
    {
        public User()
        {
            this.UserGroups = new List<UserGroup>();
            this.Roles = new List<Role>();
        }

        [Required]
        [Description("登录名称")]
        [StringLength(50)]
        public string UserName { get; set; }

        [Required]
        [Description("密码")]
        [StringLength(32)]
        public string Password { get; set; }

        [Required]
        [Description("真实姓名")]
        [StringLength(50)]
        public string TrueName { get; set; }

        //[Description("邮箱")]
        //[StringLength(50)]
        //public string Email { get; set; }

        //[Description("电话")]
        //[StringLength(50)]
        //public string Phone { get; set; }

        //[Description("地址")]
        //[StringLength(300)]
        //public string Address { get; set; }

        [Description("性别")]
        [StringLength(10)]
        public string Gender { get; set; }

        [Description("部门")]
        [StringLength(100)]
        public string Department { get; set; }

        [Description("员工职称")]
        [StringLength(50)]
        public string Title { get; set; }

        [Description("证件号码")]
        [StringLength(50)]
        public string CertificateID { get; set; }

        [Display(Name = "是否激活")]
        public bool Enabled { get; set; }
        /// <summary>
        /// 用户组集合
        /// </summary>
        public virtual ICollection<UserGroup> UserGroups { get; set; }

        /// <summary>
        /// 用户拥有的角色信息集合
        /// </summary>
        public virtual ICollection<Role> Roles { get; set; }

    }
}
