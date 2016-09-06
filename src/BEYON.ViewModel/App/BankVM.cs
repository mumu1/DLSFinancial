using System;
using System.ComponentModel.DataAnnotations;

namespace BEYON.ViewModel.App
{
    public class BankVM
    {
        public BankVM()
        {

        }
        [Required]
        [Display(Name = "银行编码")]
        [StringLength(10)]
        public string BankCode { get; set; }

        [Display(Name = "开户银行")]
        [StringLength(100)]
        public string BankName { get; set; }
    }
}
