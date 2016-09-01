using System;
using System.ComponentModel.DataAnnotations;

namespace BEYON.ViewModel.Plot
{
    public class BasicPropertyVM
    {
        public BasicPropertyVM()
        {

        }
        [Required]
        [Display(Name = "遗址编号")]
        [StringLength(36)]
        public string UmrID { get; set; }

        [Display(Name = "代码")]
        [StringLength(12)]
        public string Code { get; set; }

        [Display(Name = "地址及位置")]
        [StringLength(255)]
        public string Address { get; set; }

        [Display(Name = "经度")]
        [StringLength(25)]
        public string Longitude { get; set; }

        [Display(Name = "纬度")]
        [StringLength(25)]
        public string Latitude { get; set; }

        [Display(Name = "经度")]
        [StringLength(25)]
        public string Longitude2 { get; set; }

        [Display(Name = "纬度")]
        [StringLength(25)]
        public string Latitude2 { get; set; }

        [Display(Name = "海拔高程")]
        public double Altitude { get; set; }

        [Display(Name = "测点说明")]
        [StringLength(255)]
        public string PointDescription { get; set; }

        [Display(Name = "Rank")]
        public int Rank { get; set; }

        [Display(Name = "SpreadArea")]
        public int SpreadArea { get; set; }

        [Display(Name = "BuildingTakeoffArea")]
        public int BuildingTakeoffArea { get; set; }

        [Display(Name = "AvoidBuildingArea")]
        public int AvoidBuildingArea { get; set; }

        [Display(Name = "年代")]
        [StringLength(25)]
        public string Year { get; set; }

        [Display(Name = "统计年代")]
        [StringLength(25)]
        public string YearForCount { get; set; }

        [Display(Name = "面积")]
        [StringLength(12)]
        public string Category { get; set; }

        [Display(Name = "所有权")]
        [StringLength(50)]
        public string Ownership { get; set; }

        [Display(Name = "使用单位（或人）")]
        [StringLength(25)]
        public string Owner { get; set; }

        [Display(Name = "隶属")]
        [StringLength(25)]
        public string Vestin { get; set; }

        [Display(Name = "用途")]
        [StringLength(50)]
        public string Purpose { get; set; }

        [Display(Name = "单体文物数量")]
        public int SingleRelicNumber { get; set; }

        [Display(Name = "单体文物说明")]
        [StringLength(512)]
        public string SingleRelicDescription { get; set; }

        [Display(Name = "简介")]
        [StringLength(1024)]
        public string Brief { get; set; }

        [Display(Name = "现状评估")]
        public int StateEvaluation { get; set; }

        [Display(Name = "现状描述")]
        [StringLength(1024)]
        public string StateDescription { get; set; }

        [Display(Name = "自然因素")]
        [StringLength(1024)]
        public string NaturalFactor { get; set; }

        [Display(Name = "人为因素")]
        [StringLength(1024)]
        public string ManualFactor { get; set; }

        [Display(Name = "损坏原因描述")]
        [StringLength(1024)]
        public string DestroyReason { get; set; }

        [Display(Name = "人文环境")]
        [StringLength(1024)]
        public string Soceity { get; set; }

        [Display(Name = "普查组建议")]
        [StringLength(512)]
        public string TeamSuggestion { get; set; }

        [Display(Name = "审核意见")]
        [StringLength(512)]
        public string AuditSight { get; set; }

        [Display(Name = "抽查结果")]
        [StringLength(25)]
        public string CheckResult { get; set; }

        [Display(Name = "备注")]
        [StringLength(512)]
        public string Remark { get; set; }

        [Display(Name = "Rank1")]
        public int Rank1 { get; set; }

        [Display(Name = "YearForCount1")]
        [StringLength(25)]
        public string YearForCount1 { get; set; }

        [Display(Name = "Area1")]
        public int Area1 { get; set; }

        [Display(Name = "ownership1")]
        [StringLength(12)]
        public string ownership1 { get; set; }

        [Display(Name = "标记坐标")]
        [StringLength(200)]
        public string Coordinate { get; set; }

        [Display(Name = "文化分期")]
        [StringLength(50)]
        public string CulturalStage { get; set; }

        [Display(Name = "学术认识")]
        [StringLength(1024)]
        public string Academic { get; set; }
        [Display(Name = "修改内容")]
        [StringLength(300)]
        public string Edit { get; set; }
    }
}
