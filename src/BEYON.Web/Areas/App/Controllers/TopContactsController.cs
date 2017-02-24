﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Web.Mvc;
using BEYON.Component.Data.Enum;
using BEYON.Component.Tools;
using BEYON.Component.Tools.helpers;
using BEYON.CoreBLL.Service.App.Interface;
using BEYON.Web.Extension.Common;
using BEYON.Web.Extension.Filters;
using BEYON.Domain.Model.App;
using BEYON.ViewModel.App;
using System.IO;


namespace BEYON.Web.Areas.App.Controllers
{
    public class TopContactsController : Controller
    {
         private readonly ITopContactsService _topContactsService;


         public TopContactsController(ITopContactsService topContactsService)
        {
            this._topContactsService = topContactsService;            
        }


        //
         // GET: /App/TopContacts/Index
        [Layout]
        public ActionResult Index()
        {
            return PartialView();
        }
        // GET: /App/TopContacts/GetAllData/
        public ActionResult GetAllData()
        {
            string userid = ((System.Web.Security.FormsIdentity)(System.Web.HttpContext.Current.User.Identity)).Ticket.UserData;
            var result = this._topContactsService.GetTopContactsByUserID(userid);
            return Json(new { total = result.Count, data = result }, JsonRequestBehavior.AllowGet);

        }

        // GET: /App/TopContacts/GetContactsByName/
        public ActionResult GetContactsByName(String name)
        {
            var result = this._topContactsService.GetTopContactsByName(name.Trim().Replace("\n", "").Replace(" ", "").Replace("\t", "").Replace("\r", ""));
            return Json(new { total = result.Count, data = result }, JsonRequestBehavior.AllowGet);

        }

        // POST: /App/TopContacts/Create/
        [HttpPost]
        public ActionResult Create()
        {
            string userid = ((System.Web.Security.FormsIdentity)(System.Web.HttpContext.Current.User.Identity)).Ticket.UserData;
            TopContactsVM[] datas = ClassConvert<TopContactsVM>.Process(Request.Form);
            datas[0].UserID = userid;
            var result = _topContactsService.Insert(datas[0]);
            if (result.ResultType != OperationResultType.Success)
                return Json(new { error = result.ResultType.GetDescription(), total = 1, data = this._topContactsService.TopContactss.ToArray() });
            else
            {
                TopContacts[] results = this._topContactsService.TopContactss.ToArray();
                return Json(new { total = 1, data = new[] { results[results.Length - 1] } });
            }

        }

        // POST: /App/TopContacts/Edit/
        [HttpPost]
        public ActionResult Edit()
        {
            string userid = ((System.Web.Security.FormsIdentity)(System.Web.HttpContext.Current.User.Identity)).Ticket.UserData;           
            TopContacts[] datas = ClassConvert<TopContacts>.Process(Request.Form);
            foreach (var data in datas)
            {
                data.UserID = userid;
                var result = _topContactsService.Update(data);
                result.Message = result.Message ?? result.ResultType.GetDescription();
                if (result.ResultType != OperationResultType.Success)
                {
                    return Json(new { error = result.ResultType.GetDescription(), total = datas.Length, data = datas });
                }
            }

            return Json(new { total = datas.Length, data = datas }, JsonRequestBehavior.AllowGet);
        }

        // POST: /App/TopContacts/Delete/
        public ActionResult Delete()
        {
            string userid = ((System.Web.Security.FormsIdentity)(System.Web.HttpContext.Current.User.Identity)).Ticket.UserData;           
            TopContacts[] datas = ClassConvert<TopContacts>.Process(Request.Form);
            foreach (var data in datas)
            {
                data.UserID = userid;
                var result = _topContactsService.Delete(data);
                if (result.ResultType != OperationResultType.Success)
                {
                    return Json(new { error = result.ResultType.GetDescription(), total = datas.Length, data = datas });
                }
            }
            return Json(new { total = datas.Length, data = datas }, JsonRequestBehavior.AllowGet);
        }

       
    }
}
