/************************************
 * 描述：调后标绘数据表--others（其它资料表）
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
    [Description("其它资料")]
    public class Others : EntityBase<int>
    {
        public Others()
        { 
        
        }

        [Required]
        [Display(Name = "遗址编号")]
        [StringLength(36)]
        public string UmrID { get; set; }

        [Display(Name = "资料编号")]
        [StringLength(36)]
        public string OtherID { get; set; }

        [Display(Name = "序号")]
        [StringLength(12)]
        public string Counter { get; set; }

        [Display(Name = "资料名称")]
        [StringLength(50)]
        public string Name { get; set; }

        [Display(Name = "用户编号")]
        [StringLength(20)]
        public string UserID { get; set; }

        [Display(Name = "类别")]
        [StringLength(25)]
        public string Category { get; set; }

        [Display(Name = "数量")]        
        public int Number { get; set; }

        [Display(Name = "保存地点")]
        [StringLength(50)]
        public string SavePlace { get; set; }

        [Display(Name = "备注")]
        [StringLength(512)]
        public string Remark { get; set; }

        [Display(Name = "资料文件路径")]
        [StringLength(200)]
        public string FilePath { get; set; }
       
        [Display(Name = "后缀")]
        [StringLength(50)]
        public string Suffix { get; set; }

        //[Display(Name = "其它资料")]
        //public byte[] Annex { get; set; }
    }
}
