using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BEYON.Component.Tools;
using BEYON.Domain.Model.App;
using BEYON.ViewModel.App;


namespace BEYON.CoreBLL.Service.App.Interface
{
    public interface ISafeguardTimeService
    {
        IQueryable<SafeguardTime> SafeguardTimes { get; }
        OperationResult Insert(SafeguardTime model);
        OperationResult Update(SafeguardTime model);

        string GetSafeguardTime();
    }
}
