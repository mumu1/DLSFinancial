/************************************
 * 描述：系统维护时间字典表--Safeguard
 * 作者：张硕
 * 日期：2016/11/16
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
    [Description("系统维护时间字典表")]
    public class SafeguardTime : EntityBase<int>
    {
        public SafeguardTime()
        { 
        
        }

        [Required]
        [Display(Name = "开始时间")]
        public DateTime StartTime { get; set; }

        [Required]
        [Display(Name = "结束时间")]
        public DateTime EndTime { get; set; }

    }
}
