/************************************
 * 描述：调后标绘数据表--Literature（相关文献表）
 * 作者：张硕
 * 日期：2016/07/01
*************************************/

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BEYON.Component.Tools;

namespace BEYON.Domain.Model.Plot
{
    [Description("相关文献表")]
    public class Literature : EntityBase<int>
    {
        public Literature()
        { 
        
        }

        [Required]
        [Display(Name = "遗址编号")]
        [StringLength(36)]
        public string UmrID { get; set; }

        [Display(Name = "文献编号")]
        [StringLength(36)]
        public string LiteratureID { get; set; }

        [Display(Name = "文献名称")]
        [StringLength(200)]
        public string Name { get; set; }

        [Display(Name = "文献类型")]
        [StringLength(30)]
        public string Category { get; set; }

        [Display(Name = "文献路径")]
        [StringLength(512)]
        public string Path { get; set; }

        [Display(Name = "备注")]
        [StringLength(512)]
        public string Remark { get; set; }
    }
}
