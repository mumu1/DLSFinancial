/************************************
 * 描述：调后标绘数据表--samples（出土文物信息表）
 * 作者：张硕
 * 日期：2016/6/30
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
    [Description("出土文物信息")]
    public class Samples : EntityBase<int>
    {
        public Samples()
        { 
        
        }

        [Required]
        [Display(Name = "遗址编号")]
        [StringLength(36)]
        public string UmrID { get; set; }

        [Display(Name = "出土文物编号")]
        [StringLength(36)]
        public string SampleID { get; set; }

        [Display(Name = "序号")]
        [StringLength(12)]
        public string Counter { get; set; }

        [Display(Name = "文物名称")]
        [StringLength(50)]
        public string Name { get; set; }

        [Display(Name = "用户编号")]
        [StringLength(20)]
        public string UserID { get; set; }

        [Display(Name = "质地")]
        [StringLength(50)]
        public string Material { get; set; }

        [Display(Name = "年代")]
        [StringLength(25)]
        public string Year { get; set; }

        [Display(Name = "保存地点")]
        [StringLength(50)]
        public string SavePlace { get; set; }

        [Display(Name = "备注")]
        [StringLength(512)]
        public string Remark { get; set; }
      
       
    }
}
