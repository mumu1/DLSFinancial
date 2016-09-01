/************************************
 * 描述：调后标绘数据表--umrcover（遗址基本信息表）
 * 作者：张硕
 * 日期：2016/6/30
*************************************/

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BEYON.Component.Tools;

namespace BEYON.Domain.Model.Plot
{
    [Description("遗址基本信息")]
    public class Umrcover : EntityBase<int>
    {
        public Umrcover() { 
        
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
        public String SearchType { get; set; }

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
        public DateTime CollectDate { get; set; }

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
        public string PlotBeforeID { get; set; }
    }
}
