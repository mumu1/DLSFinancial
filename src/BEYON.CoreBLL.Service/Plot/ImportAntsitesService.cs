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
    public class ImportAntsitesService : CoreServiceBase, IImportAntsitesService
    {
        private readonly IImportAntsitesRepository _ImportAntsitesRepository;



        public ImportAntsitesService(IImportAntsitesRepository importAntsitesRepository, IUnitOfWork unitOfWork)
            : base(unitOfWork)
        {
            this._ImportAntsitesRepository = importAntsitesRepository;
        }
        public IQueryable<ImportAntsites> ImportAntsitess
        {
            get { return _ImportAntsitesRepository.Entities; }
        }

        public OperationResult Insert(ImportAntsitesVM model)
        {
            try
            {
                ImportAntsites oldPlot = _ImportAntsitesRepository.Entities.FirstOrDefault(c => c.SiteID == model.SiteID.Trim());
                if (oldPlot != null)
                {
                    return new OperationResult(OperationResultType.Warning, "数据库中已经存在相同的重要遗迹信息，请修改后重新提交！");
                }
                if (model.SiteID == null || model.SiteID.Trim() == "")
                    return new OperationResult(OperationResultType.Warning, "编号不能为空，请修改后重新提交！");
                var entity = new ImportAntsites
                        {
                            SiteID = model.SiteID,
                            Path = model.Path,
                            Name = model.Name,                       
                            UmrID = model.UmrID,                           
                            Remark = model.Remark,
                            UpdateDate = DateTime.Now
                        };

                _ImportAntsitesRepository.Insert(entity);

                    return new OperationResult(OperationResultType.Success, "新增数据成功！");
                
            }
            catch
            {
                return new OperationResult(OperationResultType.Error, "新增数据失败，数据库插入数据时发生了错误!");
            }
        }


        public OperationResult Update(ImportAntsitesVM model)
        {
            try
            {
                var user = ImportAntsitess.FirstOrDefault(c => c.SiteID == model.SiteID);
                if (user == null)
                {
                    throw new Exception();
                }
                user.SiteID = model.SiteID;
                user.Name = model.Name;
                user.Path = model.Path;           
                user.UmrID = model.UmrID;
                user.Remark = model.Remark;
                user.UpdateDate = DateTime.Now;
                _ImportAntsitesRepository.Update(user);
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
                    int count = _ImportAntsitesRepository.Delete(_ImportAntsitesRepository.Entities.Where(c => list.Contains(c.UmrID)));
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
        public OperationResult Delete(IEnumerable<ImportAntsitesVM> list)
        {
            try
            {
                if (list != null)
                {
                    var pIds = list.Select(c => c.SiteID).ToList();
                    int count = _ImportAntsitesRepository.Delete(_ImportAntsitesRepository.Entities.Where(c => pIds.Contains(c.SiteID)));
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
