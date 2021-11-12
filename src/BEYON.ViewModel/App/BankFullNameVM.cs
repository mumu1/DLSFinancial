using System;
using System.ComponentModel.DataAnnotations;

namespace BEYON.ViewModel.App
{
    public class BankFullNameVM
    {
        public BankFullNameVM()
        {

        }
        [Required]
        [Display(Name = "银行全称代码")]
        [StringLength(20)]
        public string BankFullCode { get; set; }

        [Display(Name = "银行全称名称")]
        [StringLength(150)]
        public string BankFullNameAddr { get; set; }
    }
}
