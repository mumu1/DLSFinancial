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
    public class RefundTypeService : CoreServiceBase, IRefundTypeService
    {
        private readonly IRefundTypeRepository _RefundTypeRepository;

        public RefundTypeService(IRefundTypeRepository refundTypeRepository, IUnitOfWork unitOfWork)
            : base(unitOfWork)
        {
            this._RefundTypeRepository = refundTypeRepository;
        }
        public IQueryable<RefundType> RefundTypes
        {
            get { return _RefundTypeRepository.Entities; }
        }

        public OperationResult Insert(RefundTypeVM model)
        {
            try
            {
                RefundType refund = _RefundTypeRepository.Entities.FirstOrDefault(c => c.RefundTypeCode == model.RefundTypeCode.Trim());
                if (refund != null)
                {
                    return new OperationResult(OperationResultType.Warning, "数据库中已经存在相同的报销事项，请修改后重新提交！");
                }
                if (model.RefundTypeName == null || model.RefundTypeName.Trim() == "")
                    return new OperationResult(OperationResultType.Warning, "报销事项不能为空，请修改后重新提交！");
                var entity = new RefundType
                {
                    RefundTypeCode = model.RefundTypeCode,
                    RefundTypeName = model.RefundTypeName,
                    UpdateDate = DateTime.Now
                };
                _RefundTypeRepository.Insert(entity);
                return new OperationResult(OperationResultType.Success, "新增数据成功！");
            }
            catch(Exception ex)
            {
                return new OperationResult(OperationResultType.Error, "新增数据失败，数据库插入数据时发生了错误!");
            }
        }
        public OperationResult Update(RefundTypeVM model)
        {
            try
            {
                RefundType refund = _RefundTypeRepository.Entities.FirstOrDefault(c => c.RefundTypeCode == model.RefundTypeCode.Trim());
                if (refund == null)
                {
                    throw new Exception();
                }
                refund.RefundTypeName = model.RefundTypeName;
                refund.RefundTypeCode = model.RefundTypeCode;
                refund.UpdateDate = DateTime.Now;
                _RefundTypeRepository.Update(refund);
                return new OperationResult(OperationResultType.Success, "更新数据成功！");
            }
            catch
            {
                return new OperationResult(OperationResultType.Error, "更新数据失败!");
            }
        }

        public OperationResult Update(RefundType model)
        {
            try
            {
                model.UpdateDate = DateTime.Now;
                _RefundTypeRepository.Update(model);
                return new OperationResult(OperationResultType.Success, "更新数据成功！");
            }
            catch
            {
                return new OperationResult(OperationResultType.Error, "更新数据失败!");
            }
        }

        public OperationResult Delete(RefundType model)
        {
            try
            {
                model.UpdateDate = DateTime.Now;
                _RefundTypeRepository.Delete(model);
                return new OperationResult(OperationResultType.Success, "更新数据成功！");
            }
            catch
            {
                return new OperationResult(OperationResultType.Error, "更新数据失败!");
            }
        }

        public OperationResult Delete(List<string> refundTypeCode)
        {
            try
            {
                if (refundTypeCode != null)
                {
                    int count = _RefundTypeRepository.Delete(_RefundTypeRepository.Entities.Where(c => refundTypeCode.Contains(c.RefundTypeCode)));
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

        public OperationResult Import(String fileName, Service.Excel.ColumnMap[] columns)
        {
            try
            {
                var items = ExcelService.GetObjects<RefundType>(fileName, columns);
                _RefundTypeRepository.InsertOrUpdate(items);
                return new OperationResult(OperationResultType.Success, "导入数据成功！");
            }
            catch(Exception ex)
            {
                return new OperationResult(OperationResultType.Error, "导入数据失败!");
            }
        }
    }
}
