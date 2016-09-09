using System;
using System.ComponentModel.DataAnnotations;

namespace BEYON.ViewModel.App
{
    public class AuditOpinionVM
    {
        public AuditOpinionVM()
        {

        }
        [Required]
        [Display(Name = "审核意见编码")]
        [StringLength(10)]
        public string AuditOpinionCode { get; set; }

        [Display(Name = "审核意见描述")]
        [StringLength(100)]
        public string AuditOpinionDesp { get; set; }
    }
}
