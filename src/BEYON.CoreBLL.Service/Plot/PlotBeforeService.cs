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
    public class PlotBeforeService : CoreServiceBase, IPlotBeforeService
    {
        private readonly IPlotBeforeRepository _PlotBeforeRepository;

        public PlotBeforeService(IPlotBeforeRepository plotBeforeRepository, IUnitOfWork unitOfWork)
            : base(unitOfWork)
        {
            this._PlotBeforeRepository = plotBeforeRepository;
        }
        public IQueryable<PlotBefore> PlotBefores
        {
            get { return _PlotBeforeRepository.Entities; }
        }

        public OperationResult Insert(PlotBeforeVM model)
        {
            try
            {
                PlotBefore oldPlot = _PlotBeforeRepository.Entities.FirstOrDefault(c => c.MarkCode == model.MarkCode.Trim());
                if (oldPlot != null)
                {
                    return new OperationResult(OperationResultType.Warning, "数据库中已经存在相同的标绘信息，请修改后重新提交！");
                }
                if (model.MarkCode == null || model.MarkCode.Trim() == "")
                    return new OperationResult(OperationResultType.Warning, "编号不能为空，请修改后重新提交！");
                if (model.Latitude2 == null)
                {
                    var entity = new PlotBefore
                    {
                        MarkCode = model.MarkCode.Trim(),
                        PlotName = model.PlotName.Trim(),
                        MarkPerson = model.MarkPerson,
                        MarkPersonId = model.MarkPersonId,
                        MarkTime = Convert.ToDateTime(model.MarkTime),
                        Coordinate = model.Latitude + ";" + model.Longitude,

                        PlotStatus = model.PlotStatus,

                        //UpdateDate = DateTime.Now
                    };

                    _PlotBeforeRepository.Insert(entity);
                }
                else
                {
                    var entity = new PlotBefore
                    {
                        MarkCode = model.MarkCode.Trim(),
                        PlotName = model.PlotName.Trim(),
                        MarkPerson = model.MarkPerson,
                        MarkPersonId = model.MarkPersonId,
                        MarkTime = Convert.ToDateTime(model.MarkTime),
                        Coordinate = model.Latitude + ";" + model.Longitude+":"+model.Latitude2+"|"+model.Longitude2,
                        PlotStatus = model.PlotStatus,
                        //UpdateDate = DateTime.Now
                    };

                    _PlotBeforeRepository.Insert(entity);
                }

                return new OperationResult(OperationResultType.Success, "新增数据成功！");
            }
            catch
            {
                return new OperationResult(OperationResultType.Error, "新增数据失败，数据库插入数据时发生了错误!");
            }
        }
        public OperationResult Update(PlotBeforeVM model)
        {
            try
            {
                //Todo 实现功能

                return new OperationResult(OperationResultType.Success, "更新数据成功！");
            }
            catch
            {
                return new OperationResult(OperationResultType.Error, "更新数据失败!");
            }
        }

        public OperationResult Delete(IEnumerable<PlotBeforeVM> list)
        {
            try
            {
                if (list != null)
                {
                    var markIds = list.Select(c => c.MarkCode).ToList();
                    int count = _PlotBeforeRepository.Delete(_PlotBeforeRepository.Entities.Where(c => markIds.Contains(c.MarkCode)));
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