/************************************
 * 描述：按年累计每月工资表--TaxBaseEveryMonth（每月工资及汇总值）
 * 作者：张硕
 * 日期：2019/01/29
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
    [Description("每月初始化工资表")]
    public class TaxBaseEveryMonth : EntityBase<int>
    {
        public TaxBaseEveryMonth()
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

        [Display(Name = "年度累计税前收入")]
        public Double InitialEaring { get; set; }

        //[Display(Name = "免税额")]
        //public Double TaxFree { get; set; }

        [Display(Name = "年度累计基本扣除")]
        public Double AmountDeducted { get; set; }

        [Display(Name = "年度累计应纳税所得额")]
        public Double InitialTaxPayable { get; set; }

       // [Display(Name = "初始税额")]
        //public Double InitialTax { get; set; }

        [Display(Name = "年度")]
        [StringLength(50)]
        public String Period { get; set; }

        [Display(Name = "年度累计专项附加扣除")]
        public Double SpecialDeduction { get; set; }

      //  [Display(Name = "月收入总额")]
        //public Double TotalIncome { get; set; }

       // [Display(Name = "月税总额")]
        //public Double TotalTax { get; set; }

        [Display(Name = "年度累计税后收入")]
        public Double TotalTemp { get; set; }

        [Display(Name = "当前已累计月")]
        public String LastMonths { get; set; }

        [Display(Name = "年度累计免税收入")]
        public Double TaxFreeIncome { get; set; }

        [Display(Name = "年度累计养老保险")]
        public Double EndowmentInsurance { get; set; }

        [Display(Name = "年度累计医疗保险")]
        public Double MedicalInsurance { get; set; }

        [Display(Name = "年度累计职业年金")]
        public Double OccupationalAnnuity { get; set; }

        [Display(Name = "年度累计住房公积金")]
        public Double HousingFund { get; set; }

        [Display(Name = "年度累计失业保险")]
        public Double UnemployedInsurance { get; set; }

        [Display(Name = "年度累计已扣缴税额")]
        public Double TotalTax { get; set; }


        /*
        [Display(Name = "人员类型")]
        [StringLength(36)]
        public string PersonType { get; set; }
 
        [Display(Name = "职称")]
        [StringLength(36)]
        public string Title { get; set; }
        */
    }
}
