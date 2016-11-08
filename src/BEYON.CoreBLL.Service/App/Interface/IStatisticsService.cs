using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BEYON.ViewModel.App;

namespace BEYON.CoreBLL.Service.App.Interface
{
    public interface IStatisticsService
    {
        #region 按人统计明细表
        Column[] GetPerPersonDetailColumns();

        List<Object> GetPerPersonDetail();
        #endregion

        #region 按劳务统计明细表
        Column[] GetLaborStatisticsColumns();

        List<Object> GetLaborStatisticsDetail();
        #endregion

        #region 按课题统计明细表
        Column[] GetTaskStatisticsColumns();

        List<Object> GetTaskStatisticsDetail();
        #endregion
    }
}
