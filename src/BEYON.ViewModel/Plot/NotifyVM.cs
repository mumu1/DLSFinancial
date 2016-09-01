using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BEYON.ViewModel.Plot
{
    public class NotifyItem
    {
        [Display(Name = "消息类型")]
        [StringLength(20)]
        public string Name { get; set; }

        [Display(Name = "消息总数")]
        public int Count { get; set; }
    }

    public class NotifyVM
    {
        [Display(Name = "消息总数")]
        public int Total { get; set; }

        [Display(Name = "用户ID")]
        public int UserID { get; set; }

        [Display(Name = "角色类型")]
        [StringLength(20)]
        public String RoleName { get; set; }

        [Display(Name = "消息总数")]
        public List<NotifyItem> NotifyItems { get; set; }
    }
}
