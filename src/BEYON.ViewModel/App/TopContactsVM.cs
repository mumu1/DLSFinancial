//using System;
using System.ComponentModel.DataAnnotations;

namespace BEYON.ViewModel.App
{
    public class TopContactsVM
    {
        public TopContactsVM()
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
        public string CertificateType { get; set; }

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
      
    }
}
