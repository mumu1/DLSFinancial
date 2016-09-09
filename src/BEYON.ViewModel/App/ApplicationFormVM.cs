using System;
using System.ComponentModel.DataAnnotations;

namespace BEYON.ViewModel.App
{
    public class ApplicationFormVM
    {
        public ApplicationFormVM()
        {

        }
        [Required]
        [Display(Name = "申请单流水号")]
        [StringLength(36)]
        public string SerialNumber { get; set; }

        [Display(Name = "课题号")]
        [StringLength(36)]
        public string ProjectNumber { get; set; }

        [Display(Name = "报销事由")]
        [StringLength(100)]
        public String RefundType { get; set; }

        [Display(Name = "课题负责人")]
        [StringLength(36)]
        public string ProjectDirector { get; set; }

        [Display(Name = "经办人")]
        [StringLength(36)]
        public string Agent { get; set; }

        [Display(Name = "审核状态")]
        [StringLength(36)]
        public string AuditStatus { get; set; }

        [Display(Name = "提交时间")]
        public DateTime SubmitTime { get; set; }

        [Display(Name = "审核时间")]
        public DateTime AuditTime { get; set; }

        [Display(Name = "审核意见")]
        [StringLength(255)]
        public string AuditOpinion { get; set; }

        [Display(Name = "用户Email")]
        [StringLength(100)]
        public string UserEmail { get; set; }

        [Display(Name = "报销合计")]
        public Double Summation { get; set; }
    }
}
