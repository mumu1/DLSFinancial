using System.Collections.Generic;
using System.Linq;
using BEYON.Component.Tools;
using BEYON.Domain.Model.Plot;
using BEYON.ViewModel.Plot;
using BEYON.ViewModel;
using System.Web;

namespace BEYON.CoreBLL.Service.Plot.Interface
{
    public interface IPhotosService
    {
        IQueryable<Photos> Photoss { get; }
        OperationResult Insert(PhotosVM model);
        OperationResult Update(PhotosVM model);
        OperationResult Delete(IEnumerable<PhotosVM> list);
        OperationResult UMrIDDelete(List<string> list);
    }
}
