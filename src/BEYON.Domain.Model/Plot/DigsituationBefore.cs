/************************************
 * 描述：调后标绘数据表--DigsituationBefore（以往发掘情况表）
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
    [Description("以往发掘情况表")]
    public class DigsituationBefore : EntityBase<int>
    {
        public DigsituationBefore()
        { 
        
        }

        [Required]
        [Display(Name = "遗址编号")]
        [StringLength(36)]
        public string UmrID { get; set; }

        [Display(Name = "发掘区编号")]
        [StringLength(36)]
        public string DigareaID { get; set; }

        [Display(Name = "发掘区名称")]
        [StringLength(50)]
        public string Name { get; set; }

        [Display(Name = "发掘区面积")]
        [StringLength(20)]
        public string Digarea { get; set; }

        [Display(Name = "分布情况")]
        [StringLength(200)]
        public string Distribution { get; set; }

        [Display(Name = "发掘时间")]
        public DateTime Time { get; set; }

        [Display(Name = "备注")]
        [StringLength(512)]
        public string Remark { get; set; }
    }
}
