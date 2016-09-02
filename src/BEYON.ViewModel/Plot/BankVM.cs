using System;
using System.ComponentModel.DataAnnotations;

namespace BEYON.ViewModel.Plot
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
