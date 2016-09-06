using System;
using System.ComponentModel.DataAnnotations;

namespace BEYON.ViewModel.App
{
    public class TitleVM
    {
        public TitleVM()
        {

        }
        [Required]
        [Display(Name = "职称编码")]
        [StringLength(10)]
        public string TitleCode { get; set; }

        [Display(Name = "职称名称")]
        [StringLength(36)]
        public string TitleName { get; set; }
    }
}
