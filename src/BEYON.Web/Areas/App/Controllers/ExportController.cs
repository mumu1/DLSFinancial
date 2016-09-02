using Newtonsoft.Json;
using System;
using System.IO;
using System.Data;
using System.IO.Compression;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Caching;
using System.Diagnostics;
using System.ComponentModel;
using Microsoft.Win32;
using BEYON.Component.Data.EF;
using BEYON.Component.Data.Enum;
using BEYON.Component.Tools;
using BEYON.Component.Tools.helpers;
using BEYON.CoreBLL.Service;
//using BEYON.CoreBLL.Service.Plot.Interface;
using BEYON.Domain.Model.Plot;
using BEYON.ViewModel;
using BEYON.ViewModel.Plot;
using BEYON.Web.Extension.Common;
using BEYON.Web.Extension.Filters;
using BEYON.Domain.Model.Member;
using BEYON.CoreBLL.Service.Member.Interface;
using BEYON.ViewModel.Member;
//using BEYON.Domain.Data.Repositories.Plot;

namespace BEYON.Web.Areas.App.Controllers
{
    public class ExportController : Controller
    {
        //private readonly IExportService _exportService;

        //public ExportController(IExportService exportService)
        //{
        //    this._exportService = exportService;
        //}

        [HttpPost]
        public ActionResult ExportFile(string[] urmids)
        {
            var filepath = System.IO.Path.Combine(Server.MapPath("/"));
            if (!Directory.Exists(filepath + "Exports/"))
            {
                Directory.CreateDirectory(filepath + "Exports/");
            }

            String file = null;// this._exportService.Export(filepath, urmids.ToList());


            return Json(file);
        }

        [HttpPost]
        public void DownloadFile(string filePath)
        {
            try
            {
                if (String.IsNullOrEmpty(filePath))
                    return;

                var filepath = System.IO.Path.Combine(Server.MapPath("/Exports/"), filePath);

                System.IO.FileInfo file = new System.IO.FileInfo(filepath);
                if (file.Exists)//判断文件是否存在
                {
                    const long ChunkSize = 102400;//100K 每次读取文件，只读取100Ｋ，这样可以缓解服务器的压力
                    byte[] buffer = new byte[ChunkSize];

                    Response.Clear();
                    System.IO.FileStream iStream = System.IO.File.OpenRead(filepath);
                    long dataLengthToRead = iStream.Length;//获取下载的文件总大小
                    Response.ContentType = "application/octet-stream";
                    Response.AddHeader("Content-Disposition", "attachment; filename=" + HttpUtility.UrlEncode("Export.zip"));
                    while (dataLengthToRead > 0 && Response.IsClientConnected)
                    {
                        int lengthRead = iStream.Read(buffer, 0, Convert.ToInt32(ChunkSize));//读取的大小
                        Response.OutputStream.Write(buffer, 0, lengthRead);
                        Response.Flush();
                        dataLengthToRead = dataLengthToRead - lengthRead;
                    }
                    Response.Close();
                    //Response.End();

                    //if (file.Attributes.ToString().IndexOf("ReadOnly") != -1)
                    //{
                    //    file.Attributes = FileAttributes.Normal;
                    //}
                    //System.IO.File.Delete(file.FullName);
                }
            }
            catch (Exception e)
            {
                Console.Write(e.ToString());
            }
        }

    }
}
