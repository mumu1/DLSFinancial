using System;
using System.ComponentModel.DataAnnotations;

namespace BEYON.ViewModel.Plot
{
    public class LayerDepositVM
    {
        public LayerDepositVM()
        {

        }
        [Required]
        [Display(Name = "遗址编号")]
        [StringLength(36)]
        public string UmrID { get; set; }

        [Display(Name = "致密度")]
        [StringLength(36)]
        public string Density { get; set; }

        [Display(Name = "堆积形状")]
        [StringLength(255)]
        public string Shape { get; set; }

        [Display(Name = "土色")]
        [StringLength(100)]
        public string Earthcolor { get; set; }

        [Display(Name = "土质")]
        [StringLength(100)]
        public string Soil { get; set; }

        [Display(Name = "包含物")]
        [StringLength(255)]
        public String Inclusions { get; set; }

        [Display(Name = "保存状况")]
        [StringLength(255)]
        public string Savesituation { get; set; }

        [Display(Name = "清理方式")]
        [StringLength(100)]
        public String Clearway { get; set; }

        [Display(Name = "堆积性质")]
        [StringLength(255)]
        public string Properties { get; set; }

        [Display(Name = "其它")]
        [StringLength(255)]
        public String Other { get; set; }

        [Display(Name = "备注")]
        [StringLength(512)]
        public string Remark { get; set; }
        [Display(Name = "修改内容")]
        [StringLength(300)]
        public string Edit { get; set; }
    }
}
