using System.Collections.Generic;
using System.Linq;
using BEYON.Component.Tools;
using BEYON.Domain.Model.Plot;
using BEYON.ViewModel.Plot;
using BEYON.ViewModel;
using System.Web;

namespace BEYON.CoreBLL.Service.Plot.Interface
{
    public interface IDigsituationBeforeService
    {
        IQueryable<DigsituationBefore> DigsituationBefores { get; }
        OperationResult Insert(DigsituationBeforeVM model);
        OperationResult Update(DigsituationBeforeVM model);
        OperationResult Delete(IEnumerable<DigsituationBeforeVM> list);
        OperationResult UMrIDDelete(List<string> list);
    }
}
