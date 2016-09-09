using System;
using System.Collections.Generic;
using System.Linq;
using BEYON.Component.Data.EF.Interface;
using BEYON.Component.Tools;
using BEYON.CoreBLL.Service.App.Interface;
using BEYON.Domain.Data.Repositories.App;
using BEYON.Domain.Model.App;
using BEYON.ViewModel.App;
using BEYON.CoreBLL.Service.Excel;
using EntityFramework.Extensions;

namespace BEYON.CoreBLL.Service.App
{
    public class TitleService : CoreServiceBase, ITitleService
    {
        private readonly ITitleRepository _TitleRepository;



        public TitleService(ITitleRepository titleRepository, IUnitOfWork unitOfWork)
            : base(unitOfWork)
        {
            this._TitleRepository = titleRepository;
        }
        public IQueryable<Title> Titles
        {
            get { return _TitleRepository.Entities; }
        }

        public OperationResult Insert(TitleVM model)
        {
            try
            {
                Title title = _TitleRepository.Entities.FirstOrDefault(c => c.TitleCode == model.TitleCode.Trim());
                if (title != null)
                {
                    return new OperationResult(OperationResultType.Warning, "数据库中已经存在相同的报销事项，请修改后重新提交！");
                }
                if (model.TitleName == null || model.TitleName.Trim() == "")
                    return new OperationResult(OperationResultType.Warning, "职称名称不能为空，请修改后重新提交！");
                var entity = new Title
                {
                    TitleCode = model.TitleCode,
                    TitleName = model.TitleName,
                    UpdateDate = DateTime.Now
                };
                _TitleRepository.Insert(entity);

                return new OperationResult(OperationResultType.Success, "新增数据成功！");
            }
            catch
            {
                return new OperationResult(OperationResultType.Error, "新增数据失败，数据库插入数据时发生了错误!");
            }
        }
        public OperationResult Update(TitleVM model)
        {
            try
            {
                Title title = _TitleRepository.Entities.FirstOrDefault(c => c.TitleCode == model.TitleCode.Trim());
                if (title == null)
                {
                    throw new Exception();
                }
                title.TitleName = model.TitleName;
                title.TitleCode = model.TitleCode;
                title.UpdateDate = DateTime.Now;
                _TitleRepository.Update(title);
                return new OperationResult(OperationResultType.Success, "更新数据成功！");
            }
            catch
            {
                return new OperationResult(OperationResultType.Error, "更新数据失败!");
            }
        }

        public OperationResult Delete(List<string> titleCode)
        {
            try
            {
                if (titleCode != null)
                {
                    int count = _TitleRepository.Delete(_TitleRepository.Entities.Where(c => titleCode.Contains(c.TitleCode)));
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
        public OperationResult Update(Title model)
        {
            try
            {
                model.UpdateDate = DateTime.Now;
                _TitleRepository.Update(model);
                return new OperationResult(OperationResultType.Success, "更新职称数据成功！");
            }
            catch
            {
                return new OperationResult(OperationResultType.Error, "更新职称数据失败!");
            }
        }

        public OperationResult Delete(Title model)
        {
            try
            {
                model.UpdateDate = DateTime.Now;
                _TitleRepository.Delete(model);
                return new OperationResult(OperationResultType.Success, "更新职称数据成功！");
            }
            catch
            {
                return new OperationResult(OperationResultType.Error, "更新职称数据失败!");
            }
        }

        public OperationResult Import(String fileName, Service.Excel.ColumnMap[] columns)
        {
            try
            {
                var items = ExcelService.GetObjects<Title>(fileName, columns);
                _TitleRepository.InsertOrUpdate(items);
                return new OperationResult(OperationResultType.Success, "导入数据成功！");
            }
            catch (Exception ex)
            {
                return new OperationResult(OperationResultType.Error, "导入数据失败!");
            }
        }
    }
}
