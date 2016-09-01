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
    public class BasicPropertyService : CoreServiceBase, IBasicPropertyService
    {
         private readonly IBasicPropertyRepository _BasicPropertyRepository;



         public BasicPropertyService(IBasicPropertyRepository basicPropertyRepository, IUnitOfWork unitOfWork)
            : base(unitOfWork)
        {
            this._BasicPropertyRepository = basicPropertyRepository;
        }
         public IQueryable<BasicProperty> BasicPropertys
        {
            get { return _BasicPropertyRepository.Entities; }
        }

         public OperationResult Insert(BasicPropertyVM model)
        {
            try
            {
                BasicProperty oldPlot = _BasicPropertyRepository.Entities.FirstOrDefault(c => c.UmrID == model.UmrID.Trim());
                if (oldPlot != null)
                {
                    return new OperationResult(OperationResultType.Warning, "数据库中已经存在相同的遗址信息，请修改后重新提交！");
                }
                if (model.UmrID == null || model.UmrID.Trim() == "")
                    return new OperationResult(OperationResultType.Warning, "遗址编号不能为空，请修改后重新提交！");

                if (model.Latitude2 == null)//点
                {
                    var entity = new BasicProperty
                    {
                        Code = model.Code,
                        Address = model.Address,
                        Altitude = model.Altitude,
                        PointDescription = model.PointDescription,
                        Rank = model.Rank,
                        SpreadArea = model.SpreadArea,
                        BuildingTakeoffArea = model.BuildingTakeoffArea,
                        AvoidBuildingArea = model.AvoidBuildingArea,
                        Year = model.Year,
                        YearForCount = model.YearForCount,
                        UmrID = model.UmrID,
                        Category = model.Category,
                        Ownership = model.Ownership,
                        Owner = model.Owner,
                        Vestin = model.Vestin,
                        Purpose = model.Purpose,
                        SingleRelicNumber = model.SingleRelicNumber,
                        SingleRelicDescription = model.SingleRelicDescription,
                        Brief = model.Brief,
                        StateEvaluation = model.StateEvaluation,
                        StateDescription = model.StateDescription,
                        NaturalFactor = model.NaturalFactor,
                        ManualFactor = model.ManualFactor,
                        DestroyReason = model.DestroyReason,
                        Soceity = model.Soceity,
                        TeamSuggestion = model.TeamSuggestion,
                        AuditSight = model.AuditSight,
                        CheckResult = model.CheckResult,
                        Remark = model.Remark,
                        Coordinate = model.Latitude + ";" + model.Longitude,
                         Academic=model.Academic,
                          CulturalStage=model.CulturalStage,
                        UpdateDate = DateTime.Now
                    };
                    _BasicPropertyRepository.Insert(entity);
                }
                else //矩形
                {
                    var entity = new BasicProperty
                    {
                        Code = model.Code,
                        Address = model.Address,
                        Altitude = model.Altitude,
                        PointDescription = model.PointDescription,
                        Rank = model.Rank,
                        SpreadArea = model.SpreadArea,
                        BuildingTakeoffArea = model.BuildingTakeoffArea,
                        AvoidBuildingArea = model.AvoidBuildingArea,
                        Year = model.Year,
                        YearForCount = model.YearForCount,
                        UmrID = model.UmrID,
                        Category = model.Category,
                        Ownership = model.Ownership,
                        Owner = model.Owner,
                        Vestin = model.Vestin,
                        Purpose = model.Purpose,
                        SingleRelicNumber = model.SingleRelicNumber,
                        SingleRelicDescription = model.SingleRelicDescription,
                        Brief = model.Brief,
                        StateEvaluation = model.StateEvaluation,
                        StateDescription = model.StateDescription,
                        NaturalFactor = model.NaturalFactor,
                        ManualFactor = model.ManualFactor,
                        DestroyReason = model.DestroyReason,
                        Soceity = model.Soceity,
                        TeamSuggestion = model.TeamSuggestion,
                        AuditSight = model.AuditSight,
                        CheckResult = model.CheckResult,
                        Remark = model.Remark,
                        Coordinate = model.Latitude + ";" + model.Longitude + ":" + model.Latitude2 + "|" + model.Longitude2,
                        Academic = model.Academic,
                        CulturalStage = model.CulturalStage,
                        UpdateDate = DateTime.Now
                    };
                    _BasicPropertyRepository.Insert(entity);
                }

                return new OperationResult(OperationResultType.Success, "新增数据成功！");
            }
            catch
            {
                return new OperationResult(OperationResultType.Error, "新增数据失败，数据库插入数据时发生了错误!");
            }
        }
         public OperationResult Update(BasicPropertyVM model)
         {
             try
             {
                 var user = BasicPropertys.FirstOrDefault(c => c.UmrID == model.UmrID);
                 if (user == null)
                 {
                     throw new Exception();
                 }
                 user.Code = model.Code;
                 user.Address = model.Address;
                 user.Altitude = model.Altitude;
                 user.PointDescription = model.PointDescription;
                 user.Rank = model.Rank;
                 user.SpreadArea = model.SpreadArea;
                 user.BuildingTakeoffArea = model.BuildingTakeoffArea;
                 user.AvoidBuildingArea = model.AvoidBuildingArea;
                 user.Year = model.Year;
                 user.YearForCount = model.YearForCount;
                 user.UmrID = model.UmrID;
                 user.Category = model.Category;
                 user.Ownership = model.Ownership;
                 user.Owner = model.Owner;
                 user.Vestin = model.Vestin;
                 user.Purpose = model.Purpose;
                 user.SingleRelicNumber = model.SingleRelicNumber;
                 user.SingleRelicDescription = model.SingleRelicDescription;
                 user.Brief = model.Brief;
                 user.StateEvaluation = model.StateEvaluation;
                 user.StateDescription = model.StateDescription;
                 user.NaturalFactor = model.NaturalFactor;
                 user.ManualFactor = model.ManualFactor;
                 user.DestroyReason = model.DestroyReason;
                 user.Soceity = model.Soceity;
                 user.TeamSuggestion = model.TeamSuggestion;
                 user.AuditSight = model.AuditSight;
                 user.CheckResult = model.CheckResult;
                 user.Remark = model.Remark;
                 user.CulturalStage = model.CulturalStage;
                 user.Academic = model.Academic;
                 if (model.Latitude2==null)
                 {
                     user.Coordinate = model.Latitude + ";" + model.Longitude;

                 }
                 else
                 {
                     user.Coordinate = model.Latitude + ";" + model.Longitude + ":" + model.Latitude2 + "|" + model.Longitude2;

                 }
                 user.UpdateDate = DateTime.Now;
                 _BasicPropertyRepository.Update(user);
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
                     int count = _BasicPropertyRepository.Delete(_BasicPropertyRepository.Entities.Where(c => list.Contains(c.UmrID)));
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
         public OperationResult Delete(IEnumerable<BasicPropertyVM> list)
        {
            try
            {
                if (list != null)
                {
                    var basicPropertyIds = list.Select(c => c.UmrID).ToList();
                    int count = _BasicPropertyRepository.Delete(_BasicPropertyRepository.Entities.Where(c => basicPropertyIds.Contains(c.UmrID)));
                    //int count = _BasicPropertyRepository.Entities.Where(c => basicPropertyIds.Contains(c.UmrID)).Delete();
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
