using System.Collections.Generic;
using System.Linq;
using BEYON.Component.Tools;
using BEYON.Domain.Model.Plot;
using BEYON.ViewModel.Plot;
using BEYON.ViewModel;
using System.Web;

namespace BEYON.CoreBLL.Service.Plot.Interface
{
    public interface IImportAntsitesService
    {
        IQueryable<ImportAntsites> ImportAntsitess { get; }
        OperationResult Insert(ImportAntsitesVM model);
        OperationResult Update(ImportAntsitesVM model);
        OperationResult Delete(IEnumerable<ImportAntsitesVM> list);
        OperationResult UMrIDDelete(List<string> list);
    }
}
