/************************************
 * 描述：职称字典表--Title（职称字典表）
 * 作者：张硕
 * 日期：2016/09/01
*************************************/

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BEYON.Component.Tools;

namespace BEYON.Domain.Model.App
{
    [Description("职称字典表")]
    public class Title : EntityBase<int>
    {
        public Title()
        { 
        
        }

        [Required]
        [Display(Name = "职称名称")]
        [StringLength(36)]
        public string TitleName { get; set; }

    }
}
