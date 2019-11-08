/************************************
 * 描述：申请单表--PersonalRecord（个人申请信息表）
 * 作者：张硕
 * 日期：2016/09/02
*************************************/

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BEYON.Component.Tools;

namespace BEYON.Domain.Model.App
{
    [Description("个人常用领款人信息表")]
    public class TopContacts : EntityBase<int>
    {
        public TopContacts()
        { 
        
        }

        [Required]
        [Display(Name = "用户ID")]
        [StringLength(36)]
        public string UserID { get; set; }

        [Display(Name = "姓名")]
        [StringLength(36)]
        public string Name { get; set; }

        [Display(Name = "证件类型")]
        [StringLength(36)]
        public String CertificateType { get; set; }

        [Display(Name = "证件号码")]
        [StringLength(36)]
        public string CertificateID { get; set; }

        [Display(Name = "单位")]
        [StringLength(150)]
        public string Company { get; set; }

        [Display(Name = "联系电话")]
        [StringLength(36)]
        public string Tele { get; set; }

        [Display(Name = "人员类型")]
        [StringLength(36)]
        public string PersonType { get; set; }

        [Display(Name = "国籍")]
        [StringLength(100)]
        public string Nationality { get; set; }

        [Display(Name = "职称")]
        [StringLength(36)]
        public string Title { get; set; }

        [Display(Name = "开户银行")]
        [StringLength(100)]
        public string Bank { get; set; }

        [Display(Name = "开户行详细名称")]
        [StringLength(100)]
        public string BankDetailName { get; set; }

        [Display(Name = "银行账号")]
        [StringLength(36)]
        public string AccountNumber { get; set; }

        [Display(Name = "收款账号省市")]
        [StringLength(200)]
        public string ProvinceCity { get; set; }

        [Display(Name = "性别")]
        [StringLength(6)]
        public string Gender { get; set; }

        [Display(Name = "出生日期")]
        [StringLength(12)]
        public string Birth { get; set; }

    }
}
