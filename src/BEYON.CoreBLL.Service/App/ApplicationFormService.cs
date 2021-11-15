using System;
using System.Collections.Generic;
using System.Linq;
using BEYON.Component.Data.EF.Interface;
using BEYON.Component.Tools;
using BEYON.CoreBLL.Service.App.Interface;
using BEYON.Domain.Data.Repositories.App;
using BEYON.Domain.Model.App;
using BEYON.ViewModel.App;
using BEYON.CoreBLL.Service.Excel;
using EntityFramework.Extensions;
using BEYON.Component.Data.EF;

namespace BEYON.CoreBLL.Service.App
{
    public class ApplicationFormService : CoreServiceBase, IApplicationFormService
    {
        private readonly log4net.ILog _log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private readonly IApplicationFormRepository _ApplicationFormRepository;

        public ApplicationFormService(IApplicationFormRepository applicationFormRepository, IUnitOfWork unitOfWork)
            : base(unitOfWork)
        {
            this._ApplicationFormRepository = applicationFormRepository;
        }
        public IQueryable<ApplicationForm> ApplicationForms
        {
            get { return _ApplicationFormRepository.Entities; }
        }

        public OperationResult Insert(ApplicationFormVM model)
        {
            try
            {
                ApplicationForm applicationForm = _ApplicationFormRepository.Entities.FirstOrDefault(c => c.SerialNumber == model.SerialNumber.Trim());
                if (applicationForm != null)
                {
                    return new OperationResult(OperationResultType.Warning, "数据库中已经存在相同的申请单，请修改后重新提交！");
                }
                if (model.ProjectNumber == null || model.ProjectNumber.Trim() == "")
                    return new OperationResult(OperationResultType.Warning, "课题编号不能为空，请修改后重新提交！");
                var entity = new ApplicationForm
                {
                    SerialNumber = model.SerialNumber,
                    DepartmentName = model.DepartmentName,
                    ProjectNumber = model.ProjectNumber,
                    TaskName = model.TaskName,
                    RefundType = model.RefundType,
                    ProjectDirector = model.ProjectDirector,
                    PaymentType = model.PaymentType,
                    Agent = model.Agent,
                    AuditStatus = model.AuditStatus,
                    SubmitTime = model.SubmitTime,
                    AuditTime = model.AuditTime,
                    AuditOpinion = model.AuditOpinion,
                    ApplyDesp = model.ApplyDesp,
                    Summation = model.Summation,
                    UserName = model.UserName,
                    UpdateDate = DateTime.Now
                };
                _ApplicationFormRepository.Insert(entity);

                return new OperationResult(OperationResultType.Success, "新增数据成功！");
            }
            catch(Exception ex)
            {
                _log.Error("导入申请表数据失败",ex);
                return new OperationResult(OperationResultType.Error, "新增数据失败，数据库插入数据时发生了错误!");
            }
        }
        public OperationResult Update(ApplicationFormVM model)
        {
            try
            {
                ApplicationForm applicationForm = _ApplicationFormRepository.Entities.FirstOrDefault(c => c.SerialNumber == model.SerialNumber.Trim());
                if (applicationForm == null)
                {
                    throw new Exception();
                }
                applicationForm.SerialNumber = model.SerialNumber;
                applicationForm.ProjectNumber = model.ProjectNumber;
                applicationForm.TaskName = model.TaskName;
                applicationForm.DepartmentName = model.DepartmentName;
                applicationForm.RefundType = model.RefundType;
                applicationForm.ProjectDirector = model.ProjectDirector;
                applicationForm.Agent = model.Agent;
                applicationForm.PaymentType = model.PaymentType;
                applicationForm.AuditStatus = model.AuditStatus;
                applicationForm.SubmitTime = model.SubmitTime;
                applicationForm.AuditTime = model.AuditTime;
                applicationForm.AuditOpinion = model.AuditOpinion;
                applicationForm.ApplyDesp = model.ApplyDesp;
                applicationForm.Summation = model.Summation;
                applicationForm.UserName = model.UserName;             
                applicationForm.UpdateDate = DateTime.Now;
                _ApplicationFormRepository.Update(applicationForm);
                return new OperationResult(OperationResultType.Success, "更新数据成功！");
            }
            catch(Exception ex)
            {
                _log.Error("更新申请表数据失败", ex);
                return new OperationResult(OperationResultType.Error, "更新数据失败!");
            }
        }

