/************************************
 * 描述：调后标绘数据表--points（测量控制点信息表）
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
    [Description("测量控制点信息")]
    public class Points : EntityBase<int>
    {
        public Points()
        { 
        
        }

        [Required]
        [Display(Name = "遗址编号")]
        [StringLength(36)]
        public string UmrID { get; set; }

        [Display(Name = "测点ID")]
        [StringLength(36)]
        public string PointID { get; set; }

        [Display(Name = "序号")]
        [StringLength(12)]
        public string Counter { get; set; }

        [Display(Name = "用户编号")]
        [StringLength(20)]
        public string UserID { get; set; }

        [Display(Name = "经度")]
        [StringLength(25)]
        public string Longitude { get; set; }

        [Display(Name = "纬度")]
        [StringLength(25)]
        public string Latitude { get; set; }

        [Display(Name = "海拔高程")]
        public double Altitude { get; set; }

        [Display(Name = "测点说明")]
        [StringLength(1024)]
        public string PointDescription { get; set; }

        [Display(Name = "备注")]
        [StringLength(512)]
        public string Remark { get; set; }

        [Display(Name = "标记坐标")]
        [StringLength(200)]
        public string Coordinate { get; set; }
    }
}
