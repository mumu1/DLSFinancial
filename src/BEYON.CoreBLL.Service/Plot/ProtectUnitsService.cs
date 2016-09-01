using System.Collections.Generic;
using System.Linq;
using EntityFramework.Extensions;
using BEYON.Component.Data.EF.Interface;
using BEYON.Component.Tools;
using BEYON.CoreBLL.Service.Plot.Interface;
using BEYON.Domain.Data.Repositories.Plot;
using BEYON.Domain.Model.Plot;
using BEYON.ViewModel.Plot;
using System;

namespace BEYON.CoreBLL.Service.Plot
{
    public class ProtectUnitsService : CoreServiceBase, IProtectUnitsService
    {
        private readonly IProtectUnitsRepository _ProtectUnitsRepository;

        public ProtectUnitsService(IProtectUnitsRepository protectUnitsRepository, IUnitOfWork unitOfWork)
            : base(unitOfWork)
        {
            this._ProtectUnitsRepository = protectUnitsRepository;
        }
        public IQueryable<ProtectUnits> ProtectUnitss
        {
            get { return _ProtectUnitsRepository.Entities; }
        }

        public OperationResult Insert(ProtectUnitsVM model)
        {
            try
            {
                ProtectUnits oldPlot = _ProtectUnitsRepository.Entities.FirstOrDefault(c => c.ProtectunitID == model.ProtectunitID.Trim());
                if (oldPlot != null)
                {
                    return new OperationResult(OperationResultType.Warning, "数据库中已经存在相同的文物保护单位信息，请修改后重新提交！");
                }

                if (model.ProtectunitID == null || model.ProtectunitID.Trim() == "")
                    return new OperationResult(OperationResultType.Warning, "编号不能为空，请修改后重新提交！");
                var entity = new ProtectUnits
                {
                    UserID = model.UseId,
                    ProtectunitID = model.ProtectunitID,
                    Batch = model.Batch,
                    Name = model.Name,
                    Place = model.Place,
                    Year = model.Year,
                    Category = model.Category,
                    Remark = model.Remark,
                    UpdateDate = DateTime.Now
                };

                _ProtectUnitsRepository.Insert(entity);

                return new OperationResult(OperationResultType.Success, "新增数据成功！");
            }
            catch
            {
                return new OperationResult(OperationResultType.Error, "新增数据失败，数据库插入数据时发生了错误!");
            }
        }
        public OperationResult Update(ProtectUnitsVM model)
        {
            try
            {
                var user = ProtectUnitss.FirstOrDefault(c => c.ProtectunitID == model.ProtectunitID);
                if (user == null)
                {
                    throw new Exception();
                }
                user.ProtectunitID = model.ProtectunitID;
                user.Name = model.Name;
                user.Batch = model.Batch;
                user.Place = model.Place;
                user.Year = model.Year;
                user.Category = model.Category;            
                user.Remark = model.Remark;               
                user.UpdateDate = DateTime.Now;
                _ProtectUnitsRepository.Update(user);
                return new OperationResult(OperationResultType.Success, "更新数据成功！");
            }
            catch
            {
                return new OperationResult(OperationResultType.Error, "更新数据失败!");
            }
        }
        public OperationResult UMrIDDelete(List<string> list)
        {
            try
            {
                if (list != null)
                {
                    //var sampleIds = list.Select(c => c.SampleID).ToList();
                    int count = _ProtectUnitsRepository.Delete(_ProtectUnitsRepository.Entities.Where(c => list.Contains(c.ProtectunitID)));
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
        public OperationResult Delete(IEnumerable<ProtectUnitsVM> list)
        {
            try
            {
                if (list != null)
                {
                    var markIds = list.Select(c => c.ProtectunitID).ToList();
                    int count = _ProtectUnitsRepository.Delete(_ProtectUnitsRepository.Entities.Where(c => markIds.Contains(c.ProtectunitID)));
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