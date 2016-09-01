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
using System.Web;

namespace BEYON.CoreBLL.Service.Plot
{
    public class LayerDepositService : CoreServiceBase, ILayerDepositService
    {
        private readonly ILayerDepositRepository _LayerDepositRepository;



        public LayerDepositService(ILayerDepositRepository layerDepositRepository, IUnitOfWork unitOfWork)
            : base(unitOfWork)
        {
            this._LayerDepositRepository = layerDepositRepository;
        }
        public IQueryable<LayerDeposit> LayerDeposits
        {
            get { return _LayerDepositRepository.Entities; }
        }

        public OperationResult Insert(LayerDepositVM model)
        {
            try
            {
                LayerDeposit oldPlot = _LayerDepositRepository.Entities.FirstOrDefault(c => c.UmrID == model.UmrID.Trim());
                if (oldPlot != null)
                {
                    return new OperationResult(OperationResultType.Warning, "数据库中已经存在相同的地层堆积情况信息，请修改后重新提交！");
                }
                
                var entity = new LayerDeposit
                        {
                            Shape = model.Shape,
                            Density = model.Density,
                            Earthcolor = model.Earthcolor,
                            Soil = model.Soil,
                            Inclusions = model.Inclusions,
                            Savesituation = model.Savesituation,
                            Clearway = model.Clearway,
                            Properties = model.Properties,
                            Other = model.Other,
                            UmrID = model.UmrID,
                            Remark = model.Remark,
                            UpdateDate = DateTime.Now
                        };

                _LayerDepositRepository.Insert(entity);

                    return new OperationResult(OperationResultType.Success, "新增数据成功！");
                
            }
            catch
            {
                return new OperationResult(OperationResultType.Error, "新增数据失败，数据库插入数据时发生了错误!");
            }
        }


        public OperationResult Update(LayerDepositVM model)
        {
            try
            {
                var user = LayerDeposits.FirstOrDefault(c => c.UmrID == model.UmrID);
                if (user == null)
                {
                    throw new Exception();
                }
                user.Shape = model.Shape;
                user.Density = model.Density;
                user.Earthcolor = model.Earthcolor;
                user.Soil = model.Soil;
                user.Inclusions = model.Inclusions;
                user.Savesituation = model.Savesituation;
                user.Clearway = model.Clearway;
                user.Properties = model.Properties;               
                user.Other = model.Other;
                user.UmrID = model.UmrID;
                user.Remark = model.Remark;         
                user.UpdateDate = DateTime.Now;
                _LayerDepositRepository.Update(user);
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
                    int count = _LayerDepositRepository.Delete(_LayerDepositRepository.Entities.Where(c => list.Contains(c.UmrID)));
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
     
        public OperationResult Delete(List<string> umrIds)
        {
            try
            {
                if (umrIds != null)
                {
                    //var umrIds = list.Select(c => c.UmrID).ToList();
                    int count = _LayerDepositRepository.Delete(_LayerDepositRepository.Entities.Where(c => umrIds.Contains(c.UmrID)));
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
