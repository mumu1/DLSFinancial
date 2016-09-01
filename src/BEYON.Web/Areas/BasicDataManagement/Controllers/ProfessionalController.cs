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
    public class ProfessionalController : Controller
    {
        private readonly IPermissionService _permissionService;
        private readonly IModuleService _moduleService;
        public ProfessionalController(IPermissionService permissionService, IModuleService moduleService)
        {
            this._permissionService = permissionService;
            this._moduleService = moduleService;
        }

        //
        // GET: /BasicDataManagement/ProfessionalController/
        [Layout]
        public ActionResult Index()
        {
            return PartialView();
        }
    }
}
