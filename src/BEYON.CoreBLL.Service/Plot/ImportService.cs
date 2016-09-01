using System;
using System.IO;
using System.Linq;
using System.Text;
using System.IO.Compression;
using System.Collections.Generic;
using System.Threading.Tasks;
using ExcelExporter;
using LinqToExcel;
using LinqToExcel.Query;
using LinqToExcel.Domain;
using LinqToExcel.Extensions;
using BEYON.Domain.Model.Plot;
//using BEYON.CoreBLL.Service.Excel;
using BEYON.CoreBLL.Service.Plot.Interface;
using BEYON.Domain.Data.Repositories.Plot;

namespace BEYON.CoreBLL.Service.Plot
{
    public class ImportService :  IImportService
    {
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

        public ImportService(IUmrcoverRepository umrcoverRepository, IBasicPropertyRepository basicPropertyRepository,
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

        public void Import(string zipFile, string filePath)
        {
            //1.解压文件
            string tempName = DateTime.Now.ToString("yyyyMMddHHMMss") + DateTime.Now.Millisecond;
            string tempFolder = filePath + "\\Imports\\" + tempName;
            Directory.CreateDirectory(tempFolder);
            ZipFile.ExtractToDirectory(zipFile, tempFolder);

            //2.判断数据库表文件，并更新数据库,如果不是拷贝到Content/Update文件夹下
            String destinaPath = filePath + "Content\\Upload";
            if (!Directory.Exists(destinaPath))
                Directory.CreateDirectory(destinaPath);

            DirectoryInfo directory = new DirectoryInfo(tempFolder);
            foreach (FileInfo file in directory.GetFiles())
            {
                if((file.Extension == ".xlsx" || file.Extension == ".xls") && file.Name.Contains("BeyonDBTable"))
                {
                    ImportExcel(file.FullName);
                }
                else
                {
                    String destName = String.Format("{0}\\{1}", destinaPath, file.Name);
                    if (!File.Exists(destName))
                        File.Copy(file.FullName, destName, true);
                }
            }

            //3.删除所有临时文件
            DeleteFiles(tempFolder);
            System.IO.File.Delete(zipFile);
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

        private void ImportExcel(string filePath)
        {
            //读取Excel文件并保持数据
            var excel = new ExcelQueryFactory(filePath);
            var Umrcovers = from c in excel.Worksheet<Umrcover>("Umrcover") select c;
            var BasicPropertys = from c in excel.Worksheet<BasicProperty>("BasicPropertys") select c;
            var Draftss = from c in excel.Worksheet<Drafts>("Draftss") select c;
            var Otherss = from c in excel.Worksheet<Others>("Otherss") select c;
            var Photoss = from c in excel.Worksheet<Photos>("Photoss") select c;
            var Pointss = from c in excel.Worksheet<Points>("Pointss") select c;
            var Sampless = from c in excel.Worksheet<Samples>("Sampless") select c;
            var DigsituationBefores = from c in excel.Worksheet<DigsituationBefore>("DigsituationBefores") select c;
            var ImportAntsitess = from c in excel.Worksheet<ImportAntsites>("ImportAntsitess") select c;
            var LayerDeposits = from c in excel.Worksheet<LayerDeposit>("LayerDeposits") select c;
            var Literatures = from c in excel.Worksheet<Literature>("Literatures") select c;
            var Audits = from c in excel.Worksheet<Audit>("Audits") select c;

            this._umrcoverRepository.InsertOrUpdate(Umrcovers);
            this._basicPropertyRepository.InsertOrUpdate(BasicPropertys);
            this._draftsRepository.InsertOrUpdate(Draftss);
            this._othersRepository.InsertOrUpdate(Otherss);
            this._photosRepository.InsertOrUpdate(Photoss);
            this._pointsRepository.InsertOrUpdate(Pointss);
            this._sampleRepository.InsertOrUpdate(Sampless);
            this._digsituationBeforeRepository.InsertOrUpdate(DigsituationBefores);
            this._importAntsitesRepository.InsertOrUpdate(ImportAntsitess);
            this._layerDepositRepository.InsertOrUpdate(LayerDeposits);
            this._literatureRepository.InsertOrUpdate(Literatures);
            this._auditRepository.InsertOrUpdate(Audits);
        }
    }
}
