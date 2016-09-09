/************************************
 * 描述：审核状态表：t_AuditStatus    审核状态分为：待提交、待审核、审核算税、审核驳回
 * 作者：张硕
 * 日期：2016/09/09
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
    [Description("审核状态表")]
    public class AuditStatus : EntityBase<int>
    {
        public AuditStatus()
        { 
        
        }

        [Required]
        [Display(Name = "状态编码")]
        [StringLength(10)]
        public string StatusCode { get; set; }

        [Display(Name = "审核状态")]
        [StringLength(20)]
        public string StatusName { get; set; }

    }
}
