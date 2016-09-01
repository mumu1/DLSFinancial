using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Entity.SqlServer;
using System.Linq;
using System.Linq.Expressions;
using System.Web.Mvc;
using BEYON.Component.Data.Enum;
using BEYON.Component.Tools;
using BEYON.Component.Tools.helpers;
using BEYON.CoreBLL.Service.Member.Interface;
using BEYON.Domain.Model.Member;
using BEYON.ViewModel.Member;
using BEYON.Web.Extension.Common;
using BEYON.Web.Extension.Filters;

namespace BEYON.Web.Areas.BasicDataManagement.Controllers
{
    public class ReimbursementController : Controller
    {
        private readonly IModuleService _moduleService;
        public ReimbursementController(IModuleService moduleService)
        {
            this._moduleService = moduleService;
        }

        //
        // GET: /BasicDataManagement/Reimbursement/
        [Layout]
        public ActionResult Index()
        {
            return PartialView();
        }

    }
}
