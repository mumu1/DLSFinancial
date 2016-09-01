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
        private readonly IUserGroupService _userGroupService;
        public StatisticsController(IUserService userService, IRoleService roleService, IUserGroupService userGroupService)
        {
            this._userService = userService;
            this._roleService = roleService;
            this._userGroupService = userGroupService;
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
        
    }
}
