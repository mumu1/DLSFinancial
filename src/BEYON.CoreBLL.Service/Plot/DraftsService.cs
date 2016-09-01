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
    public class DraftsService : CoreServiceBase, IDraftsService
    {
        private readonly IDraftsRepository _DraftsRepository;



        public DraftsService(IDraftsRepository draftsRepository, IUnitOfWork unitOfWork)
            : base(unitOfWork)
        {
            this._DraftsRepository = draftsRepository;
        }
        public IQueryable<Drafts> Draftss
        {
            get { return _DraftsRepository.Entities; }
        }

        public OperationResult Insert(DraftsVM model)
        {
            try
            {
                Drafts oldPlot = _DraftsRepository.Entities.FirstOrDefault(c => c.D_ID == model.D_ID.Trim());
                if (oldPlot != null)
                {
                    return new OperationResult(OperationResultType.Warning, "数据库中已经存在相同的图纸信息，请修改后重新提交！");
                }
                if (model.D_ID == null || model.D_ID.Trim() == "")
                    return new OperationResult(OperationResultType.Warning, "编号不能为空，请修改后重新提交！");
                var entity = new Drafts
                    {
                        D_ID = model.D_ID,
                        Counter = model.Counter,
                        Name = model.Name,
                        UserID = model.UserID,
                        DraftID = model.DraftID,
                        Scale = model.Scale,
                        Drawer = model.Drawer,
                        FilePath = model.FilePath,
                        Suffix = model.Suffix,
                        Time = Convert.ToDateTime(model.Time),
                        UmrID = model.UmrID,
                        //Annex = model.Annex,
                        Remark = model.Remark,
                        UpdateDate = DateTime.Now
                    };

                _DraftsRepository.Insert(entity);

                return new OperationResult(OperationResultType.Success, "新增数据成功！");

            }
            catch
            {
                return new OperationResult(OperationResultType.Error, "新增数据失败，数据库插入数据时发生了错误!");
            }
        }


        public OperationResult Update(DraftsVM model)
        {
            try
            {
                var user = Draftss.FirstOrDefault(c => c.D_ID == model.D_ID);
                if (user == null)
                {
                    throw new Exception();
                }
                user.Counter = model.Counter;
                user.Name = model.Name;
                user.UserID = model.UserID;
                user.DraftID = model.DraftID;
                user.Scale = model.Scale;
                user.Drawer = model.Drawer;
                user.FilePath = model.FilePath;
                user.Suffix = model.Suffix;
                user.Time = Convert.ToDateTime(model.Time);
                //user.Annex = model.Annex;
                user.UmrID = model.UmrID;
                user.Remark = model.Remark;
                user.D_ID = model.D_ID;
                user.UpdateDate = DateTime.Now;
                _DraftsRepository.Update(user);
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
                    int count = _DraftsRepository.Delete(_DraftsRepository.Entities.Where(c => list.Contains(c.UmrID)));
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
        public OperationResult Delete(IEnumerable<DraftsVM> list)
        {
            try
            {
                if (list != null)
                {
                    var pIds = list.Select(c => c.D_ID).ToList();
                    int count = _DraftsRepository.Delete(_DraftsRepository.Entities.Where(c => pIds.Contains(c.D_ID)));
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
