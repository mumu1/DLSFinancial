using System;
using System.ComponentModel.DataAnnotations;

namespace BEYON.ViewModel.Plot
{
    public class TitleVM
    {
        public TitleVM()
        {

        }
        [Required]
        [Display(Name = "职称名称")]
        [StringLength(36)]
        public string TitleName { get; set; }
    }
}
