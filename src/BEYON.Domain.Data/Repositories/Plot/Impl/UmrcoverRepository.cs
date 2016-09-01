/************************************
 * 描述：尚未添加描述
 * 作者：林永恒
 * 日期：2016/7/7 16:48:54  
*************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using BEYON.Domain.Model.Plot;
using BEYON.ViewModel.Plot;

namespace BEYON.Domain.Data.Repositories.Plot.Impl
{
    public partial class UmrcoverRepository
    {
        public IList<StatisticsVM> GetListStatisticsVM(DateTime start, DateTime end)
        {
            var q = from p in Context.Audits.Where(w => w.OperateTime > start && w.OperateTime < end && w.AuditStatus == "通过")
                        join u in Context.Umrcovers on p.UmrID equals u.UmrID
                            join b in Context.BasicPropertys on p.UmrID equals b.UmrID orderby b.Year
                                    select new StatisticsVM
                                    {
                                        UmrID = p.UmrID,
                                        OperateTime = p.OperateTime,
                                        Name = u.Name,
                                        Year = b.Year
                                    };
            return q.ToList();
        }

        /// <summary>
        /// 朝代统计考古数量，用于饼图计算
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        public IList<DynastyVM> GetDynastysVM(DateTime start, DateTime end)
        {
            var q = from p in Context.Audits.Where(w => w.OperateTime > start && w.OperateTime < end && w.AuditStatus == "通过")
                    join u in Context.Umrcovers on p.UmrID equals u.UmrID
                    join b in Context.BasicPropertys on p.UmrID equals b.UmrID
                    group b by b.Year into groupAudtis
                    select new DynastyVM
                    {
                        Year = groupAudtis.Key,
                        Number = groupAudtis.Count()
                    };
            return q.ToList();
        }

        /// <summary>
        /// 统计每月新增考古数量
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        public IQueryable<NewRelicsVM> GetNewDynastyPerMonthVM(DateTime start, DateTime end)
        {
            var q = from p in Context.Audits.Where(w => w.OperateTime > start && w.OperateTime < end && w.AuditStatus == "通过")
                        join u in Context.Umrcovers on p.UmrID equals u.UmrID
                            join b in Context.BasicPropertys on p.UmrID equals b.UmrID 
                                group p by p.OperateTime.Year.ToString() + p.OperateTime.Month.ToString() into groupAudit
                                    select new NewRelicsVM
                                    {
                                        Date = groupAudit.Key,
                                        Count = groupAudit.Count()
                                    };

            return q;
        }
    }
}
