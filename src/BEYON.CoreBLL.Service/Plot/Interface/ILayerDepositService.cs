using System.Collections.Generic;
using System.Linq;
using BEYON.Component.Tools;
using BEYON.Domain.Model.Plot;
using BEYON.ViewModel.Plot;
using BEYON.ViewModel;
using System.Web;

namespace BEYON.CoreBLL.Service.Plot.Interface
{
    public interface ILayerDepositService
    {
        IQueryable<LayerDeposit> LayerDeposits { get; }
        OperationResult Insert(LayerDepositVM model);
        OperationResult Update(LayerDepositVM model);
        OperationResult Delete(List<string> list);
        OperationResult UMrIDDelete(List<string> list);
    }
}
