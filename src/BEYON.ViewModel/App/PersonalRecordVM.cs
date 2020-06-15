using System;
using System.ComponentModel.DataAnnotations;

namespace BEYON.ViewModel.App
{
    public class PersonalRecordVM
    {
        public PersonalRecordVM()
        {
        }

        [Required]
        [Display(Name = "申请单流水号")]
        [StringLength(36)]
        public string SerialNumber { get; set; }

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

        [Display(Name = "金额(元)")]
        public Double Amount { get; set; }

        [Display(Name = "是否含税")]
        [StringLength(36)]
        public string TaxOrNot { get; set; }

        [Display(Name = "开户银行")]
        [StringLength(100)]
        public string Bank { get; set; }

        [Display(Name = "开户行详细名称")]
        [StringLength(100)]
        public string BankDetailName { get; set; }

        [Display(Name = "银行账号")]
        [StringLength(36)]
        public string AccountNumber { get; set; }

        [Display(Name = "账户名称")]
        [StringLength(100)]
        public string AccountName { get; set; }

        [Display(Name = "支付类型")]
        [StringLength(36)]
        public string PaymentType { get; set; }

        [Display(Name = "收款账号省份")]
        [StringLength(200)]
        public string ProvinceCity { get; set; }

        [Display(Name = "收款账号地市")]
        [StringLength(150)]
        public string CityField { get; set; }

        [Display(Name = "性别")]
        [StringLength(6)]
        public string Gender { get; set; }

        [Display(Name = "出生日期")]
        [StringLength(12)]
        public string Birth { get; set; }

        [Display(Name = "领取人签字")]
        [StringLength(36)]
        public string Signature { get; set; }
    }
}
