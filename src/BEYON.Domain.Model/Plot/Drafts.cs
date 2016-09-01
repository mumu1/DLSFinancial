/************************************
 * 描述：调后标绘数据表--drafts（图纸册表）
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
    [Description("图纸册")]
    public class Drafts : EntityBase<int>
    {
        public Drafts()
        { 
        
        }

        [Required]
        [Display(Name = "遗址编号")]
        [StringLength(36)]
        public string UmrID { get; set; }

        [Display(Name = "图纸编号")]
        [StringLength(36)]
        public string D_ID { get; set; }

        [Display(Name = "序号")]
        [StringLength(12)]
        public string Counter { get; set; }

        [Display(Name = "图纸名称")]
        [StringLength(50)]
        public string Name { get; set; }

        [Display(Name = "用户编号")]
        [StringLength(20)]
        public string UserID { get; set; }

        [Display(Name = "图号")]
        [StringLength(20)]
        public string DraftID { get; set; }

        [Display(Name = "比例")]
        [StringLength(20)]
        public string Scale { get; set; }

        [Display(Name = "绘制人")]
        [StringLength(25)]
        public string Drawer { get; set; }

        [Display(Name = "绘制时间")]
        public DateTime Time { get; set; }

        [Display(Name = "照片文件路径")]
        [StringLength(255)]
        public string FilePath { get; set; }

        [Display(Name = "后缀")]
        [StringLength(50)]
        public string Suffix { get; set; }

        //[Display(Name = "图纸")]
        //public byte[] Annex { get; set; }

        [Display(Name = "备注")]
        [StringLength(512)]
        public string Remark { get; set; }
    }
}
