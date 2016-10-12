/************************************
 * 描述：部门字典表--Department（部门字典表）
 * 作者：李翠翠
 * 日期：2016/10/12
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
    [Description("部门字典表")]
    public class Department : EntityBase<int>
    {
        public Department()
        { 
        
        }

        [Required]
        [Display(Name = "部门代码")]
        [StringLength(20)]
        public string DepartmentCode { get; set; }

        [Display(Name = "部门名称")]
        [StringLength(100)]
        public string DepartmentName { get; set; }

    }
}
