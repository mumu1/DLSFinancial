/************************************
 * 描述：报销事由字典表--RefundType（报销事由字典表）
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
    [Description("报销事由字典表")]
    public class RefundType : EntityBase<int>
    {
        public RefundType()
        { 
        
        }

        [Required]
        [Display(Name = "报销事由")]
        [StringLength(36)]
        public string RefundTypeName { get; set; }

    }
}
