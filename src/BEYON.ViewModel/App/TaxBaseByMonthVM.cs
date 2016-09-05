using System;
using System.ComponentModel.DataAnnotations;

namespace BEYON.ViewModel.App
{
    public class TaxBaseByMonthVM
    {
        public TaxBaseByMonthVM()
        {

        }
        [Required]
        [Display(Name = "证件号码")]
        [StringLength(36)]
        public string CertificateID { get; set; }

        [Display(Name = "姓名")]
        [StringLength(36)]
        public string Name { get; set; }

        [Display(Name = "证件类型")]
        [StringLength(36)]
        public String CertificateType { get; set; }

        [Display(Name = "初始应发数")]
        public Double InitialEaring { get; set; }

        [Display(Name = "免税额")]
        public Double TaxFree { get; set; }

        [Display(Name = "基本扣除")]
        public Double AmountDeducted { get; set; }

        [Display(Name = "初始应纳税所得额")]
        public Double InitialTaxPayable { get; set; }

        [Display(Name = "初始税额")]
        public Double InitialTax { get; set; }

        [Display(Name = "人员类型")]
        [StringLength(36)]
        public string PersonType { get; set; }

        [Display(Name = "职称")]
        [StringLength(36)]
        public string Title { get; set; }
    }
}
