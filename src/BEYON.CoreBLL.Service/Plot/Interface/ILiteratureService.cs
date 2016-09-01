using System.Collections.Generic;
using System.Linq;
using BEYON.Component.Tools;
using BEYON.Domain.Model.Plot;
using BEYON.ViewModel.Plot;
using BEYON.ViewModel;
using System.Web;

namespace BEYON.CoreBLL.Service.Plot.Interface
{
    public interface ILiteratureService
    {
        IQueryable<Literature> Literatures { get; }
        OperationResult Insert(LiteratureVM model);
        OperationResult Update(LiteratureVM model);
        OperationResult Delete(IEnumerable<LiteratureVM> list);
        OperationResult UMrIDDelete(List<string> list);
    }
}
