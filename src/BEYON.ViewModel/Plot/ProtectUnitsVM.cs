using System;
using System.ComponentModel.DataAnnotations;

namespace BEYON.ViewModel.Plot
{
    public class ProtectUnitsVM
    {
        public ProtectUnitsVM()
        {

        }
        [Required]
        [Display(Name = "文物保护单位编号")]
        [StringLength(36)]
        public string ProtectunitID { get; set; }

        [Display(Name = "文物保护单位名称")]
        [StringLength(100)]
        public string Name { get; set; }

        [Display(Name = "批次")]
        [StringLength(36)]
        public string Batch { get; set; }

        [Display(Name = "地点")]
        [StringLength(255)]
        public string Place { get; set; }

        [Display(Name = "年代")]
        [StringLength(30)]
        public String Year { get; set; }

        [Display(Name = "类别")]
        [StringLength(30)]
        public String Category { get; set; }

        [Display(Name = "备注")]
        [StringLength(512)]
        public string Remark { get; set; }

        [Display(Name = "审核状态")]
        [StringLength(50)]
        public string AuditStatus { get; set; }
        [Display(Name = "用户编号")]
        [StringLength(50)]
        public string UseId { get; set; }
        [Display(Name = "用户角色")]
        [StringLength(50)]
        public string RoleId { get; set; }
        [Display(Name = "修改内容")]
        [StringLength(300)]
        public string Edit { get; set; }
    }
}
