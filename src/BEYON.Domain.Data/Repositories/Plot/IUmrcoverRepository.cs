using BEYON.Component.Data.EF.Interface;
using BEYON.Domain.Model.Plot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BEYON.ViewModel.Plot;

namespace BEYON.Domain.Data.Repositories.Plot
{
    public partial interface IUmrcoverRepository
    {
        IList<StatisticsVM> GetListStatisticsVM(DateTime start, DateTime end);

        IList<DynastyVM> GetDynastysVM(DateTime start, DateTime end);

        IQueryable<NewRelicsVM> GetNewDynastyPerMonthVM(DateTime start, DateTime end);

        IList<Umrcover> Export(IList<String> umrids);

        void InsertOrUpdate(IQueryable<Umrcover> umrcovers);
    }
}
