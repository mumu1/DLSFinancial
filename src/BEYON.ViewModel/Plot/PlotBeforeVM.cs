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
    public class PlotBeforeVM
    {
        public PlotBeforeVM()
        {

        }
        [Required(ErrorMessage = "编号不能为空")]
        [Display(Name = "编号")]
        [StringLength(30)]
        public string MarkCode { get; set; }

        [Required(ErrorMessage = "标绘名称不能为空")]
        [Display(Name = "标绘名称")]
        [StringLength(20)]
        public string PlotName { get; set; }

        [Display(Name = "标绘人员编号")]
        [StringLength(100)]
        public string MarkPersonId { get; set; }

        [Display(Name = "标绘人员姓名")]
        [StringLength(100)]
        public string MarkPerson { get; set; }

        [Display(Name = "标绘时间")]
        public string MarkTime { get; set; }

        [Display(Name = "经度")]
        [StringLength(100)]
        public string Longitude { get; set; }
        [Display(Name = "纬度")]
        [StringLength(100)]
        public string Latitude { get; set; }

        [Display(Name = "经度2")]
        [StringLength(100)]
        public string Longitude2 { get; set; }

        [Display(Name = "纬度2")]
        [StringLength(100)]
        public string Latitude2 { get; set; }

        [Display(Name = "坐标")]
        [StringLength(100)]
        public string Coordinate { get; set; }

        [Display(Name = "调查状态")]
        [StringLength(50)]
        public string PlotStatus { get; set; }
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