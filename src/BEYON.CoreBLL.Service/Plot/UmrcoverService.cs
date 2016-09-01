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

namespace BEYON.CoreBLL.Service.Plot
{
    public class UmrcoverService : CoreServiceBase, IUmrcoverService
    {
         private readonly IUmrcoverRepository _UmrcoverRepository;



         public UmrcoverService(IUmrcoverRepository umrcoverRepository, IUnitOfWork unitOfWork)
            : base(unitOfWork)
        {
            this._UmrcoverRepository = umrcoverRepository;
        }
        public IQueryable<Umrcover> Umrcovers
        {
            get { return _UmrcoverRepository.Entities; }
        }

        public OperationResult Insert(UmrcoverVM model)
        {
            try
            {
                Umrcover oldPlot = _UmrcoverRepository.Entities.FirstOrDefault(c => c.UmrID == model.UmrID.Trim());
                if (oldPlot != null)
                {
                    return new OperationResult(OperationResultType.Warning, "数据库中已经存在相同的遗址编号，请修改后重新提交！");
                }
                if (model.UmrID == null || model.UmrID.Trim() == "")
                    return new OperationResult(OperationResultType.Warning, "编号不能为空，请修改后重新提交！");
                var entity = new Umrcover
                {
                    CollectDate=Convert.ToDateTime(model.CollectDate),
                    Collector=model.Collector,
                    AuditDate = model.AuditDate,
                    Auditor = model.Auditor,
                    CheckDate = model.CheckDate,
                    Checker = model.Checker,
                    City = model.City,
                    //CollectDate = dt,
                    //Collector = model.Collector,
                    Country = model.Country,
                    Name = model.Name,
                    Province = model.Province,
                    RegionCode = model.RegionCode,
                    //SearchType = Convert.ToInt32(model.SearchType),
                    SearchType = model.SearchType,
                    UmrID = model.UmrID,
                    UserID = model.UserID,
                    PlotBeforeID = model.MarkCode,
                    UpdateDate = DateTime.Now
                };
                _UmrcoverRepository.Insert(entity);

                return new OperationResult(OperationResultType.Success, "新增数据成功！");
            }
            catch
            {
                return new OperationResult(OperationResultType.Error, "新增数据失败，数据库插入数据时发生了错误!");
            }
        }
        public OperationResult Update(UmrcoverVM model)
        {
            try
            {
                var user = Umrcovers.FirstOrDefault(c => c.UmrID == model.UmrID);
                if (user == null)
                {
                    throw new Exception();
                }
                user.CollectDate = Convert.ToDateTime(model.CollectDate);
                user.Collector = model.Collector;
                user.AuditDate = model.AuditDate;
                user.Auditor = model.Auditor;
                user.CheckDate = model.CheckDate;
                user.City = model.City;
                user.Country = model.Country;
                user.Name = model.Name;
                user.Province = model.Province;
                user.RegionCode = model.RegionCode;
                user.UmrID = model.UmrID;
                //user.SearchType = Convert.ToInt32(model.SearchType);
                user.SearchType = model.SearchType;
                user.UserID = model.UserID;
                user.PlotBeforeID = model.MarkCode;              
                user.UpdateDate = DateTime.Now;
                _UmrcoverRepository.Update(user);
                return new OperationResult(OperationResultType.Success, "更新数据成功！");
            }
            catch
            {
                return new OperationResult(OperationResultType.Error, "更新数据失败!");
            }
        }

        public OperationResult Delete(List<string> umrIds)
        {
            try
            {
                if (umrIds != null)
                {
                    //var umrIds = list.Select(c => c.UmrID).ToList();
                    int count = _UmrcoverRepository.Delete(_UmrcoverRepository.Entities.Where(c => umrIds.Contains(c.UmrID)));
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
