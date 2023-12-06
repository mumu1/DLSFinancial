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
        [StringLength(50)]
        public string Name { get; set; }

        [Display(Name = "证件类型")]
        [StringLength(36)]
        public String CertificateType { get; set; }

        [Display(Name = "本期初始税前收入额")]
        public Double InitialEaring { get; set; }

        //[Display(Name = "免税额")]
        //public Double TaxFree { get; set; }

        [Display(Name = "本期基本扣除")]
        public Double AmountDeducted { get; set; }

        [Display(Name = "本期应纳税所得额")]
        public Double InitialTaxPayable { get; set; }

        [Display(Name = "本期初始已扣缴税额")]
        public Double InitialTax { get; set; }

        [Display(Name = "期间")]
        [StringLength(50)]
        public String Period { get; set; }

        [Display(Name = "本期专项附加扣除")]
        public Double SpecialDeduction { get; set; }

        [Display(Name = "本期免税收入")]
        public Double TaxFreeIncome { get; set; }

        [Display(Name = "本期养老保险")]
        public Double EndowmentInsurance { get; set; }

        [Display(Name = "本期失业保险")]
        public Double UnemployedInsurance { get; set; }

        [Display(Name = "本期医疗保险")]
        public Double MedicalInsurance { get; set; }

        [Display(Name = "本期职业年金")]
        public Double OccupationalAnnuity { get; set; }

        [Display(Name = "本期住房公积金")]
        public Double HousingFund { get; set; }

        [Display(Name = "本期初始税后收入额")]
        public Double InitialAfterTaxIncome { get; set; }

        [Display(Name = "年度累计减免税额")]
        public Double CutTax { get; set; }

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
