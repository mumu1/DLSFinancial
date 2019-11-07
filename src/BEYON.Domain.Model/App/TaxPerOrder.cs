/************************************
 * 描述：申请单表--TaxPerOrder（单笔税金表）
 * 作者：张硕
 * 日期：2016/09/01
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
    [Description("单笔税金表")]
    public class TaxPerOrder : EntityBase<int>
    {
        public TaxPerOrder()
        { 
        
        }

        [Required]
        [Display(Name = "申请单流水号")]
        [StringLength(36)]
        public string SerialNumber { get; set; }

        [Display(Name = "课题号")]
        [StringLength(36)]
        public string ProjectNumber { get; set; }

        [Display(Name = "课题名称")]
        [StringLength(100)]
        public string TaskName { get; set; }

        [Display(Name = "姓名")]
        [StringLength(36)]
        public string Name { get; set; }

        [Display(Name = "报销事由")]
        [StringLength(100)]
        public String RefundType { get; set; }

        [Display(Name = "课题负责人")]
        [StringLength(36)]
        public string ProjectDirector { get; set; }

        [Display(Name = "经办人")]
        [StringLength(36)]
        public string Agent { get; set; }

        [Display(Name = "人员类型")]
        [StringLength(36)]
        public string PersonType { get; set; }

        [Display(Name = "证件类型")]
        [StringLength(36)]
        public String CertificateType { get; set; }

        [Display(Name = "证件号码")]
        [StringLength(36)]
        public string CertificateID { get; set; }

        [Display(Name = "金额（元）")]
        public Double Amount { get; set; }

        [Display(Name = "是否含税")]
        [StringLength(36)]
        public string TaxOrNot { get; set; }

        [Display(Name = "税金")]
        public Double Tax { get; set; }

        [Display(Name = "税前金额Y")]
        public Double AmountY { get; set; }

        [Display(Name = "税后金额X")]
        public Double AmountX { get; set; }

        [Display(Name = "开户银行")]
        [StringLength(100)]
        public string Bank { get; set; }

        [Display(Name = "开户银行名称")]
        [StringLength(100)]
        public string BankDetailName { get; set; }

        [Display(Name = "银行存折账号")]
        [StringLength(36)]
        public string AccountNumber { get; set; }

        [Display(Name = "账户名称")]
        [StringLength(100)]
        public string AccountName { get; set; }

        [Display(Name = "支付类型")]
        [StringLength(36)]
        public string PaymentType { get; set; }

        [Display(Name = "收款账号省市")]
        [StringLength(200)]
        public string ProvinceCity { get; set; }

    }
}
