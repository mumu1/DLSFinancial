using System;
using System.Collections.Generic;
using System.Linq;
using BEYON.Component.Tools;
using BEYON.Domain.Model.Plot;
using BEYON.ViewModel.Plot;
using BEYON.ViewModel;

namespace BEYON.CoreBLL.Service.Plot.Interface
{
    public interface IStatisticsService
    {
        /// <summary>
        /// 获取指定时间内，古墓相关信息，用于统计模块列表暂时
        /// </summary>
        /// <param name="start">录入时间</param>
        /// <param name="end">录入时间</param>
        /// <returns></returns>
        IList<StatisticsVM> GetListStatistics(DateTime start, DateTime end);

        /// <summary>
        /// 获取指定时间内的朝代遗址统计
        /// </summary>
        /// <param name="start">录入时间</param>
        /// <param name="end">录入时间</param>
        /// <returns></returns>
        IList<DynastyVM> GetDynastys(DateTime start, DateTime end);

        /// <summary>
        /// 每月新增遗址数目
        /// </summary>
        /// <param name="start">录入时间</param>
        /// <param name="end">录入时间</param>
        /// <returns></returns>
        IQueryable<NewRelicsVM> GetNewDynastyPerMonth(DateTime start, DateTime end);
    }
}
