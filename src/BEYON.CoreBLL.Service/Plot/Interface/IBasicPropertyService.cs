using System.Collections.Generic;
using System.Linq;
using BEYON.Component.Tools;
using BEYON.Domain.Model.Plot;
using BEYON.ViewModel.Plot;
using BEYON.ViewModel;

namespace BEYON.CoreBLL.Service.Plot.Interface
{
    public interface IBasicPropertyService
    {
        IQueryable<BasicProperty> BasicPropertys { get; }
        OperationResult Insert(BasicPropertyVM model);
        OperationResult Update(BasicPropertyVM model);
        OperationResult Delete(IEnumerable<BasicPropertyVM> list);
        OperationResult UMrIDDelete(List<string> list);
    }
}
