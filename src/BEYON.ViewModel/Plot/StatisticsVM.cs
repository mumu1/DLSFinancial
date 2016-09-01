using System;
using System.ComponentModel.DataAnnotations;

namespace BEYON.ViewModel.Plot
{
    /// <summary>
    /// 朝代统计
    /// </summary>
    public class DynastyVM
    {
        [Required]
        [Display(Name = "遗址朝代")]
        [StringLength(25)]
        public string Year { get; set; }

        [Display(Name = "个数")]
        public int Number { get; set; }
    }

    /// <summary>
    /// 每月新增遗址数目
    /// </summary>
    public class NewRelicsVM
    {
        //[Required]
        //[Display(Name = "类型")]
        //[StringLength(10)]
        //public string Type { get; set; }

        [Required]
        [Display(Name = "录入时间")]
        [StringLength(10)]
        public string Date { get; set; }

        [Required]
        [Display(Name = "遗址数目")]
        public int Count { get; set; }
    }

    /// <summary>
    /// 查询统计列表
    /// </summary>
    public class StatisticsVM
    {
        private int _count = 1;

        public StatisticsVM()
        {
        }

        [Required]
        [Display(Name = "遗址编号")]
        [StringLength(36)]
        public string UmrID { get; set; }

        [Display(Name = "遗址名称")]
        [StringLength(100)]
        public string Name { get; set; }

        [Required]
        [Display(Name = "遗址朝代")]
        [StringLength(25)]
        public string Year { get; set; }

        [Required]
        [Display(Name = "遗址朝代")]
        public int count { get { return _count; } set { _count = value; } }

        [Display(Name = "录入时间")]
        public DateTime OperateTime { get; set; }
    }
}
