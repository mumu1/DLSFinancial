using System;
using System.ComponentModel.DataAnnotations;

namespace BEYON.ViewModel.Plot
{
    public class IndexVM
    {
        public IndexVM()
        {

        }
        [Display(Name = "经度")]
        [StringLength(36)]
        public string jd { get; set; }

        [Display(Name = "纬度")]
        [StringLength(30)]
        public string wd { get; set; }

        [Display(Name = "功能")]
        [StringLength(50)]
        public string Operator { get; set; }

        
    }
}

