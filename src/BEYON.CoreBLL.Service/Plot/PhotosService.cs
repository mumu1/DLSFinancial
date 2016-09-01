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
    public class PhotosService : CoreServiceBase, IPhotosService
    {
        private readonly IPhotosRepository _PhotosRepository;



        public PhotosService(IPhotosRepository photosRepository, IUnitOfWork unitOfWork)
            : base(unitOfWork)
        {
            this._PhotosRepository = photosRepository;
        }
        public IQueryable<Photos> Photoss
        {
            get { return _PhotosRepository.Entities; }
        }

        public OperationResult Insert(PhotosVM model)
        {
            try
            {
                Photos oldPlot = _PhotosRepository.Entities.FirstOrDefault(c => c.P_ID == model.P_ID.Trim());
                if (oldPlot != null)
                {
                    return new OperationResult(OperationResultType.Warning, "数据库中已经存在相同的照片编号，请修改后重新提交！");
                }
                if (model.P_ID == null || model.P_ID.Trim() == "")
                    return new OperationResult(OperationResultType.Warning, "编号不能为空，请修改后重新提交！");
                var entity = new Photos
                    {

                        Cameraman = model.Cameraman,
                        Counter = model.Counter,
                        FilePath = model.FilePath,
                        Name = model.Name,
                        NegativeID = model.NegativeID,
                        Orientation = model.Orientation,
                        P_ID = model.P_ID,
                        PhotoID = model.PhotoID,
                        Remark = model.Remark,
                        Suffix = model.Suffix,
                        Time =Convert.ToDateTime(model.Time),
                        UmrID = model.UmrID,
                        UserID = model.UserID,
                        //Annex = model.Annex
                        UpdateDate = DateTime.Now
                    };

                _PhotosRepository.Insert(entity);

                return new OperationResult(OperationResultType.Success, "新增数据成功！");
            }
            catch
            {
                return new OperationResult(OperationResultType.Error, "新增数据失败，数据库插入数据时发生了错误!");
            }
        }
        public OperationResult Update(PhotosVM model)
        {
            try
            {
                var user = Photoss.FirstOrDefault(c => c.P_ID == model.P_ID);
                if (user == null)
                {
                    throw new Exception();
                }
                user.Counter = model.Counter;
                user.Name = model.Name;
                user.UserID = model.UserID;
                user.NegativeID = model.NegativeID;
                user.Orientation = model.Orientation;
                user.Cameraman = model.Cameraman;
                user.FilePath = model.FilePath;
                user.Suffix = model.Suffix;
                user.Time = Convert.ToDateTime(model.Time);
                //user.Annex = model.Annex;
                user.UmrID = model.UmrID;
                user.Remark = model.Remark;
                user.PhotoID = model.PhotoID;
                user.P_ID = model.P_ID;
                user.UpdateDate = DateTime.Now;
                _PhotosRepository.Update(user);
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
                    int count = _PhotosRepository.Delete(_PhotosRepository.Entities.Where(c => list.Contains(c.UmrID)));
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
        public OperationResult Delete(IEnumerable<PhotosVM> list)
        {
            try
            {
                if (list != null)
                {
                    var pIds = list.Select(c => c.P_ID).ToList();
                    int count = _PhotosRepository.Delete(_PhotosRepository.Entities.Where(c => pIds.Contains(c.P_ID)));
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
