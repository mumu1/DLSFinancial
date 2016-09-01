using System.Collections.Generic;
using System.Linq;
using BEYON.Component.Tools;
using BEYON.Domain.Model.Plot;
using BEYON.ViewModel.Plot;
using BEYON.ViewModel;

namespace BEYON.CoreBLL.Service.Plot.Interface
{
    public interface IUmrcoverService
    {
        IQueryable<Umrcover> Umrcovers { get; }
        OperationResult Insert(UmrcoverVM model);
        OperationResult Update(UmrcoverVM model);
        OperationResult Delete(List<string> list);
    }
}
