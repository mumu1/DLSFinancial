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
    public class DigsituationBeforeService : CoreServiceBase, IDigsituationBeforeService
    {
        private readonly IDigsituationBeforeRepository _DigsituationBeforeRepository;



        public DigsituationBeforeService(IDigsituationBeforeRepository digsituationBeforeRepository, IUnitOfWork unitOfWork)
            : base(unitOfWork)
        {
            this._DigsituationBeforeRepository = digsituationBeforeRepository;
        }
        public IQueryable<DigsituationBefore> DigsituationBefores
        {
            get { return _DigsituationBeforeRepository.Entities; }
        }

        public OperationResult Insert(DigsituationBeforeVM model)
        {
            try
            {
                DigsituationBefore oldPlot = _DigsituationBeforeRepository.Entities.FirstOrDefault(c => c.DigareaID == model.DigareaID.Trim());
                if (oldPlot != null)
                {
                    return new OperationResult(OperationResultType.Warning, "数据库中已经存在相同的以往发掘情况信息，请修改后重新提交！");
                }
                if (model.DigareaID == null || model.DigareaID.Trim() == "")
                    return new OperationResult(OperationResultType.Warning, "编号不能为空，请修改后重新提交！");

                var entity = new DigsituationBefore
                        {
                            DigareaID = model.DigareaID,
                            Digarea = model.Digarea,
                            Name = model.Name,
                            Distribution = model.Distribution,
                            Time =Convert.ToDateTime(model.Time),
                            UmrID = model.UmrID,
                            Remark = model.Remark,
                            UpdateDate = DateTime.Now
                        };

                _DigsituationBeforeRepository.Insert(entity);

                    return new OperationResult(OperationResultType.Success, "新增数据成功！");
                
            }
            catch
            {
                return new OperationResult(OperationResultType.Error, "新增数据失败，数据库插入数据时发生了错误!");
            }
        }


        public OperationResult Update(DigsituationBeforeVM model)
        {
            try
            {
                var user = DigsituationBefores.FirstOrDefault(c => c.DigareaID == model.DigareaID);
                if (user == null)
                {
                    throw new Exception();
                }
                user.DigareaID = model.DigareaID;
                user.Name = model.Name;
                user.Digarea = model.Digarea;
                user.Distribution = model.Distribution;              
                user.Time = Convert.ToDateTime(model.Time);              
                user.UmrID = model.UmrID;
                user.Remark = model.Remark;            
                user.UpdateDate = DateTime.Now;
                _DigsituationBeforeRepository.Update(user);
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
                    int count = _DigsituationBeforeRepository.Delete(_DigsituationBeforeRepository.Entities.Where(c => list.Contains(c.UmrID)));
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
        public OperationResult Delete(IEnumerable<DigsituationBeforeVM> list)
        {
            try
            {
                if (list != null)
                {
                    var pIds = list.Select(c => c.DigareaID).ToList();
                    int count = _DigsituationBeforeRepository.Delete(_DigsituationBeforeRepository.Entities.Where(c => pIds.Contains(c.DigareaID)));
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
