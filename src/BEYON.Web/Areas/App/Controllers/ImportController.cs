using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Web.Mvc;
using System.Web;
using System.IO;
using System.Web.Caching;
using BEYON.Component.Data.EF;
using BEYON.Component.Data.Enum;
using BEYON.Component.Tools;
using BEYON.Component.Tools.helpers;
using BEYON.CoreBLL.Service;
//using BEYON.CoreBLL.Service.Plot.Interface;
using BEYON.Domain.Model.App;
using BEYON.ViewModel;
using BEYON.ViewModel.App;
using BEYON.Web.Extension.Common;
using BEYON.Web.Extension.Filters;
using BEYON.Domain.Model.Member;
using BEYON.CoreBLL.Service.Member.Interface;
using BEYON.ViewModel.Member;
//using BEYON.Domain.Data.Repositories.Plot;


namespace BEYON.Web.Areas.App.Controllers
{
    public class ImportController : Controller
    {
        //private readonly IImportService _importService;

        //public ImportController(IImportService importService)
        //{
        //    this._importService = importService;
        //}

        public ActionResult ImportData()
        {
            return PartialView("ImportData");
        }

        [HttpPost]
        public ActionResult Import(HttpPostedFileBase upload)
        {
            string fileName = "";
            String filePath = Server.MapPath("~/Imports");
            if(!Directory.Exists(filePath))
            {
                Directory.CreateDirectory(filePath);
            }

            string tempName = DateTime.Now.ToString("yyyyMMddHHMMss") + DateTime.Now.Millisecond;
            if (upload.ContentLength > 0)
            {
                fileName = tempName + Path.GetFileName(upload.FileName);
                var path = Path.Combine(filePath, fileName);
                upload.SaveAs(path);

                var filepath = System.IO.Path.Combine(Server.MapPath("/"));

                //this._importService.Import(path, filepath);
            }

            return Json(new OperationResult(OperationResultType.ParamError, "参数错误，请重新检查输入"));
        }
        
    }
}
