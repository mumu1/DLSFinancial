using System;
using System.ComponentModel.DataAnnotations;

namespace BEYON.ViewModel.Plot
{
    public class UmrcoverVM
    {
        public UmrcoverVM()
        {

        }
        [Required]
        [Display(Name = "遗址编号")]
        [StringLength(36)]
        public string UmrID { get; set; }

        [Display(Name = "用户编号")]
        [StringLength(20)]
        public string UserID { get; set; }

        [Display(Name = "普查类型")]
        [StringLength(50)]
        public string SearchType { get; set; }

        [Display(Name = "遗址名称")]
        [StringLength(100)]
        public string Name { get; set; }

        [Display(Name = "行政区域代码")]
        [StringLength(20)]
        public string RegionCode { get; set; }

        [Display(Name = "自治区、直辖市")]
        [StringLength(50)]
        public string Province { get; set; }

        [Display(Name = "市（地区、州、盟）")]
        [StringLength(50)]
        public string City { get; set; }

        [Display(Name = "县（区、市、旗）")]
        [StringLength(50)]
        public string Country { get; set; }

        [Display(Name = "调查人")]
        [StringLength(200)]
        public string Collector { get; set; }

        [Display(Name = "调查日期")]
        public string CollectDate { get; set; }

        [Display(Name = "审定人")]
        [StringLength(200)]
        public string Auditor { get; set; }

        [Display(Name = "审定日期")]
        public DateTime AuditDate { get; set; }

        [Display(Name = "抽查人")]
        [StringLength(200)]
        public string Checker { get; set; }

        [Display(Name = "抽查日期")]
        public DateTime CheckDate { get; set; }

        [Display(Name = "调前关联ID")]
        [StringLength(30)]
        public string MarkCode { get; set; }

        [Display(Name = "审核状态")]
        [StringLength(30)]
        public string AuditStatus { get; set; }
        [Display(Name = "修改内容")]
        [StringLength(300)]
        public string Edit { get; set; }
    }
}
