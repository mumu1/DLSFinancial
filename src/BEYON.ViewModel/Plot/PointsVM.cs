using System;
using System.ComponentModel.DataAnnotations;

namespace BEYON.ViewModel.Plot
{
    public class PointsVM
    {
        public PointsVM()
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
        [Display(Name = "审核状态")]
        [StringLength(200)]
        public string Audit { get; set; }
        [Display(Name = "用户角色")]
        [StringLength(50)]
        public string RoleId { get; set; }
        [Display(Name = "修改内容")]
        [StringLength(300)]
        public string Edit { get; set; }
        [Display(Name = "当前模块")]
        [StringLength(20)]
        public string CurrentModel { get; set; }
    }
}
