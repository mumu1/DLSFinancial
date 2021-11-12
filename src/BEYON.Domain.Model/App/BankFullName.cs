/************************************
 * 描述：银行字典表--BankFullName（银行全称字典表）
 * 作者：张硕
 * 日期：2021/03/11
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
    [Description("银行全称字典表")]
    public class BankFullName : EntityBase<int>
    {
        public BankFullName()
        { 
        
        }

        [Required]
        [Display(Name = "银行全称代码")]
        [StringLength(20)]
        public string BankFullCode { get; set; }

        [Display(Name = "银行全称名称")]
        [StringLength(150)]
        public string BankFullNameAddr { get; set; }

    }
}
