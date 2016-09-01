using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BEYON.ViewModel.Plot;

namespace BEYON.CoreBLL.Service.Plot.Interface
{
    public interface INotifyService
    {
        NotifyVM GetNotify(int userID);
    }
}
