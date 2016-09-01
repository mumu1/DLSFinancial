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
    public class LiteratureService : CoreServiceBase, ILiteratureService
    {
        private readonly ILiteratureRepository _LiteratureRepository;



        public LiteratureService(ILiteratureRepository literatureRepository, IUnitOfWork unitOfWork)
            : base(unitOfWork)
        {
            this._LiteratureRepository = literatureRepository;
        }
        public IQueryable<Literature> Literatures
        {
            get { return _LiteratureRepository.Entities; }
        }

        public OperationResult Insert(LiteratureVM model)
        {
            try
            {
                Literature oldPlot = _LiteratureRepository.Entities.FirstOrDefault(c => c.LiteratureID == model.LiteratureID.Trim());
                if (oldPlot != null)
                {
                    return new OperationResult(OperationResultType.Warning, "数据库中已经存在相同的相关文献信息，请修改后重新提交！");
                }
                if (model.LiteratureID == null || model.LiteratureID.Trim() == "")
                    return new OperationResult(OperationResultType.Warning, "编号不能为空，请修改后重新提交！");
                var entity = new Literature
                        {
                            LiteratureID = model.LiteratureID,
                            Category = model.Category,
                            Name = model.Name,
                            Path = model.Path,                         
                            UmrID = model.UmrID,                          
                            Remark = model.Remark,
                            UpdateDate = DateTime.Now
                        };

                _LiteratureRepository.Insert(entity);

                    return new OperationResult(OperationResultType.Success, "新增数据成功！");
                
            }
            catch
            {
                return new OperationResult(OperationResultType.Error, "新增数据失败，数据库插入数据时发生了错误!");
            }
        }


        public OperationResult Update(LiteratureVM model)
        {
            try
            {
                var user = Literatures.FirstOrDefault(c => c.LiteratureID == model.LiteratureID);
                if (user == null)
                {
                    throw new Exception();
                }
                user.LiteratureID = model.LiteratureID;
                user.Name = model.Name;
                user.Category = model.Category;
                user.Path = model.Path;            
                user.UmrID = model.UmrID;
                user.Remark = model.Remark;              
                user.UpdateDate = DateTime.Now;
                _LiteratureRepository.Update(user);
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
                    int count = _LiteratureRepository.Delete(_LiteratureRepository.Entities.Where(c => list.Contains(c.UmrID)));
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
        public OperationResult Delete(IEnumerable<LiteratureVM> list)
        {
            try
            {
                if (list != null)
                {
                    var pIds = list.Select(c => c.LiteratureID).ToList();
                    int count = _LiteratureRepository.Delete(_LiteratureRepository.Entities.Where(c => pIds.Contains(c.LiteratureID)));
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
