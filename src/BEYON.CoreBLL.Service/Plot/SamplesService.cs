using BEYON.Component.Data.EF.Interface;
using BEYON.Component.Tools;
using BEYON.CoreBLL.Service.Plot.Interface;
using BEYON.Domain.Data.Repositories.Plot;
using BEYON.Domain.Model.Plot;
using BEYON.ViewModel.Plot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntityFramework.Extensions;
namespace BEYON.CoreBLL.Service.Plot
{
   public class SamplesService:CoreServiceBase ,ISamplesService
    {
       private readonly ISamplesRepository _sampleRepository;

       public SamplesService(ISamplesRepository sampleRepository,IUnitOfWork unitOfWork)
           :base(unitOfWork)
       {
           this._sampleRepository = sampleRepository;
       }
       public IQueryable<Samples> Sampless
       {
           get { return _sampleRepository.Entities; }
       }
       public OperationResult Insert(SamplesVM model)
       {
           try
           {
               Samples oldPlot = _sampleRepository.Entities.FirstOrDefault(c => c.SampleID== model.SampleID.Trim());
               if (oldPlot != null)
               {
                   return new OperationResult(OperationResultType.Warning, "数据库中已经存在相同的文物编号，请修改后重新提交！");
               }
               if(model.SampleID==null || model.SampleID.Trim()=="")
               {
                   return new OperationResult(OperationResultType.Warning, "文物编号不能为空");
               }
               var entity = new Samples
               {
                   Counter = model.Counter,
                   Material = model.Material,
                   Name = model.Name,
                   Remark = model.Remark,
                   SampleID = model.SampleID,
                   SavePlace = model.SavePlace,
                   UmrID = model.UmrID,
                   UserID = model.UserID,
                   Year = model.Year,
                   UpdateDate = DateTime.Now
               };
               _sampleRepository.Insert(entity);

               return new OperationResult(OperationResultType.Success, "新增数据成功！");
           }
           catch
           {
               return new OperationResult(OperationResultType.Error, "新增数据失败，数据库插入数据时发生了错误!");
           }
       }
       public OperationResult Update(SamplesVM model)
       {
           try
           {
               var user = Sampless.FirstOrDefault(c => c.SampleID == model.SampleID);
               if (user == null)
               {
                   throw new Exception();
               }
               user.Counter = model.Counter;
               user.Name = model.Name;
               user.UserID = model.UserID;
               user.Material = model.Material;
               user.SampleID = model.SampleID;
               user.SavePlace = model.SavePlace;
               user.Year = model.Year;             
               user.UmrID = model.UmrID;
               user.Remark = model.Remark;              
               user.UpdateDate = DateTime.Now;
               _sampleRepository.Update(user);
               return new OperationResult(OperationResultType.Success, "更新数据成功！");
           }
           catch
           {
               return new OperationResult(OperationResultType.Error, "更新数据失败!");
           }
       }





       public OperationResult Delete(IEnumerable<SamplesVM> list)
       {
           try
           {
               if (list != null)
               {
                   var sampleIds = list.Select(c => c.SampleID).ToList();
                   int count = _sampleRepository.Delete(_sampleRepository.Entities.Where(c => sampleIds.Contains(c.SampleID)));
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
       public OperationResult UMrIDDelete(List<string> list)
       {
           try
           {
               if (list != null)
               {
                   //var sampleIds = list.Select(c => c.SampleID).ToList();
                   int count = _sampleRepository.Delete(_sampleRepository.Entities.Where(c => list.Contains(c.UmrID)));
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
