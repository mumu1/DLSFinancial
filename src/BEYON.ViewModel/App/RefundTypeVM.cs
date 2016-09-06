using System;
using System.ComponentModel.DataAnnotations;

namespace BEYON.ViewModel.App
{
    public class RefundTypeVM
    {
        public RefundTypeVM()
        {

        }
        [Required]
        [Display(Name = "科目代码")]
        [StringLength(10)]
        public string RefundTypeCode { get; set; }

        [Display(Name = "报销事由")]
        [StringLength(36)]
        public string RefundTypeName { get; set; }
    }
}
