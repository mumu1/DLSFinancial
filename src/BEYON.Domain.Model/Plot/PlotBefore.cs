/************************************
 * 描述：尚未添加描述
 * 作者：林永恒
 * 日期：2015/9/15 15:45:32  
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
    [Description("调前标绘")]
    public class PlotBefore : EntityBase<int>
    {
        public PlotBefore()
        {

        }
        [Required]
        [Display(Name = "编号")]
        [StringLength(30)]
        public string MarkCode { get; set; }

        [Required]
        [Display(Name = "标绘名称")]
        [StringLength(20)]
        public string PlotName { get; set; }

        [Display(Name = "标绘人员编号")]
        [StringLength(100)]
        public string MarkPersonId { get; set; }

        [Display(Name = "标绘人员姓名")]
        [StringLength(100)]
        public string MarkPerson { get; set; }

        [Display(Name = "标绘时间")]
        public DateTime MarkTime { get; set; }

        [Display(Name = "标记坐标")]
        [StringLength(200)]
        public string Coordinate { get; set; }

        [Display(Name = "调查状态")]
        [StringLength(50)]
        public string PlotStatus { get; set; }

    }
}