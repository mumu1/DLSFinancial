using System;
using System.ComponentModel.DataAnnotations;

namespace BEYON.ViewModel.Plot
{
    public class ImportAntsitesVM
    {
        public ImportAntsitesVM()
        {

        }
        [Required]
        [Display(Name = "遗址编号")]
        [StringLength(36)]
        public string UmrID { get; set; }

        [Display(Name = "遗迹编号")]
        [StringLength(36)]
        public string SiteID { get; set; }

        [Display(Name = "遗迹名称")]
        [StringLength(50)]
        public string Name { get; set; }

        [Display(Name = "遗迹图片路径")]
        [StringLength(512)]
        public string Path { get; set; }

        [Display(Name = "备注")]
        [StringLength(512)]
        public string Remark { get; set; }
         [Display(Name = "审核状态")]
        [StringLength(200)]
        public string Audit { get; set; }
         [Display(Name = "用户编号")]
         [StringLength(20)]
         public string UserID { get; set; }
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
