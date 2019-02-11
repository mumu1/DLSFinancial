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
        */
    }
}
