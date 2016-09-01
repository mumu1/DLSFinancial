/************************************
 * 描述：调后标绘数据表--Importantsites（重要遗迹信息表）
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
    [Description("重要遗迹信息表")]
    public class ImportAntsites : EntityBase<int>
    {
        public ImportAntsites()
        { 
        
        }

        [Required]
        [Display(Name = "遗址编号")]
        [StringLength(36)]
        public string UmrID { get; set; }

        [Display(Name = "遗迹编号")]
        [StringLength(36)]
        public string SiteID { get; set; }

        [Display(Name = "遗迹名称")]
        [StringLength(50)]
        public string Name { get; set; }

        [Display(Name = "遗迹图片路径")]
        [StringLength(512)]
        public string Path { get; set; }

        [Display(Name = "备注")]
        [StringLength(512)]
        public string Remark { get; set; }
    }
}
