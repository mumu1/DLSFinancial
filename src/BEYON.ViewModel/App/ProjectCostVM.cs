using System;
using System.ComponentModel.DataAnnotations;

namespace BEYON.ViewModel.App
{
    public class ProjectCostVM
    {
        public ProjectCostVM()
        {

        }
        [Required]
        [Display(Name = "课题号")]
        [StringLength(36)]
        public string ProjectNumber { get; set; }

        [Display(Name = "金额")]
        public Double Amount { get; set; }

        [Display(Name = "报销事由")]
        [StringLength(100)]
        public String RefundType { get; set; }

        [Display(Name = "课题负责人")]
        [StringLength(36)]
        public string ProjectDirector { get; set; }

        [Display(Name = "工资薪金税额")]
        public Double SalaryTaxAmount { get; set; }

        [Display(Name = "劳务费税额")]
        public Double LabourTaxAmount { get; set; }

        [Display(Name = "支付类型")]
        [StringLength(36)]
        public string PaymentType { get; set; }    
    }
}
