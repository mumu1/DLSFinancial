using System;
using System.ComponentModel.DataAnnotations;

namespace BEYON.ViewModel.Plot
{
    public class DigsituationBeforeVM
    {
        public DigsituationBeforeVM()
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
        [StringLength(200)]
        public string Time { get; set; }

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
