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
    public class PointsService : CoreServiceBase, IPointsService
    {
         private readonly IPointsRepository _PointsRepository;



         public PointsService(IPointsRepository pointsRepository, IUnitOfWork unitOfWork)
            : base(unitOfWork)
        {
            this._PointsRepository = pointsRepository;
        }
         public IQueryable<Points> Pointss
        {
            get { return _PointsRepository.Entities; }
        }

         public OperationResult Insert(PointsVM model)
        {
            try
            {
                Points oldPlot = _PointsRepository.Entities.FirstOrDefault(c => c.PointID == model.PointID.Trim());
                if (oldPlot != null)
                {
                    return new OperationResult(OperationResultType.Warning, "数据库中已经存在相同的测点信息，请修改后重新提交！");
                }
                if (model.PointID == null || model.PointID.Trim() == "")
                    return new OperationResult(OperationResultType.Warning, "编号不能为空，请修改后重新提交！");
                var entity = new Points
                {
                    PointID = model.PointID,
                    Counter = model.Counter,
                    UserID = model.UserID,
                    Longitude = model.Longitude,
                    Latitude = model.Latitude,
                    Altitude = model.Altitude,
                    PointDescription = model.PointDescription,
                    Remark = model.Remark,
                    Coordinate = model.Longitude + ";" + model.Latitude,  
                    UmrID=model.UmrID,
                    
                    UpdateDate = DateTime.Now
                };
                _PointsRepository.Insert(entity);

                return new OperationResult(OperationResultType.Success, "新增数据成功！");
            }
            catch
            {
                return new OperationResult(OperationResultType.Error, "新增数据失败，数据库插入数据时发生了错误!");
            }
        }
         public OperationResult Update(PointsVM model)
        {
            try
            {
                var user = Pointss.FirstOrDefault(c => c.PointID == model.PointID);
                if (user == null)
                {
                    throw new Exception();
                }
                user.Counter = model.Counter;
                user.PointID = model.PointID;
                user.UserID = model.UserID;
                user.Longitude = model.Longitude;
                user.Latitude = model.Latitude;
                user.Altitude = model.Altitude;
                user.PointDescription = model.PointDescription;       
                user.UmrID = model.UmrID;
                user.Remark = model.Remark;
                user.Coordinate = model.Longitude+";"+model.Latitude;
                user.UpdateDate = DateTime.Now;
                _PointsRepository.Update(user);
                return new OperationResult(OperationResultType.Success, "更新数据成功！");
            }
            catch
            {
                return new OperationResult(OperationResultType.Error, "更新数据失败!");
            }
        }

         public OperationResult Delete(IEnumerable<PointsVM> list)
        {
            try
            {
                if (list != null)
                {
                    var pids = list.Select(c => c.PointID).ToList();
                    int count = _PointsRepository.Delete(_PointsRepository.Entities.Where(c => pids.Contains(c.PointID)));
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
         public OperationResult UMrIDDelete(List<string> list)
         {
             try
             {
                 if (list != null)
                 {

                     int count = _PointsRepository.Delete(_PointsRepository.Entities.Where(c => list.Contains(c.UmrID)));
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
