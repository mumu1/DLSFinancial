using System;
using System.ComponentModel.DataAnnotations;

namespace BEYON.ViewModel.App
{
    public class DepartmentVM
    {
        public DepartmentVM()
        {

        }
        [Required]
        [Display(Name = "部门编码")]
        [StringLength(20)]
        public string DepartmentCode { get; set; }

        [Display(Name = "部门名称")]
        [StringLength(100)]
        public string DepartmentName { get; set; }
    }
}
