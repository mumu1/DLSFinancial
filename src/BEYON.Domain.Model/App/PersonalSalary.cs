/************************************
 * 描述：申请单表--PersonalSalary（所内个人工资表）
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
    [Description("所内个人工资表")]
    public class PersonalSalary : EntityBase<int>
    {
        public PersonalSalary()
        { 
        
        }

        [Required]
        [Display(Name = "姓名")]
        [StringLength(50)]
        public string Name { get; set; }

        [Display(Name = "证件类型")]
        [StringLength(36)]
        public String CertificateType { get; set; }

        [Display(Name = "证件号码")]
        [StringLength(36)]
        public string CertificateID { get; set; }

        [Display(Name = "收入额")]
        public Double Earning { get; set; }

        [Display(Name = "免税金额")]
        public Double TaxFree { get; set; }

        [Display(Name = "基本扣除")]
        public Double AmountDeducted { get; set; }

        [Display(Name = "已扣缴税额")]
        public Double WithHolding { get; set; }

        [Display(Name = "应纳税额")]
        public Double TaxPayable { get; set; }

        [Display(Name = "税率")]
        public Double TaxRate { get; set; }

        [Display(Name = "速算扣除数")]
        public Double QuickCalDeduction { get; set; }

    }
}
