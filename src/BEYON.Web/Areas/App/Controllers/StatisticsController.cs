using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Web.Mvc;
using BEYON.Component.Data.Enum;
using BEYON.Component.Tools;
using BEYON.Component.Tools.helpers;
using BEYON.CoreBLL.Service.Member.Interface;
using BEYON.CoreBLL.Service.App.Interface;
using BEYON.Domain.Model.Member;
using BEYON.ViewModel;
using BEYON.ViewModel.Member;
using BEYON.Web.Extension.Common;
using BEYON.Web.Extension.Filters;

namespace BEYON.Web.Areas.App.Controllers
{
    public class StatisticsController : Controller
    {
        private readonly IUserService _userService;
        private readonly IRoleService _roleService;
        private readonly IStatisticsService _statisticsServer;
        public StatisticsController(IUserService userService, IRoleService roleService, IStatisticsService statisticsServer)
        {
            this._userService = userService;
            this._roleService = roleService;
            this._statisticsServer = statisticsServer;
        }

        //
        // GET: /App/Statistics/PerPersonDetail
        [Layout]
        public ActionResult PerPersonDetail()
        {
            return PartialView("PerPersonDetail");
        }

        //
        // GET: /App/Statistics/Monthly
        [Layout]
        public ActionResult Monthly()
        {
            return PartialView("Monthly");
        }

        #region 按人明细统计业务流程
        //
        // GET: /App/Statistics/PerPersonDetailColumns
        public ActionResult PerPersonDetailColumns()
        {
            return Json(this._statisticsServer.GetPerPersonDetailColumns());
        }

        //
        // GET: /App/Statistics/PerPersonDetailDatas
        public ActionResult PerPersonDetailDatas()
        {
            var datas = this._statisticsServer.GetPerPersonDetail();

            return Json(new { total = datas.Count, data = datas }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region 按月明细统计业务流程
        //
        // GET: /App/Statistics/LaborStatisticsColumns
        public ActionResult LaborStatisticsColumns()
        {
            return Json(this._statisticsServer.GetLaborStatisticsColumns());
        }

        //
        // GET: /App/Statistics/LaborStatisticsDatas
        public ActionResult LaborStatisticsDatas()
        {
            var datas = this._statisticsServer.GetLaborStatisticsDetail();

            return Json(new { total = datas.Count, data = datas }, JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}
