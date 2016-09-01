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

namespace BEYON.Web.Areas.BasicDataManagement.Controllers
{
    public class WageBaseTableController : Controller
    {
        private readonly IUserService _userService;
        private readonly IRoleService _roleService;
        private readonly IUserGroupService _userGroupService;
        public WageBaseTableController(IUserService userService, IRoleService roleService, IUserGroupService userGroupService)
        {
            this._userService = userService;
            this._roleService = roleService;
            this._userGroupService = userGroupService;
        }

        //
        // GET: /BasicDataManagement/WageBaseTable/
        [Layout]
        public ActionResult Index()
        {
            return PartialView();
        }

    }
}
