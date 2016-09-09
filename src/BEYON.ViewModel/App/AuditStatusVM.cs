using System;
using System.ComponentModel.DataAnnotations;

namespace BEYON.ViewModel.App
{
    public class AuditStatusVM
    {
        public AuditStatusVM()
        {

        }
        [Required]
        [Display(Name = "状态编码")]
        [StringLength(10)]
        public string StatusCode { get; set; }

        [Display(Name = "审核状态")]
        [StringLength(20)]
        public string StatusName { get; set; }
    }
}
