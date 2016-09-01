using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using BEYON.Component.Data.EF;
using BEYON.Component.Data.EF.Interface;
using BEYON.Domain.Model.Plot;
using BEYON.ViewModel.Plot;
using BEYON.Domain.Model.Member;

namespace BEYON.Domain.Data.Repositories.Plot.Impl
{
    public partial class AuditRepository
    {
        public NotifyVM GetNotify(int userID)
        {
            NotifyVM notify = new NotifyVM();
            notify.NotifyItems = new List<NotifyItem>();

            User user = Context.Users.FirstOrDefault(u => u.Id == userID);
            if (user == null || user.Roles.Count < 1)
            {
                return notify;
            }

            var role = user.Roles.ToList()[0];
            notify.RoleName = role.RoleName;

            switch (role.RoleName)
            {
                case "数据录入人员":
                    notify.NotifyItems.Add(GetPassOrNotAudit("OperatorID", user.Id.ToString(), "通过"));
                    notify.NotifyItems.Add(GetPassOrNotAudit("OperatorID", user.Id.ToString(), "退回"));
                    break;
                case "项目负责人":
                    notify.NotifyItems.Add(GetNeedAudit( "LeaderAuditorID"));
                    notify.NotifyItems.Add(GetPassOrNotAudit("LeaderAuditorID", user.Id.ToString(), "通过"));
                    notify.NotifyItems.Add(GetPassOrNotAudit("LeaderAuditorID", user.Id.ToString(), "退回"));
                    break;
                case "所里审核人员":
                    notify.NotifyItems.Add(GetNeedAudit("InstituteAuditorID"));
                    notify.NotifyItems.Add(GetPassOrNotAudit("InstituteAuditorID", user.Id.ToString(), "通过"));
                    notify.NotifyItems.Add(GetPassOrNotAudit("InstituteAuditorID", user.Id.ToString(), "退回"));
                    break;
                case "局里审核人员":
                    notify.NotifyItems.Add(GetNeedAudit("BureauAuditorID"));
                    notify.NotifyItems.Add(GetPassOrNotAudit("BureauAuditorID", user.Id.ToString(), "通过"));
                    notify.NotifyItems.Add(GetPassOrNotAudit("BureauAuditorID", user.Id.ToString(), "退回"));
                    break;
            }

            foreach(var item in notify.NotifyItems){
                notify.Total += item.Count;
            }

            return notify;
        }

        /// <summary>
        /// 判断通过或退回审核状态
        /// </summary>
        /// <param name="columName">审核表对应列名,OperatorID,LeaderAuditorID,InstituteAuditorID,BureauAuditorID</param>
        /// <param name="userID">用户ID</param>
        /// <param name="auditStatus">审核状态：退回或通过</param>
        /// <returns>通过或退回个数</returns>
        private NotifyItem GetPassOrNotAudit(string columName, String userID, string auditStatus)
        {
            var count = 0;
            switch(columName)
            {
                case "OperatorID":
                    count = (from p in Context.Audits.Where(w=>w.OperatorID == userID && w.AuditStatus == auditStatus) select p).Count();
                    break;
                case "LeaderAuditorID":
                    count = (from p in Context.Audits.Where(w => w.LeaderAuditorID == userID && w.AuditStatus == auditStatus) select p).Count();
                    break;
                case "InstituteAuditorID":
                    count = (from p in Context.Audits.Where(w => w.InstituteAuditorID == userID && w.AuditStatus == auditStatus) select p).Count();
                    break;
                case "BureauAuditorID":
                    count = (from p in Context.Audits.Where(w => w.BureauAuditorID == userID && w.AuditStatus == auditStatus) select p).Count();
                    break;
            }

            return new NotifyItem() { Name = auditStatus, Count = count };
        }

        private NotifyItem GetNeedAudit(string columnName)
        {
            var count = 0;

            switch (columnName)
            {
                case "LeaderAuditorID":
                    count = (from p in Context.Audits.Where(w => w.AuditStatus == "提交" && String.IsNullOrEmpty(w.LeaderAuditorID)) select p).Count();
                    break;
                case "InstituteAuditorID":
                    count = (from p in Context.Audits.Where(w => w.LeaderAuditStatus == "通过" && String.IsNullOrEmpty(w.InstituteAuditorID)) select p).Count();
                    break;
                case "BureauAuditorID":
                    count = (from p in Context.Audits.Where(w => w.InstituteAuditStatus == "通过" && String.IsNullOrEmpty(w.BureauAuditorID)) select p).Count();
                    break;
            }
            
            return new NotifyItem() { Name = "待审核", Count = count };
        }
    }
}
