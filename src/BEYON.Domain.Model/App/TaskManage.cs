/************************************
 * 描述：课题字典表--Task（科研处维护）
 * 作者：张硕
 * 日期：2016/10/10
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
    [Description("课题字典表")]
    public class TaskManage : EntityBase<int>
    {
        public TaskManage()
        { 
        
        }

        [Required]
        [Display(Name = "课题号")]
        [StringLength(100)]
        public string TaskID { get; set; }

        [Display(Name = "课题名称")]
        [StringLength(300)]
        public string TaskName { get; set; }

        [Display(Name = "课题负责人")]
        [StringLength(50)]
        public string TaskLeader { get; set; }

        [Display(Name = "可用资金")]
        public Double AvailableFund { get; set; }

        [Display(Name = "赤字运行")]
        [StringLength(4)]
        public string Deficit { get; set; }
    }
}
