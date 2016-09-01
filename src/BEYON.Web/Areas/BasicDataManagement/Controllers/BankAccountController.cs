using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Web.Mvc;
using BEYON.Component.Data.EF;
using BEYON.Component.Data.Enum;
using BEYON.Component.Tools;
using BEYON.Component.Tools.helpers;
using BEYON.CoreBLL.Service;
using BEYON.CoreBLL.Service.Member.Interface;
using BEYON.Domain.Model.Member;
using BEYON.ViewModel;
using BEYON.ViewModel.Member;
using BEYON.Web.Extension.Common;
using BEYON.Web.Extension.Filters;

namespace BEYON.Web.Areas.BasicDataManagement.Controllers
{
    public class BankAccountController : Controller
    {
        private readonly IRoleService _roleService;
        public BankAccountController(IRoleService roleService)
        {
            this._roleService = roleService;
        }

        //
        // GET: /BasicDataManagement/BankAccount/Index
        [Layout]
        public ActionResult Index()
        {
            return PartialView();
        }
    }
}