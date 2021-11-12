using System;
using System.ComponentModel.DataAnnotations;

namespace BEYON.ViewModel.App
{
    public class TaskManageVM
    {
        public TaskManageVM()
        {

        }
        [Required]
        [Display(Name = "课题号")]
        [StringLength(100)]
        public string TaskID { get; set; }

        [Display(Name = "课题名称")]
        [StringLength(300)]
        public string TaskName { get; set; }

        [Display(Name = "课题负责人")]
        [StringLength(50)]
        public string TaskLeader { get; set; }

        [Display(Name = "可用资金")]
        public Double AvailableFund { get; set; }

        [Display(Name = "赤字运行")]
        [StringLength(4)]
        public string Deficit { get; set; }
    }
}
