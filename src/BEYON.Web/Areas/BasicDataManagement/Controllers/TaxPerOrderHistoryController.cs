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
using BEYON.Domain.Model.App;
using BEYON.ViewModel.App;
using BEYON.CoreBLL.Service.App.Interface;
using BEYON.CoreBLL.Service.Excel.Interface;
using System.IO;
using BEYON.CoreBLL.Service.Excel;

namespace BEYON.Web.Areas.BasicDataManagement.Controllers
{
    public class TaxPerOrderHistoryController : Controller
    {
        private readonly log4net.ILog _log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
         private readonly ITaxPerOrderHistoryService _taxPerOrderHistoryService;
         private readonly IApplyPrintService _applyPrintService;

         public TaxPerOrderHistoryController(ITaxPerOrderHistoryService taxPerOrderHistoryService, IApplyPrintService applyPrintService)
        {
            this._taxPerOrderHistoryService = taxPerOrderHistoryService;
            this._applyPrintService = applyPrintService;
        }


        //
         // GET: /BasicDataManagement/TaxPerOrderHistory/Index
        [Layout]
        public ActionResult Index()
        {
            return PartialView();
        }

        // GET: /BasicDataManagement/TaxPerOrderHistory/GetAllData/
        public ActionResult GetAllData()
        {
            //var echo = int.Parse(HttpContext.Request.Params["sEcho"]);
            //var displayLength = int.Parse(HttpContext.Request.Params["iDisplayLength"]);
            //var displayStart = int.Parse(HttpContext.Request.Params["iDisplayStart"]);
            //var sortOrder = HttpContext.Request.Params["sSortDir_0"].ToString();

            //var records = this._taxPerOrderHistoryService.TaxPerOrderHistorys.ToList();

            //var orderedResults = sortOrder == "asc"
            //                  ? records.OrderBy(o => o.Id)
            //                  : records.OrderByDescending(o => o.Id);

            //var itemsToSkip = displayStart == 0
            //                  ? 0
            //                  : displayStart + 1;

            //var pagedResults = orderedResults.Skip(itemsToSkip).Take(displayLength).ToList();


            //return Json(new { sEcho = echo, 
            //    recordsTotal = records.Count,
            //    recordsFiltered = records.Count,
            //    iTotalRecords = records.Count,
            //    iTotalDisplayRecords = records.Count,
            //    aaData = pagedResults
            //}, JsonRequestBehavior.AllowGet);

            var start = 0;
            var limit = 0;

            if (Request.Params["iDisplayStart"] != null)
            {
                //数据起始位置
                start = Int32.Parse(Request.Params["iDisplayStart"]);
                //数据长度
                limit = Int32.Parse(Request.Params["iDisplayLength"]);
            }

            String sortName = null;
            String sortType = null;
            var searchText = Request.Params["sSearch"];
            var sortidx = Request.Params["iSortCol_0"];
            if (!String.IsNullOrEmpty(sortidx))
            {
                sortName = Request.Params["mDataProp_" + sortidx];
                sortType = Request.Params["sSortDir_0"];
            }

            var total = this._taxPerOrderHistoryService.GetTotal(searchText);
            var records = this._taxPerOrderHistoryService.GetAllData(searchText, sortName, sortType, start, limit);

            //var records = this._taxPerOrderHistoryService.TaxPerOrderHistorys.ToList();

            /*
           var result = Json(new { total = records.Count, data = records }, JsonRequestBehavior.AllowGet);
           result.MaxJsonLength = Int32.MaxValue;

           return result;
            * */
            return new JsonResult()
            {
                //Data = new { total = records.Count, data = records },
                Data = new { status = "success",  recordsTotal = total, recordsFiltered = total, data = records },
                MaxJsonLength = int.MaxValue,
                ContentType = "application/json",
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        // GET: /BasicDataManagement/TaxPerOrderHistory/ExportAllExcels
        public ActionResult ExportAllExcels()
        {
            string fullPath = Server.MapPath("/Exports/");

            string userid = ((System.Web.Security.FormsIdentity)(System.Web.HttpContext.Current.User.Identity)).Ticket.UserData;
            var userID = Int32.Parse(userid);
        
            //数据起始位置
            int start = 0;
            //数据长度
            int limit = -1;

            String sortName = null;
            String sortType = null;
            var searchText = Request.Params["sSearch"];
            var sortidx = Request.Params["iSortCol_0"];
            if (!String.IsNullOrEmpty(sortidx))
            {
                sortName = Request.Params["mDataProp_" + sortidx];
                sortType = Request.Params["sSortDir_0"];
            }

            var rows = this._taxPerOrderHistoryService.GetAllData(searchText, sortName, sortType, start, limit);

            IList<String> headNames = new List<String> { "期间", "申请单流水号","姓名","人员类型","证件类型","证件号码","支付类型","金额","是否含税","应纳税额","税前金额","税后金额", "课题号", "课题名称", "课题负责人", "经办人", "开户银行", "银行账号", "收款账号省份", "收款账号地市", "开户行详细名称", "联系电话"};
            IList<int> headWidths = new List<int>      { 8,      15,            8,    10,        10,        18,       10,         10,    8,         8,        8,         8,         10,         30,        10,            8,        10,        20,          10,             10,             30,               10 };
            int rowCount = rows.Count;
            int columnCount = headNames.Count;
            object[,] cellData = new object[rowCount, columnCount];
            for (int iRow = 0; iRow < rowCount; iRow++)
            {
                var row = rows[iRow];
                cellData[iRow, 0] = row.Period;
                cellData[iRow, 1] = row.ProjectNumber;
                cellData[iRow, 2] = row.Name;
                cellData[iRow, 3] = row.PersonType;
                cellData[iRow, 4] = row.CertificateType;
                cellData[iRow, 5] = row.CertificateID;
                cellData[iRow, 6] = row.PaymentType;
                cellData[iRow, 7] = row.Amount;
                cellData[iRow, 8] = row.TaxOrNot;
                cellData[iRow, 9] = row.Tax;
                cellData[iRow, 10] = row.AmountX;
                cellData[iRow, 11] = row.AmountY;
                cellData[iRow, 12] = row.ProjectNumber;
                cellData[iRow, 13] = row.TaskName;
                cellData[iRow, 14] = row.ProjectDirector;
                cellData[iRow, 15] = row.Agent;
                cellData[iRow, 16] = row.Bank;
                cellData[iRow, 17] = row.AccountNumber;
                cellData[iRow, 18] = row.ProvinceCity;
                cellData[iRow, 19] = row.CityField;
                cellData[iRow, 20] = row.BankDetailName;
                cellData[iRow, 21] = row.Tele;
            }

            String fileName = this._applyPrintService.ExportAllExcel(fullPath, "历史算税记录", headNames, headWidths, cellData);
            var jsonResult = Json(fileName, JsonRequestBehavior.AllowGet);
            return jsonResult;
        }

        [HttpPost]
        public void DownloadFile(string filePath)
        {
            try
            {
                if (String.IsNullOrEmpty(filePath))
                    return;

                var filepath = System.IO.Path.Combine(Server.MapPath("/Exports/"), filePath);
                _log.Info(filepath);
                System.IO.FileInfo file = new System.IO.FileInfo(filepath);
                if (file.Exists)//判断文件是否存在
                {
                    Response.Clear();
                    Response.ClearHeaders();
                    Response.AddHeader("Content-Disposition", "attachment;filename=" + System.Web.HttpUtility.UrlEncode("历史算税记录.xlsx"));
                    Response.AddHeader("Content-Length", new FileInfo(filepath).Length.ToString());
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.TransmitFile(filepath);

                    if (Response.IsClientConnected)
                    {
                        //Response.Close();
                        Response.End();
                    }

                    System.IO.File.SetAttributes(filepath, System.IO.FileAttributes.Normal);
                    System.IO.File.Delete(filepath);
                }
            }
            catch (Exception e)
            {
                _log.Error(e);
            }
        }

        [HttpPost]
        public ActionResult DeleteFile(string fileName)
        {
            if (String.IsNullOrEmpty(fileName))
                return Json(new { });
            var filepath = System.IO.Path.Combine(Server.MapPath("/Exports/"), fileName);
            System.IO.FileInfo file = new System.IO.FileInfo(filepath);
            if (file.Exists)
            {
                if (file.Attributes.ToString().IndexOf("ReadOnly") != -1)
                {
                    file.Attributes = System.IO.FileAttributes.Normal;
                }
                System.IO.File.Delete(file.FullName);
            }

            return Json(new { });
        }
    
    } 
}