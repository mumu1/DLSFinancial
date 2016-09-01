using System;
using System.ComponentModel.DataAnnotations;

namespace BEYON.ViewModel.Plot
{
    public class ProtectUnitAuditVM
    {
        public ProtectUnitAuditVM()
        {

        }
        [Required]
        [Display(Name = "文物保护单位编号")]
        [StringLength(36)]
        public string ProtectunitID { get; set; }

        [Display(Name = "录入员编号")]
        [StringLength(30)]
        public string OperatorID { get; set; }

        [Display(Name = "录入员")]
        [StringLength(50)]
        public string Operator { get; set; }

        [Display(Name = "录入时间")]
        public string OperateTime { get; set; }

        [Display(Name = "项目审核人编号")]
        [StringLength(30)]
        public string LeaderAuditorID { get; set; }

        [Display(Name = "项目审核人")]
        [StringLength(30)]
        public string LeaderAuditor { get; set; }

        [Display(Name = "项目负责人审核时间")]
        public string LeaderAuditTime { get; set; }

        [Display(Name = "项目负责人审核状态")]
        [StringLength(10)]
        public string LeaderAuditStatus { get; set; }

        [Display(Name = "项目负责人审核意见")]
        [StringLength(255)]
        public string LeaderAuditps { get; set; }

        [Display(Name = "项目负责人修改内容")]
        [StringLength(255)]
        public string LeaderAuditEdit{ get; set; }

        [Display(Name = "所里审核人编号")]
        [StringLength(30)]
        public string InstituteAuditorID { get; set; }

        [Display(Name = "所里审核人")]
        [StringLength(30)]
        public string InstituteAuditor { get; set; }

        [Display(Name = "所里审核时间")]
        public string InstituteAuditTime { get; set; }

        [Display(Name = "所里审核状态")]
        [StringLength(10)]
        public string InstituteAuditStatus { get; set; }

        [Display(Name = "所里审核意见")]
        [StringLength(255)]
        public string InstituteAuditps { get; set; }
        [Display(Name = "所里修改内容")]
        [StringLength(255)]
        public string InstituteAuditEdit { get; set; }

        [Display(Name = "局里审核人编号")]
        [StringLength(30)]
        public string BureauAuditorID { get; set; }

        [Display(Name = "局里审核人")]
        [StringLength(30)]
        public string BureauAuditor { get; set; }

        [Display(Name = "局里审核时间")]
        public string BureauAuditTime { get; set; }

        [Display(Name = "局里审核状态")]
        [StringLength(10)]
        public string BureauAuditStatus { get; set; }

        [Display(Name = "局里审核意见")]
        [StringLength(255)]
        public string BureauAuditps { get; set; }
        [Display(Name = "局里修改内容")]
        [StringLength(255)]
        public string BureauAuditEdit{ get; set; }

        [Display(Name = "审核状态")]
        [StringLength(20)]
        public string AuditStatus { get; set; }
        [Display(Name = "当前角色")]
        [StringLength(20)]
        public string CurrentRole { get; set; }
        [Display(Name = "当前模块")]
        [StringLength(20)]
        public string CurrentModel { get; set; }
    }
}
