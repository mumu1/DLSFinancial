﻿/************************************
 * 描述：申请单表--ApplicationForm（申请单表）
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
    [Description("申请单表")]
    public class ApplicationForm : EntityBase<int>
    {
        public ApplicationForm()
        { 
        
        }

        [Required]
        [Display(Name = "申请单流水号")]
        [StringLength(36)]
        public string SerialNumber { get; set; }

        [Display(Name = "课题号")]
        [StringLength(36)]
        public string ProjectNumber { get; set; }

        [Display(Name = "报销事由")]
        [StringLength(100)]
        public String RefundType { get; set; }

        [Display(Name = "课题负责人")]
        [StringLength(36)]
        public string ProjectDirector { get; set; }

        [Display(Name = "经办人")]
        [StringLength(36)]
        public string Agent { get; set; }

        [Display(Name = "审核状态")]
        [StringLength(36)]
        public string AuditStatus { get; set; }

        [Display(Name = "提交时间")]
        public DateTime SubmitTime { get; set; }

        [Display(Name = "审核时间")]
        public DateTime AuditTime { get; set; }

        [Display(Name = "审核意见")]
        [StringLength(255)]
        public string AuditOpinion { get; set; }

        [Display(Name = "报销合计")]
        public Double Summation { get; set; }

    }
}
