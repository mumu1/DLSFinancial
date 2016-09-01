using System.Collections.Generic;
using System.Linq;
using BEYON.Component.Tools;
using BEYON.Domain.Model.Plot;
using BEYON.ViewModel.Plot;
using BEYON.ViewModel;
using System.Web;

namespace BEYON.CoreBLL.Service.Plot.Interface
{
    public interface IDraftsService
    {
        IQueryable<Drafts> Draftss { get; }
        OperationResult Insert(DraftsVM model);
        OperationResult Update(DraftsVM model);
        OperationResult Delete(IEnumerable<DraftsVM> list);
        OperationResult UMrIDDelete(List<string> list);
    }
}
