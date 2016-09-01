using System.Collections.Generic;
using System.Linq;
using BEYON.Component.Tools;
using BEYON.Domain.Model.Plot;
using BEYON.ViewModel;
using BEYON.ViewModel.Plot;

namespace BEYON.CoreBLL.Service.Plot.Interface
{
    public interface IProtectUnitsService
    {
        IQueryable<ProtectUnits> ProtectUnitss { get; }
        OperationResult Insert(ProtectUnitsVM model);
        OperationResult Update(ProtectUnitsVM model);
        OperationResult Delete(IEnumerable<ProtectUnitsVM> list);
        OperationResult UMrIDDelete(List<string> list);
    }
}
