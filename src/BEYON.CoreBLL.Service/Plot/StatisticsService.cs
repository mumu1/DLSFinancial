using BEYON.Component.Data.EF.Interface;
using BEYON.Component.Tools;
using BEYON.CoreBLL.Service.Plot.Interface;
using BEYON.Domain.Data.Repositories.Plot;
using BEYON.Domain.Model.Plot;
using BEYON.ViewModel.Plot;
using System.Collections.Generic;
using System.Linq;
using EntityFramework.Extensions;
using System;

namespace BEYON.CoreBLL.Service.Plot
{
    public class StatisticsService : CoreServiceBase, IStatisticsService
    {
         private readonly IUmrcoverRepository _umrcoverRepository;


         public StatisticsService(IUmrcoverRepository umrcoverRepository, IUnitOfWork unitOfWork)
            : base(unitOfWork)
        {
            this._umrcoverRepository = umrcoverRepository;
        }

        public IList<StatisticsVM> GetListStatistics(DateTime start, DateTime end)
        {
            return _umrcoverRepository.GetListStatisticsVM(start, end);
        }

        public IList<DynastyVM> GetDynastys(DateTime start, DateTime end)
        {
            return _umrcoverRepository.GetDynastysVM(start, end);
        }

        public IQueryable<NewRelicsVM> GetNewDynastyPerMonth(DateTime start, DateTime end)
        {
            return _umrcoverRepository.GetNewDynastyPerMonthVM(start, end);
        }
    }
}
