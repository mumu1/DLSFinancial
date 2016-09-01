/************************************
 * 描述：调后标绘数据表--photos（照片册表）
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
    [Description("照片册")]
    public class Photos : EntityBase<int>
    {
        public Photos()
        { 
        
        }

        [Required]
        [Display(Name = "遗址编号")]
        [StringLength(36)]
        public string UmrID { get; set; }

        [Display(Name = "图片编号")]
        [StringLength(36)]
        public string P_ID { get; set; }

        [Display(Name = "序号")]
        [StringLength(12)]
        public string Counter { get; set; }

        [Display(Name = "图片名称")]
        [StringLength(50)]
        public string Name { get; set; }

        [Display(Name = "用户编号")]
        [StringLength(20)]
        public string UserID { get; set; }

        [Display(Name = "底片号")]
        [StringLength(20)]
        public string NegativeID { get; set; }

        [Display(Name = "照片号")]
        [StringLength(25)]
        public string PhotoID { get; set; }

        [Display(Name = "摄影者")]
        [StringLength(25)]
        public string Cameraman { get; set; }

        [Display(Name = "拍摄时间")]
        public DateTime Time { get; set; }

        [Display(Name = "拍摄方向")]
        [StringLength(25)]
        public string Orientation { get; set; }

        [Display(Name = "备注")]
        [StringLength(512)]
        public string Remark { get; set; }

        [Display(Name = "照片文件路径")]
        [StringLength(200)]
        public string FilePath { get; set; }

        [Display(Name = "后缀")]
        [StringLength(50)]
        public string Suffix { get; set; }

        //[Display(Name = "图片")]      
        //public byte[] Annex { get; set; }
       
    }
}
