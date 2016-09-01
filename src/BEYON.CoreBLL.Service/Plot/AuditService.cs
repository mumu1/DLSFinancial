using BEYON.Component.Data.EF.Interface;
using BEYON.Component.Tools;
using BEYON.CoreBLL.Service.Plot.Interface;
using BEYON.Domain.Data.Repositories.Plot;
using BEYON.Domain.Model.Plot;
using BEYON.ViewModel.Plot;
using System.Collections.Generic;
using System.Linq;
using EntityFramework.Extensions;
using System;
using BEYON.ViewModel.Member;

namespace BEYON.CoreBLL.Service.Plot
{
    public class AuditService : CoreServiceBase, IAuditService
    {
         private readonly IAuditRepository _AuditRepository;



         public AuditService(IAuditRepository auditRepository, IUnitOfWork unitOfWork)
            : base(unitOfWork)
        {
            this._AuditRepository = auditRepository;
        }
         public IQueryable<Audit> Audits
        {
            get { return _AuditRepository.Entities; }
        }

         public OperationResult Insert(AuditVM model)
        {
            try
            {
                Audit oldPlot = _AuditRepository.Entities.FirstOrDefault(c => c.UmrID == model.UmrID.Trim());
                if (oldPlot != null)
                {
                    return new OperationResult(OperationResultType.Warning, "数据库中已经存在相同的遗址信息，请修改后重新提交！");
                }
                if(model.UmrID==null || model.UmrID.Trim()=="")
                    return new OperationResult(OperationResultType.Warning, "遗址编号不能为空，请修改后重新提交！");

                var entity = new Audit
                {
                    OperatorID = model.OperatorID,
                    Operator = model.Operator,
                    OperateTime =Convert.ToDateTime(model.OperateTime),
                    LeaderAuditorID = model.LeaderAuditorID,
                    LeaderAuditor = model.LeaderAuditor,
                    LeaderAuditTime =Convert.ToDateTime( model.LeaderAuditTime),
                    LeaderAuditStatus = model.LeaderAuditStatus,
                    LeaderAuditEdit=model.LeaderAuditEdit,
                    LeaderAuditps = model.LeaderAuditps,
                    InstituteAuditorID = model.InstituteAuditorID,
                    InstituteAuditor = model.InstituteAuditor,
                    InstituteAuditTime = Convert.ToDateTime(model.InstituteAuditTime),
                    InstituteAuditStatus = model.InstituteAuditStatus,
                    InstituteAuditEdit=model.InstituteAuditEdit,
                    UmrID = model.UmrID,
                    InstituteAuditps = model.InstituteAuditps,                  
                    BureauAuditorID = model.BureauAuditorID,
                    BureauAuditor = model.BureauAuditor,
                    BureauAuditTime = Convert.ToDateTime(model.BureauAuditTime),
                    BureauAuditStatus = model.BureauAuditStatus,
                    BureauAuditEdit=model.BureauAuditEdit,
                    BureauAuditps = model.BureauAuditps,
                    AuditStatus=model.AuditStatus,
                    UpdateDate = DateTime.Now
                };
                _AuditRepository.Insert(entity);

                return new OperationResult(OperationResultType.Success, "新增数据成功！");
            }
            catch
            {
                return new OperationResult(OperationResultType.Error, "新增数据失败，数据库插入数据时发生了错误!");
            }
        }
         public OperationResult Update(AuditVM model)
        {
            try
            {
                var user = Audits.FirstOrDefault(c => c.UmrID == model.UmrID);
                if (user == null)
                {
                    throw new Exception();
                }
                
                user.AuditStatus = model.AuditStatus;
                //if(model.BureauAuditor==null)
                    user.BureauAuditor = model.BureauAuditor;
                user.BureauAuditorID = model.BureauAuditorID;
                user.BureauAuditps = model.BureauAuditps;
               
                user.InstituteAuditor = model.InstituteAuditor;
                user.InstituteAuditorID = model.InstituteAuditorID;
                user.InstituteAuditps = model.InstituteAuditps;
                if (model.LeaderAuditStatus == "on")
                {
                    user.BureauAuditStatus = null;
                    user.InstituteAuditStatus = null;
                    user.LeaderAuditStatus = "审核中";
                    user.AuditStatus = "审核中";
                }
                else if (model.LeaderAuditStatus == "off")
                {
                    user.BureauAuditStatus = null;
                    user.InstituteAuditStatus = null;
                    user.LeaderAuditStatus = "退回";
                    user.AuditStatus = "退回";
                }
                else
                {
                    user.LeaderAuditStatus = model.LeaderAuditStatus;
                }

                if (model.InstituteAuditStatus == "on")
                {
                    user.BureauAuditStatus = "待审核";
                    user.LeaderAuditStatus = "审核中";
                    user.InstituteAuditStatus = "审核中";
                    user.AuditStatus = "审核中";
                }
                else if (model.InstituteAuditStatus == "off")
                {
                    user.BureauAuditStatus = null;
                    user.InstituteAuditStatus = "退回";
                    user.LeaderAuditStatus = "退回";
                    user.AuditStatus = "退回";
                }
                else
                {
                    user.InstituteAuditStatus = model.InstituteAuditStatus;
                }
                user.InstituteAuditTime = Convert.ToDateTime(model.InstituteAuditTime);
                user.LeaderAuditor = model.LeaderAuditor;
                user.LeaderAuditorID = model.LeaderAuditorID;
                user.LeaderAuditps = model.LeaderAuditps;
               
                user.LeaderAuditTime = Convert.ToDateTime(model.LeaderAuditTime);
                if (model.BureauAuditStatus == "on")
                {
                    user.BureauAuditStatus = "通过";
                    user.LeaderAuditStatus = "通过";
                    user.InstituteAuditStatus = "通过";
                    user.AuditStatus = "通过";
                }
                else if (model.BureauAuditStatus == "off")
                {
                    user.BureauAuditStatus = "退回";
                    user.LeaderAuditStatus = "退回";
                    user.InstituteAuditStatus = "退回";
                    user.AuditStatus = "退回";
                }
                else
                {
                    user.BureauAuditStatus = model.BureauAuditStatus;
                }
                user.BureauAuditTime = Convert.ToDateTime(model.BureauAuditTime);
                //if (user.BureauAuditStatus == "通过")
                //{
                //    user.AuditStatus = "通过";
                //}else if(user.BureauAuditStatus == "退回")
                //{
                //    user.AuditStatus = "退回";
                //}
                user.LeaderAuditEdit = model.LeaderAuditEdit;
                user.InstituteAuditEdit = model.InstituteAuditEdit;
                user.BureauAuditEdit = model.BureauAuditEdit;
                user.UpdateDate = DateTime.Now;
                _AuditRepository.Update(user);
                return new OperationResult(OperationResultType.Success, "提交数据成功！");

            }
            catch
            {
                return new OperationResult(OperationResultType.Error, "提交数据失败!");
            }
        }
         public OperationResult UMrIDDelete(List<string> list)
         {
             try
             {
                 if (list != null)
                 {
                     //var sampleIds = list.Select(c => c.SampleID).ToList();
                     int count = _AuditRepository.Delete(_AuditRepository.Entities.Where(c => list.Contains(c.UmrID)));
                     if (count > 0)
                     {
                         return new OperationResult(OperationResultType.Success, "删除数据成功！");
                     }
                     else
                     {
                         return new OperationResult(OperationResultType.Error, "删除数据失败!");
                     }
                 }
                 else
                 {
                     return new OperationResult(OperationResultType.ParamError, "参数错误，请选择需要删除的数据!");
                 }
             }
             catch
             {
                 return new OperationResult(OperationResultType.Error, "删除数据失败!");
             }
         }
         public OperationResult Delete(IEnumerable<AuditVM> list)
        {
            try
            {
                if (list != null)
                {
                    var auditIds = list.Select(c => c.UmrID).ToList();
                    int count = _AuditRepository.Delete(_AuditRepository.Entities.Where(c => auditIds.Contains(c.UmrID)));
                    //int count = _AuditRepository.Entities.Where(c => auditIds.Contains(c.UmrID)).Delete();
                    if (count > 0)
                    {
                        return new OperationResult(OperationResultType.Success, "删除数据成功！");
                    }
                    else
                    {
                        return new OperationResult(OperationResultType.Error, "删除数据失败!");
                    }
                }
                else
                {
                    return new OperationResult(OperationResultType.ParamError, "参数错误，请选择需要删除的数据!");
                }
            }
            catch
            {
                return new OperationResult(OperationResultType.Error, "删除数据失败!");
            }
        }
    }
}
