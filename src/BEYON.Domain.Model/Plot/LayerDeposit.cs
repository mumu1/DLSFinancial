/************************************
 * 描述：调后标绘数据表--LayerDeposit（地层堆积情况表）
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
    [Description("地层堆积情况表")]
    public class LayerDeposit : EntityBase<int>
    {
        public LayerDeposit()
        { 
        
        }

        [Required]
        [Display(Name = "遗址编号")]
        [StringLength(36)]
        public string UmrID { get; set; }

        [Display(Name = "致密度")]
        [StringLength(36)]
        public string Density { get; set; }

        [Display(Name = "堆积形状")]
        [StringLength(255)]
        public string Shape { get; set; }

        [Display(Name = "土色")]
        [StringLength(100)]
        public string Earthcolor { get; set; }

        [Display(Name = "土质")]
        [StringLength(100)]
        public string Soil { get; set; }

        [Display(Name = "包含物")]
        [StringLength(255)]
        public String Inclusions { get; set; }

        [Display(Name = "保存状况")]
        [StringLength(255)]
        public string Savesituation { get; set; }

        [Display(Name = "清理方式")]
        [StringLength(100)]
        public String Clearway { get; set; }

        [Display(Name = "堆积性质")]
        [StringLength(255)]
        public string Properties { get; set; }

        [Display(Name = "其它")]
        [StringLength(255)]
        public String Other { get; set; }

        [Display(Name = "备注")]
        [StringLength(512)]
        public string Remark { get; set; }
    }
}
