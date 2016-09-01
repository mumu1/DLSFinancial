using BEYON.Component.Tools;
using BEYON.Domain.Model.Plot;
using BEYON.ViewModel.Plot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BEYON.CoreBLL.Service.Plot.Interface
{
   public interface ISamplesService
    {
        IQueryable<Samples> Sampless { get; }
        OperationResult Insert(SamplesVM model);
        OperationResult Update(SamplesVM model);
        OperationResult Delete(IEnumerable<SamplesVM> list);

        OperationResult UMrIDDelete(List<string> list);
    }
}
