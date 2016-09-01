/************************************
 * 描述：调后标绘数据表--ProtectUnits（文物保护单位数据表）
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
    [Description("文物保护单位数据表")]
    public class ProtectUnits : EntityBase<int>
    {
        public ProtectUnits()
        { 
        
        }

        [Required]
        [Display(Name = "文物保护单位编号")]
        [StringLength(36)]
        public string ProtectunitID { get; set; }

        [Display(Name = "文物保护单位名称")]
        [StringLength(100)]
        public string Name { get; set; }

        [Display(Name = "批次")]
        [StringLength(36)]
        public string Batch { get; set; }

        [Display(Name = "地点")]
        [StringLength(255)]
        public string Place { get; set; }

        [Display(Name = "年代")]
        [StringLength(30)]
        public String Year { get; set; }

        [Display(Name = "类别")]
        [StringLength(30)]
        public String Category { get; set; }

        [Display(Name = "用户编号")]
        [StringLength(30)]
        public String UserID { get; set; }
        [Display(Name = "备注")]
        [StringLength(512)]
        public string Remark { get; set; }
    }
}
