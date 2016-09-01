/************************************
 * 描述：尚未添加描述
 * 作者：林永恒
 * 日期：2015/9/15 15:45:32  
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
    [Description("调后标绘")]
    public class PlotAfter : EntityBase<int>
    {
        public PlotAfter()
        {
            
        }

        [Required]
        [Display(Name = "标绘名称")]
        [StringLength(20)]
        public string PlotName { get; set; }

        [Display(Name = "描述")]
        [StringLength(100)]
        public string Description { get; set; }
        
    }
}
