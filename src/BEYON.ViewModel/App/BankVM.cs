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
        [Display(Name = "开户银行")]
        [StringLength(100)]
        public string BankName { get; set; }
    }
}
