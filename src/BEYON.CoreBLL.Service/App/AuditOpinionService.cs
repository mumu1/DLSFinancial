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

namespace BEYON.CoreBLL.Service.App
{
    public class AuditOpinionService : CoreServiceBase, IAuditOpinionService
    {
        private readonly IAuditOpinionRepository _AuditOpinionRepository;



        public AuditOpinionService(IAuditOpinionRepository auditOpinionRepository, IUnitOfWork unitOfWork)
            : base(unitOfWork)
        {
            this._AuditOpinionRepository = auditOpinionRepository;
        }
        public IQueryable<AuditOpinion> AuditOpinions
        {
            get { return _AuditOpinionRepository.Entities; }
        }

        public OperationResult Insert(AuditOpinionVM model)
        {
            try
            {
                AuditOpinion auditOpinion = _AuditOpinionRepository.Entities.FirstOrDefault(c => c.AuditOpinionCode == model.AuditOpinionCode.Trim());
                if (auditOpinion != null)
                {
                    return new OperationResult(OperationResultType.Warning, "数据库中已经存在相同的审核意见，请修改后重新提交！");
                }
                if (model.AuditOpinionDesp == null || model.AuditOpinionDesp.Trim() == "")
                    return new OperationResult(OperationResultType.Warning, "审核意见描述不能为空，请修改后重新提交！");
                var entity = new AuditOpinion
                {
                    AuditOpinionCode = model.AuditOpinionCode,
                    AuditOpinionDesp = model.AuditOpinionDesp,
                    UpdateDate = DateTime.Now
                };
                _AuditOpinionRepository.Insert(entity);

                return new OperationResult(OperationResultType.Success, "新增数据成功！");
            }
            catch
            {
                return new OperationResult(OperationResultType.Error, "新增数据失败，数据库插入数据时发生了错误!");
            }
        }
        public OperationResult Update(AuditOpinionVM model)
        {
            try
            {
                AuditOpinion auditOpinion = _AuditOpinionRepository.Entities.FirstOrDefault(c => c.AuditOpinionCode == model.AuditOpinionCode.Trim());
                if (auditOpinion == null)
                {
                    throw new Exception();
                }
                auditOpinion.AuditOpinionDesp = model.AuditOpinionDesp;
                auditOpinion.AuditOpinionCode = model.AuditOpinionCode;
                auditOpinion.UpdateDate = DateTime.Now;
                _AuditOpinionRepository.Update(auditOpinion);
                return new OperationResult(OperationResultType.Success, "更新数据成功！");
            }
            catch
            {
                return new OperationResult(OperationResultType.Error, "更新数据失败!");
            }
        }

        public OperationResult Delete(List<string> auditOpinionCode)
        {
            try
            {
                if (auditOpinionCode != null)
                {
                    int count = _AuditOpinionRepository.Delete(_AuditOpinionRepository.Entities.Where(c => auditOpinionCode.Contains(c.AuditOpinionCode)));
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
        public OperationResult Update(AuditOpinion model)
        {
            try
            {
                model.UpdateDate = DateTime.Now;
                _AuditOpinionRepository.Update(model);
                return new OperationResult(OperationResultType.Success, "更新审核意见数据成功！");
            }
            catch
            {
                return new OperationResult(OperationResultType.Error, "更新审核意见数据失败!");
            }
        }

        public OperationResult Delete(AuditOpinion model)
        {
            try
            {
                model.UpdateDate = DateTime.Now;
                _AuditOpinionRepository.Delete(model);
                return new OperationResult(OperationResultType.Success, "更新审核意见数据成功！");
            }
            catch
            {
                return new OperationResult(OperationResultType.Error, "更新审核意见数据失败!");
            }
        }

        public OperationResult Import(String fileName, Service.Excel.ColumnMap[] columns)
        {
            try
            {
                var items = ExcelService.GetObjects<AuditOpinion>(fileName, columns);
                _AuditOpinionRepository.InsertOrUpdate(items);
                return new OperationResult(OperationResultType.Success, "导入数据成功！");
            }
            catch (Exception ex)
            {
                return new OperationResult(OperationResultType.Error, "导入数据失败!");
            }
        }
    }
}
