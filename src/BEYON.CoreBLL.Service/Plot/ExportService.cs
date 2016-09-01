using System;
using System.IO;
using System.Linq;
using System.Text;
using System.IO.Compression;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Reflection;
using log4net;
using ExcelExporter;
using BEYON.Domain.Model.Plot;
using BEYON.CoreBLL.Service.Plot.Interface;
using BEYON.Domain.Data.Repositories.Plot;

namespace BEYON.CoreBLL.Service.Plot
{
    public class ExportService : IExportService
    {
        private readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private readonly IUmrcoverRepository _umrcoverRepository;
        private readonly IBasicPropertyRepository _basicPropertyRepository;
        private readonly IDraftsRepository _draftsRepository;
        private readonly IOthersRepository _othersRepository;
        private readonly IPhotosRepository _photosRepository;
        private readonly IPointsRepository _pointsRepository;
        private readonly ISamplesRepository _sampleRepository;
        private readonly IDigsituationBeforeRepository _digsituationBeforeRepository;
        private readonly IImportAntsitesRepository _importAntsitesRepository;
        private readonly ILayerDepositRepository _layerDepositRepository;
        private readonly ILiteratureRepository _literatureRepository;
        private readonly IAuditRepository _auditRepository;

        public ExportService(IUmrcoverRepository umrcoverRepository, IBasicPropertyRepository basicPropertyRepository,
            IDraftsRepository draftsRepository, IOthersRepository othersRepository, IPhotosRepository photosRepository,
            IPointsRepository pointsRepository, ISamplesRepository sampleRepository, IDigsituationBeforeRepository digsituationBeforeRepository,
            IImportAntsitesRepository importAntsitesRepository, ILayerDepositRepository layerDepositRepository,
            ILiteratureRepository literatureRepository, IAuditRepository auditRepository)
        {
            this._umrcoverRepository = umrcoverRepository;
            this._basicPropertyRepository = basicPropertyRepository;
            this._draftsRepository = draftsRepository;
            this._othersRepository = othersRepository;
            this._photosRepository = photosRepository;
            this._pointsRepository = pointsRepository;
            this._sampleRepository = sampleRepository;
            this._digsituationBeforeRepository = digsituationBeforeRepository;
            this._importAntsitesRepository = importAntsitesRepository;
            this._layerDepositRepository = layerDepositRepository;
            this._literatureRepository = literatureRepository;
            this._auditRepository = auditRepository;
        }

        public String Export(string filePath, List<String> umrIds)
        {
            IList<String> files = new List<String>();

            IList<Umrcover> umrcovers = this._umrcoverRepository.Export(umrIds) ;
            IList<BasicProperty> basicPropertys = this._basicPropertyRepository.Export(umrIds);
            IList<Drafts> draftss = this._draftsRepository.Export(umrIds);
            IList<Others> otherss = this._othersRepository.Export(umrIds);
            IList<Photos> photoss = this._photosRepository.Export(umrIds);
            IList<Points> pointss = this._pointsRepository.Export(umrIds);
            IList<Samples> sampless = this._sampleRepository.Export(umrIds);
            IList<DigsituationBefore> digsituationBefores = this._digsituationBeforeRepository.Export(umrIds);
            IList<ImportAntsites> importAntsitess = this._importAntsitesRepository.Export(umrIds);
            IList<LayerDeposit> layerDeposits = this._layerDepositRepository.Export(umrIds);
            IList<Literature> literatures = this._literatureRepository.Export(umrIds);
            IList<Audit> audits = this._auditRepository.Export(umrIds);

            //2.获取文件
            for (var i = 0; i < draftss.Count; i++)
            {
                if (!String.IsNullOrEmpty(draftss[i].FilePath))
                    files.Add(filePath + "/Content/Upload/" + draftss[i].FilePath);
            }

            for (var i = 0; i < otherss.Count; i++)
            {
                if (!String.IsNullOrEmpty(otherss[i].FilePath))
                    files.Add(filePath + "/Content/Upload/" + otherss[i].FilePath);
            }

            for (var i = 0; i < photoss.Count; i++)
            {
                if (!String.IsNullOrEmpty(photoss[i].FilePath))
                    files.Add(filePath + "/Content/Upload/" + photoss[i].FilePath);
            }

            for (var i = 0; i < importAntsitess.Count; i++)
            {
                if (!String.IsNullOrEmpty(importAntsitess[i].Path))
                    files.Add(filePath + "/Content/Upload/" + importAntsitess[i].Path);
            }

            for (var i = 0; i < literatures.Count; i++)
            {
                if (!String.IsNullOrEmpty(literatures[i].Path))
                    files.Add(filePath + "/Content/Upload/" + literatures[i].Path);
            }

            //3.导出数据到Excel表
            ExcelExport export = new ExcelExport()
                .AddSheet("Umrcover", umrcovers)
                .AddSheet("BasicPropertys", basicPropertys)
                .AddSheet("Draftss", draftss)
                .AddSheet("Otherss", otherss)
                .AddSheet("Photoss", photoss)
                .AddSheet("Pointss", pointss)
                .AddSheet("Sampless", sampless)
                .AddSheet("DigsituationBefores", digsituationBefores)
                .AddSheet("ImportAntsitess", importAntsitess)
                .AddSheet("LayerDeposits", layerDeposits)
                .AddSheet("Literatures", literatures)
                .AddSheet("Audits", audits);

            string userid = ((System.Web.Security.FormsIdentity)(System.Web.HttpContext.Current.User.Identity)).Ticket.UserData;
            string time = String.Format("{0}{1}{2}_{3}{4}{5}_{6}", DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day,
                DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second, DateTime.Now.Millisecond);
            var fileName = string.Format("BeyonDBTable_{0}_{1}.xls", userid, time);
            var tableFile = String.Format("{0}/Exports/{1}", filePath, fileName);
            try
            {
                export.ExportTo(tableFile, ExcelFormat.Excel2003);
                files.Add(tableFile);
            }
            catch (Exception ex)
            {
                _log.Error(ex);
                Console.WriteLine(ex.Message);
            }

            //压缩文件
            string tempName = DateTime.Now.ToString("yyyyMMddHHMMss") + DateTime.Now.Millisecond;
            string tempFolder = filePath + "\\Exports\\" + tempName;
            string zipFile = filePath + "\\Exports\\" + tempName + ".zip";

            Directory.CreateDirectory(tempFolder);
            for(int i = 0; i < files.Count; i++)
            {
                FileInfo file = new FileInfo(files[i]);
                if(file.Exists)
                {
                    string filename = file.Name;
                    if (!File.Exists(tempFolder + "/" + filename))
                        System.IO.File.Copy(files[i], tempFolder + "/" + filename, true);
                }
            }

            ZipFile.CreateFromDirectory(tempFolder, zipFile);

            //删除Excel文件
            DeleteFiles(tempFolder);
            System.IO.File.Delete(tableFile);

            return tempName + ".zip";
        }

        /// <summary>
        /// 删除临时目录下的所有文件
        /// </summary>
        /// <param name="tempPath">临时目录路径</param>
        private void DeleteFiles(string tempPath)
        {
            DirectoryInfo directory = new DirectoryInfo(tempPath);
            try
            {
                foreach (FileInfo file in directory.GetFiles())
                {
                    if (file.Attributes.ToString().IndexOf("ReadOnly") != -1)
                    {
                        file.Attributes = FileAttributes.Normal;
                    }
                    System.IO.File.Delete(file.FullName);
                }

                Directory.Delete(tempPath);
            }
            catch (Exception)
            {
                throw;
            }
        }

    }
}
