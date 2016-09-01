/************************************
 * 描述：调后标绘数据表--audit（审核信息数据表）
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
    [Description("文物保护单位审核信息数据")]
    public class ProtectUnitAudit : EntityBase<int>
    {
        public ProtectUnitAudit()
        { 
        
        }

        [Required]
        [Display(Name = "文物保护单位编号")]
        [StringLength(36)]
        public string ProtectunitID { get; set; }

        [Display(Name = "录入员编号")]
        [StringLength(30)]
        public string OperatorID { get; set; }

        [Display(Name = "录入员")]
        [StringLength(50)]
        public string Operator { get; set; }

        [Display(Name = "录入时间")]
        public DateTime OperateTime { get; set; }

        [Display(Name = "项目审核人编号")]
        [StringLength(30)]
        public string LeaderAuditorID { get; set; }

        [Display(Name = "项目审核人")]
        [StringLength(30)]
        public string LeaderAuditor { get; set; }

        [Display(Name = "项目负责人审核时间")]
        public DateTime LeaderAuditTime { get; set; }

        [Display(Name = "项目负责人审核状态")]
        [StringLength(10)]
        public string LeaderAuditStatus { get; set; }

        [Display(Name = "项目负责人审核意见")]
        [StringLength(255)]

        public string LeaderAuditps { get; set; }

        [Display(Name = "项目负责人修改内容")]
        [StringLength(255)]
        public string LeaderAuditEdit { get; set; }

        [Display(Name = "所里审核人编号")]
        [StringLength(30)]
        public string InstituteAuditorID { get; set; }

        [Display(Name = "所里审核人")]
        [StringLength(30)]
        public string InstituteAuditor { get; set; }

        [Display(Name = "所里审核时间")]     
        public DateTime InstituteAuditTime { get; set; }

        [Display(Name = "所里审核状态")]
        [StringLength(10)]
        public string InstituteAuditStatus { get; set; }
        [Display(Name = "所里修改内容")]
        [StringLength(255)]
        public string InstituteAuditEdit { get; set; }


        [Display(Name = "所里审核意见")]
        [StringLength(255)]
        public string InstituteAuditps { get; set; }

        [Display(Name = "局里审核人编号")]
        [StringLength(30)]
        public string BureauAuditorID { get; set; }

        [Display(Name = "局里审核人")]
        [StringLength(30)]
        public string BureauAuditor { get; set; }

        [Display(Name = "局里审核时间")]
        public DateTime BureauAuditTime { get; set; }

        [Display(Name = "局里审核状态")]
        [StringLength(10)]
        public string BureauAuditStatus { get; set; }
        [Display(Name = "局里修改内容")]
        [StringLength(255)]
        public string BureauAuditEdit { get; set; }


        [Display(Name = "局里审核意见")]
        [StringLength(255)]
        public string BureauAuditps { get; set; }

        [Display(Name = "审核状态")]
        [StringLength(20)]
        public string AuditStatus { get; set; }
    }
}
