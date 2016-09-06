/************************************
 * 描述：开户银行字典表--Bank（开户银行字典表）
 * 作者：张硕
 * 日期：2016/09/01
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
    [Description("开户银行字典表")]
    public class Bank : EntityBase<int>
    {
        public Bank()
        { 
        
        }

        [Required]
        [Display(Name = "银行编码")]
        [StringLength(10)]
        public string BankCode { get; set; }

        [Display(Name = "开户银行")]
        [StringLength(100)]
        public string BankName { get; set; }

    }
}
