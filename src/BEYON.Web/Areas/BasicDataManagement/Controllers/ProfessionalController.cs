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
using BEYON.Domain.Model.App;
using BEYON.ViewModel.App;
using BEYON.CoreBLL.Service.App.Interface;

namespace BEYON.Web.Areas.BasicDataManagement.Controllers
{
    public class ProfessionalController : Controller
    {
        private readonly ITitleService _titleService;
        public ProfessionalController(ITitleService titleService)
        {       
            this._titleService = titleService;
        }

        //
        // GET: /BasicDataManagement/Professional/Index
        [Layout]
        public ActionResult Index()
        {
            return PartialView();
        }

        // GET: /BasicDataManagement/Professional/GetAllData/
        public ActionResult GetAllData()
        {
            var result = this._titleService.Titles.ToList();
            return Json(new { total = result.Count, rows = result }, JsonRequestBehavior.AllowGet);

        }
    }
}
