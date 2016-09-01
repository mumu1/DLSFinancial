using System.Collections.Generic;
using System.Linq;
using BEYON.Component.Tools;
using BEYON.Domain.Model.Plot;
using BEYON.ViewModel;
using BEYON.ViewModel.Plot;

namespace BEYON.CoreBLL.Service.Plot.Interface
{
    public interface IPlotBeforeService
    {
        IQueryable<PlotBefore> PlotBefores { get; }
        OperationResult Insert(PlotBeforeVM model);
        OperationResult Update(PlotBeforeVM model);
        OperationResult Delete(IEnumerable<PlotBeforeVM> list);
    }
}
