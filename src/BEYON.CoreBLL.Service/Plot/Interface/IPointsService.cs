using System.Collections.Generic;
using System.Linq;
using BEYON.Component.Tools;
using BEYON.Domain.Model.Plot;
using BEYON.ViewModel.Plot;
using BEYON.ViewModel;

namespace BEYON.CoreBLL.Service.Plot.Interface
{
    public interface IPointsService
    {
        IQueryable<Points> Pointss { get; }
        OperationResult Insert(PointsVM model);
        OperationResult Update(PointsVM model);
        OperationResult Delete(IEnumerable<PointsVM> list);
        OperationResult UMrIDDelete(List<string> list);
    }
}
