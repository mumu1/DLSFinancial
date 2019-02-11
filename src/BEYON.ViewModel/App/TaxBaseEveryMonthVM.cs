using System;
using System.ComponentModel.DataAnnotations;

namespace BEYON.ViewModel.App
{
    public class TaxBaseEveryMonthVM
    {
        public TaxBaseEveryMonthVM()
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

        [Display(Name = "期间")]
        [StringLength(50)]
        public String Period { get; set; }

        [Display(Name = "专项扣除额")]
        public Double SpecialDeduction { get; set; }

        [Display(Name = "月收入总额")]
        public Double TotalIncome { get; set; }

        [Display(Name = "月税总额")]
        public Double TotalTax { get; set; }

        [Display(Name = "年度不含税收入累计")]
        public Double TotalTemp { get; set; }


        [Display(Name = "当前已累计月")]
        public String LastMonths { get; set; }
/*
        [Display(Name = "人员类型")]
        [StringLength(36)]
        public string PersonType { get; set; }

        [Display(Name = "职称")]
        [StringLength(36)]
        public string Title { get; set; }
 * */
    }
}
