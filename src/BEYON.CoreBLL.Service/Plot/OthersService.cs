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
    public class OthersService : CoreServiceBase, IOthersService
    {
        private readonly IOthersRepository _OthersRepository;



        public OthersService(IOthersRepository othersRepository, IUnitOfWork unitOfWork)
            : base(unitOfWork)
        {
            this._OthersRepository = othersRepository;
        }
        public IQueryable<Others> Otherss
        {
            get { return _OthersRepository.Entities; }
        }

        public OperationResult Insert(OthersVM model)
        {
            try
            {
                Others oldPlot = _OthersRepository.Entities.FirstOrDefault(c => c.OtherID == model.OtherID.Trim());
                if (oldPlot != null)
                {
                    return new OperationResult(OperationResultType.Warning, "数据库中已经存在相同的资料信息，请修改后重新提交！");
                }
                if (model.OtherID == null || model.OtherID.Trim() == "")
                    return new OperationResult(OperationResultType.Warning, "编号不能为空，请修改后重新提交！");
                var entity = new Others
                    {
                        OtherID = model.OtherID,
                        Counter = model.Counter,
                        Name = model.Name,
                        UserID = model.UserID,
                        Category = model.Category,
                        Number = model.Number,
                        SavePlace = model.SavePlace,
                        FilePath = model.FilePath,
                        Remark = model.Remark,
                        Suffix = model.Suffix,
                        //Annex = model.Annex,                        
                        UmrID = model.UmrID,                       
                        UpdateDate = DateTime.Now
                    };

                _OthersRepository.Insert(entity);
                
                return new OperationResult(OperationResultType.Success, "新增数据成功！");
            }
            catch
            {
                return new OperationResult(OperationResultType.Error, "新增数据失败，数据库插入数据时发生了错误!");
            }
        }
        public OperationResult Update(OthersVM model)
        {
            try
            {
                var user = Otherss.FirstOrDefault(c => c.OtherID == model.OtherID);
                if (user == null)
                {
                    throw new Exception();
                }
                user.Counter = model.Counter;
                user.Name = model.Name;
                user.UserID = model.UserID;
                user.FilePath = model.FilePath;
                user.Suffix = model.Suffix;
                //user.Annex = model.Annex;
                user.UmrID = model.UmrID;
                user.Remark = model.Remark;
                user.SavePlace = model.SavePlace;
                user.Number = model.Number;
                
                user.UpdateDate = DateTime.Now;
                _OthersRepository.Update(user);
                return new OperationResult(OperationResultType.Success, "更新数据成功！");

                //return new OperationResult(OperationResultType.Success, "更新数据成功！");
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
                    int count = _OthersRepository.Delete(_OthersRepository.Entities.Where(c => list.Contains(c.UmrID)));
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
        public OperationResult Delete(IEnumerable<OthersVM> list)
        {
            try
            {
                if (list != null)
                {
                    var pIds = list.Select(c => c.OtherID).ToList();
                    int count = _OthersRepository.Delete(_OthersRepository.Entities.Where(c => pIds.Contains(c.OtherID)));
                   
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
