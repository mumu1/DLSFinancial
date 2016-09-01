using System.Collections.Generic;
using System.Linq;
using BEYON.Component.Tools;
using BEYON.Domain.Model.Plot;
using BEYON.ViewModel.Plot;
using BEYON.ViewModel;
using System.Web;

namespace BEYON.CoreBLL.Service.Plot.Interface
{
    public interface IOthersService
    {
        IQueryable<Others> Otherss { get; }
        OperationResult Insert(OthersVM model);
        OperationResult Update(OthersVM model);
        OperationResult Delete(IEnumerable<OthersVM> list);
        OperationResult UMrIDDelete(List<string> list);
    }
}
