/************************************
 * 描述：尚未添加描述
 * 作者：林永恒
 * 日期：2015/11/26 17:25:36  
*************************************/

using System;
using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace BEYON.ViewModel.Plot
{
    public class SamplesVM
    {
        public SamplesVM()
        {
            
        }
        [Required(ErrorMessage = "编号不能为空")]
        [Display(Name = "遗址编号")]
        [StringLength(36)]
        public string UmrID { get; set; }

        [Required(ErrorMessage = "标绘名称不能为空")]
        [Display(Name = "出土文物编号")]
        [StringLength(36)]
        public string SampleID { get; set; }

        [Display(Name = "序号")]
        [StringLength(12)]
        public string Counter { get; set; }

        [Display(Name = "文物名称")]
        [StringLength(50)]
        public string Name { get; set; }

        [Display(Name = "用户编号")]
        [StringLength(20)]
        public string UserID { get; set; }
        [Display(Name = "质地")]
        [StringLength(50)]
        public string Material { get; set; }

        [Display(Name = "年代")]
        [StringLength(25)]
        public string Year { get; set; }

        [Display(Name = "保存地点")]
        [StringLength(50)]
        public string SavePlace { get; set; }

        [Display(Name = "备注")]
        [StringLength(512)]
        public string Remark { get; set; }

        [Display(Name = "审核状态")]
        [StringLength(50)]
        public string Audit { get; set; }
        [Display(Name = "用户角色")]
        [StringLength(50)]
        public string RoleId { get; set; }
        [Display(Name = "修改内容")]
        [StringLength(300)]
        public string Edit { get; set; }
        [Display(Name = "当前模块")]
        [StringLength(20)]
        public string CurrentModel { get; set; }
        //public string StrUpdateDate
        //{
        //    get
        //    {
        //        return MarkTime.ToString();
        //    }
        //}
    }
}
