/************************************
 * 描述：每月初始化工资表--TaxBaseByMonth（每月初始化工资表）
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
    [Description("每月初始化工资表")]
    public class TaxBaseByMonth : EntityBase<int>
    {
        public TaxBaseByMonth()
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