        public OperationResult Delete(List<string> serialNumber)
        {
            try
            {
                if (serialNumber != null)
                {
                    int count = _ApplicationFormRepository.Delete(_ApplicationFormRepository.Entities.Where(c => serialNumber.Contains(c.SerialNumber)));
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
            catch(Exception ex)
            {
                _log.Error("删除申请表数据失败", ex);
                return new OperationResult(OperationResultType.Error, "删除数据失败!");
            }
        }
        public OperationResult Update(ApplicationForm model)
        {
            try
            {
                model.SerialNumber = model.SerialNumber;
                model.ProjectNumber = model.ProjectNumber;
                model.TaskName = model.TaskName;
                model.RefundType = model.RefundType;
                model.ProjectDirector = model.ProjectDirector;
                model.DepartmentName = model.DepartmentName;
                model.Agent = model.Agent;
                model.PaymentType = model.PaymentType;
                model.AuditStatus = model.AuditStatus;
                model.SubmitTime = model.SubmitTime;
                model.AuditTime = model.AuditTime;
                model.AuditOpinion = model.AuditOpinion;
                model.ApplyDesp = model.ApplyDesp;
                model.Summation = model.Summation;
                model.UserName = model.UserName;             
                model.UpdateDate = DateTime.Now;
                _ApplicationFormRepository.Update(model);
                return new OperationResult(OperationResultType.Success, "更新申请单数据成功！");
            }
            catch(Exception ex)
            {
                _log.Error("更新申请表数据失败", ex);
                return new OperationResult(OperationResultType.Error, "更新申请单数据失败!");
            }
        }

        public OperationResult Delete(ApplicationForm model)
        {
            try
            {
                model.SerialNumber = model.SerialNumber;
                model.ProjectNumber = model.ProjectNumber;
                model.TaskName = model.TaskName;
                model.DepartmentName = model.DepartmentName;
                model.RefundType = model.RefundType;
                model.ProjectDirector = model.ProjectDirector;
                model.Agent = model.Agent;
                model.PaymentType = model.PaymentType;
                model.AuditStatus = model.AuditStatus;
                model.SubmitTime = model.SubmitTime;
                model.AuditTime = model.AuditTime;
                model.AuditOpinion = model.AuditOpinion;
                model.ApplyDesp = model.ApplyDesp;
                model.Summation = model.Summation;
                model.UserName = model.UserName;                          
                model.UpdateDate = DateTime.Now;
                _ApplicationFormRepository.Delete(model);
                return new OperationResult(OperationResultType.Success, "更新申请单数据成功！");
            }
            catch(Exception ex)
            {
                _log.Error("删除申请表数据失败", ex);
                return new OperationResult(OperationResultType.Error, "更新申请单数据失败!");
            }
        }

        public OperationResult Import(String fileName, Service.Excel.ColumnMap[] columns)
        {
            try
            {
                var items = ExcelService.GetObjects<ApplicationForm>(fileName, columns);
                _ApplicationFormRepository.InsertOrUpdate(items);
                return new OperationResult(OperationResultType.Success, "导入数据成功！");
            }
            catch (Exception ex)
            {
                _log.Error("导入申请表数据失败", ex);
                return new OperationResult(OperationResultType.Error, "导入数据失败!");
            }
        }

        public IList<ApplicationForm> GetApplicationFromByUser(String userName, int start, int limit, String search)
        {
            var applys = _ApplicationFormRepository.GetApplicationFromByUser(userName, start, limit,search);
            Dictionary<String, IList<Double>> totalTaxs = new Dictionary<string, IList<Double>>();
            _ApplicationFormRepository.GetSerNumberTotalTax(ref totalTaxs);
            for (var i = 0; i < applys.Count; i++)
            {
                var item = applys[i];
                if (totalTaxs.ContainsKey(item.SerialNumber))
                {
                    var taxs = totalTaxs[item.SerialNumber];
                    item.Tax = Math.Round(taxs[0], 2);
                    item.ServiceTax = Math.Round(taxs[1], 2);
                }
            }
            return applys;
        }

        public IList<ApplicationForm> GetApplicationFromByAdmin(int start, int limit, String search)
        {
            var applys = _ApplicationFormRepository.GetApplicationFromByAdmin(start,limit, search);
            
            Dictionary<String, IList<Double>> totalTaxs = new Dictionary<string, IList<Double>>();
            _ApplicationFormRepository.GetSerNumberTotalTax(ref totalTaxs);
            for (var i = 0; i < applys.Count; i++)
            {
                var item = applys[i];
                if(totalTaxs.ContainsKey(item.SerialNumber))
                {
                    var taxs = totalTaxs[item.SerialNumber];
                    item.Tax = Math.Round(taxs[0],2);
                    item.ServiceTax = Math.Round(taxs[1],2);
                }
            }
            return applys;
        }

    }
}
