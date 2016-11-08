using System;
using System.Collections.Generic;
using System.Linq;
using BEYON.Component.Data.EF.Interface;
using BEYON.Component.Tools;
using BEYON.CoreBLL.Service.App.Interface;
using BEYON.Domain.Data.Repositories.App;
using BEYON.Domain.Model.App;
using BEYON.ViewModel.App;
using BEYON.CoreBLL.Service.Excel;
using EntityFramework.Extensions;
using System.Web.SessionState;
using System.Web;
using System.Text.RegularExpressions;


namespace BEYON.CoreBLL.Service.App
{
    public class PersonalRecordService : CoreServiceBase, IPersonalRecordService
    {
        private readonly log4net.ILog _log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly IPersonalRecordRepository _PersonalRecordRepository;
        private readonly ITaxPerOrderRepository _TaxPerOrderRepository;
        private readonly ITaxBaseByMonthRepository _TaxBaseByMonthRepository;



        public PersonalRecordService(IPersonalRecordRepository personalRecordRepository, ITaxPerOrderRepository taxPerOrderRepository, ITaxBaseByMonthRepository taxBaseByMonthRepository, IUnitOfWork unitOfWork)
            : base(unitOfWork)
        {
            this._PersonalRecordRepository = personalRecordRepository;
            this._TaxPerOrderRepository = taxPerOrderRepository;
            this._TaxBaseByMonthRepository = taxBaseByMonthRepository;
        }
        public IQueryable<PersonalRecord> PersonalRecords
        {
            get { return _PersonalRecordRepository.Entities; }
        }

        public OperationResult Insert(PersonalRecordVM model, bool isSave)
        {
            try
            {
                //1.检查是否有重复字段
                PersonalRecord[] personalRecords = _PersonalRecordRepository.Entities.Where(w => w.SerialNumber == model.SerialNumber && w.CertificateID == model.CertificateID.Trim()).ToArray();
                if (personalRecords != null && personalRecords.Length > 0)
                {
                    return new OperationResult(OperationResultType.Warning, "该申请单中已经存在相同的人员信息，请修改后重新提交！");
                }

                if (String.IsNullOrEmpty(model.CertificateID))
                    return new OperationResult(OperationResultType.Warning, "证件号码不能为空，请修改后重新提交！");

                var entity = new PersonalRecord
                {
                    SerialNumber = model.SerialNumber,
                    Name = model.Name,
                    CertificateID = model.CertificateID,
                    CertificateType = model.CertificateType,
                    Company = model.Company,
                    Tele = model.Tele,
                    PersonType = model.PersonType,
                    Nationality = model.Nationality,
                    Title = model.Title,
                    Amount = model.Amount,
                    TaxOrNot = model.TaxOrNot,
                    Bank = model.Bank,
                    BankDetailName = model.BankDetailName,
                    AccountName = model.AccountName,
                    AccountNumber = model.AccountNumber,
                    PaymentType = model.PaymentType,
                    UpdateDate = DateTime.Now
                };
                _PersonalRecordRepository.Insert(entity, isSave);

                return new OperationResult(OperationResultType.Success, "新增数据成功！", entity);
            }
            catch (Exception ex)
            {
                _log.Error(ex);
                return new OperationResult(OperationResultType.Error, "新增数据失败：" + ex.Message);
            }
        }
        public OperationResult Update(PersonalRecordVM model, bool isSave)
        {
            try
            {
                PersonalRecord personalRecord = _PersonalRecordRepository.Entities.FirstOrDefault(c => c.SerialNumber == model.SerialNumber && c.CertificateID == model.CertificateID.Trim());
                if (personalRecord == null)
                {
                    throw new Exception();
                }
                personalRecord.SerialNumber = model.SerialNumber;
                personalRecord.Name = model.Name;
                personalRecord.CertificateID = model.CertificateID;
                personalRecord.CertificateType = model.CertificateType;
                personalRecord.Company = model.Company;
                personalRecord.Tele = model.Tele;
                personalRecord.PersonType = model.PersonType;
                personalRecord.Nationality = model.Nationality;
                personalRecord.Title = model.Title;
                personalRecord.Amount = model.Amount;
                personalRecord.TaxOrNot = model.TaxOrNot;
                personalRecord.Bank = model.Bank;
                personalRecord.BankDetailName = model.BankDetailName;
                personalRecord.AccountName = model.AccountName;
                personalRecord.AccountNumber = model.AccountNumber;
                personalRecord.PaymentType = model.PaymentType;
                personalRecord.UpdateDate = DateTime.Now;
                _PersonalRecordRepository.Update(personalRecord, isSave);
                return new OperationResult(OperationResultType.Success, "更新数据成功！");
            }
            catch (Exception ex)
            {
                _log.Error(ex);
                return new OperationResult(OperationResultType.Error, "更新数据失败!");
            }
        }

        public OperationResult Delete(List<string> serialNumber, bool isSave)
        {
            try
            {
                if (serialNumber != null)
                {
                    int count = _PersonalRecordRepository.Delete(_PersonalRecordRepository.Entities.Where(c => serialNumber.Contains(c.SerialNumber)), isSave);
                    if (count > 0)
                    {
                        return new OperationResult(OperationResultType.Success, "删除数据成功！");
                    }
                    else
                    {
                        return new OperationResult(OperationResultType.Error, "删除数据失败!");
                    }
                }
                else
                {
                    return new OperationResult(OperationResultType.ParamError, "参数错误，请选择需要删除的数据!");
                }
            }
            catch (Exception ex)
            {
                _log.Error(ex);
                return new OperationResult(OperationResultType.Error, "删除数据失败!");
            }
        }
        public OperationResult Update(PersonalRecord model, bool isSave)
        {
            try
            {
                model.UpdateDate = DateTime.Now;
                _PersonalRecordRepository.Update(model, isSave);
                return new OperationResult(OperationResultType.Success, "更新数据成功！");
            }
            catch (Exception ex)
            {
                _log.Error(ex);
                return new OperationResult(OperationResultType.Error, "更新数据失败!");
            }
        }

        public OperationResult Delete(PersonalRecord model, bool isSave)
        {
            try
            {
                model.UpdateDate = DateTime.Now;
                _PersonalRecordRepository.Delete(model, isSave);
                return new OperationResult(OperationResultType.Success, "更新数据成功！");
            }
            catch (Exception ex)
            {
                _log.Error(ex);
                return new OperationResult(OperationResultType.Error, "更新数据失败!");
            }
        }

        public OperationResult Import(String fileName, Service.Excel.ImportData importData)
        {
            try
            {
                var columns = importData == null ? null : importData.Columns;
                var maps = GetColumns(columns, new PersonalRecord());
                var items = ExcelService.GetObjects(fileName, columns);
                if (importData != null)
                {
                    String serialNumber = importData.Parameters[0].Value;
                    String paymentType = importData.Parameters[1].Value;
                    int num = 1;
                    foreach (var item in items)
                    {
                        PersonalRecord record = new PersonalRecord();
                        record.SerialNumber = serialNumber;
                        record.PaymentType = paymentType;
                        List<ImportFeedBack> errors = ValidatePersonalRecord(item,  num++, maps, ref record);
                        if(errors.Count > 0)
                        {
                            return new OperationResult(OperationResultType.Error, "导入数据失败", ParseToHtml(errors));
                        }

                        //插入或更新数据
                        _PersonalRecordRepository.InsertOrUpdate(record);
                    }
                }

                return new OperationResult(OperationResultType.Success, "导入数据成功！");
            }

            catch (Exception ex)
            {
                _log.Error(ex);
                ImportFeedBack feedBack = new ImportFeedBack();
                feedBack.ExceptionType = "未知错误";
                feedBack.ExceptionContent.Add(ex.Message);
                List<ImportFeedBack> erros = new List<ImportFeedBack>();
                return new OperationResult(OperationResultType.Error, "导入数据失败!", ParseToHtml(new List<ImportFeedBack>(){feedBack}));
            }
        }

        private Dictionary<String, String> GetColumns(ColumnMap[] colums, PersonalRecord record)
        {
            Dictionary<String, String> result = new Dictionary<String, String>();
            if (colums != null)
            {
                foreach (var column in colums)
                {
                    result.Add(column.ColumnName, column.TitleName);
                }
            }
            else
            {
                var properties = record.GetType().GetProperties();
                foreach (var property in properties)
                {
                    result.Add(property.Name, property.Name);
                }
            }
            
            return result;
        }

        private String ParseToHtml(List<ImportFeedBack> feedbacks)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            foreach (ImportFeedBack feedback in feedbacks)
            {
                sb.Append("<li>");
                sb.Append(String.Format("错误类型[ {0} ]", feedback.ExceptionType));
                sb.Append("</li>");
                if(feedback.ExceptionContent.Count > 0)
                {
                    sb.Append("<li>详细信息错误如下: </li>");
                    foreach(var error in feedback.ExceptionContent)
                    {
                        sb.Append("<li>");
                        sb.Append(error);
                        sb.Append("</li>");
                    }
                }
            }

            return sb.ToString();
        }

        private String GetValue(LinqToExcel.Row record, Dictionary<String, String> map, String name)
        {
            if(map.ContainsKey(name))
            {
                return record[map[name]];
            }
            else
            {
                return record[name];
            }
        }

        private List<ImportFeedBack> ValidatePersonalRecord(LinqToExcel.Row record, int num, Dictionary<String, String> map, ref PersonalRecord personal)
        {
            List<ImportFeedBack> list = new List<ImportFeedBack>();
            ImportFeedBack feedBack = null;
            //非空验证
            feedBack = new ImportFeedBack();
            feedBack.ExceptionType = "空值";
            if (personal.PaymentType.Equals("银行转账"))
            {
                var properties = personal.GetType().GetProperties();
                foreach (var property in properties)
                {
                    switch (property.Name)
                    {
                        case "Name":
                        case "CertificateType":
                        case "CertificateID":
                        case "Company":
                        case "Tele":
                        case "PersonType":
                        case "Nationality":
                        case "Title":
                        case "TaxOrNot":
                        case "AccountNumber":
                            if (String.IsNullOrEmpty(GetValue(record, map, property.Name)))
                            {
                                feedBack.ExceptionContent.Add(String.Format("第{0}行记录  {1}为空！", num, map[property.Name]));
                            }
                            else
                            {
                                property.SetValue(personal, GetValue(record, map, property.Name));
                            }
                            break;
                        case "BankDetailName":
                            if (!GetValue(record, map, "Bank").Equals("工商银行"))
                            {
                                if (String.IsNullOrEmpty(GetValue(record, map, property.Name)))
                                {
                                    feedBack.ExceptionContent.Add(String.Format("第{0}行记录  {1}为空！", num, map[property.Name]));
                                }
                                else
                                {
                                    property.SetValue(personal, GetValue(record, map, property.Name));
                                }
                            }
                            break;
                        case "Amount":
                            try
                            {
                                var value = Convert.ToDouble(GetValue(record, map, property.Name));
                                property.SetValue(personal, value);
                            }
                            catch
                            {
                                feedBack.ExceptionContent.Add(String.Format("第{0}行记录  {1}格式不正确！", num, map[property.Name]));
                            }
                            break;
                        default:
                            break;
                    }
                }
            }
            else if (personal.PaymentType.Equals("现金支付"))
            {
                var properties = personal.GetType().GetProperties();
                foreach (var property in properties)
                {
                    switch(property.Name)
                    {
                        case "Name":
                        case "CertificateType":
                        case "CertificateID":
                        case "Company":
                        case "Tele":
                        case "PersonType":
                        case "Nationality":
                        case "Title":
                        case "TaxOrNot":
                            if (String.IsNullOrEmpty(GetValue(record, map, property.Name)))
                            {
                                feedBack.ExceptionContent.Add(String.Format("第{0}行记录  {1}为空！", num, map[property.Name]));
                            }
                            else
                            {
                                property.SetValue(personal, GetValue(record, map, property.Name));
                            }
                            break;
                        case "Amount":
                            if (String.IsNullOrEmpty(GetValue(record, map, property.Name)))
                            {
                                feedBack.ExceptionContent.Add(String.Format("第{0}行记录  {1}为空！", num, map[property.Name]));
                            }
                            else
                            {
                                try
                                {
                                    var value = Convert.ToDouble(GetValue(record, map, property.Name));
                                    property.SetValue(personal, value);
                                }
                                catch
                                {
                                    feedBack.ExceptionContent.Add(String.Format("第{0}行记录  {1}格式不正确！", num, map[property.Name]));
                                }
                            }
                            break;
                        default:
                            break;
                    }
     
                }
            }
            if (feedBack.ExceptionContent.Count > 0)
            {
                list.Add(feedBack);
            }

            //格式错误验证
            feedBack = new ImportFeedBack();
            feedBack.ExceptionType = "格式错误";
            //证件类型
            String[] regCerficateType = { "居民身份证", "中国护照", "外国护照", "军官证", "士兵证", "武警警官证", "港澳居民来往内地通行证", "台湾居民来往大陆通行证", "香港身份证", "台湾身份证", "澳门身份证", "外国人永久居留证" };
            if (!regCerficateType.Contains(personal.CertificateType))
            {
                feedBack.ExceptionContent.Add("第" + num + "行记录  证件类型格式有误，必须是【居民身份证、中国护照、外国护照、军官证、士兵证、武警警官证、港澳居民来往内地通行证、台湾居民来往大陆通行证、香港身份证、台湾身份证、澳门身份证、外国人永久居留证】之一！");
            }
            //人员类型
            String[] regPersonType = { "所内", "所外" };
            if (!regPersonType.Contains(personal.PersonType))
            {
                feedBack.ExceptionContent.Add("第" + num + "行记录  人员类型格式有误，必须是【所内、所外】之一！");
            }
            //是否含税
            String[] regTaxOrNot = { "含税", "不含税" };
            if (!regTaxOrNot.Contains(personal.TaxOrNot))
            {
                feedBack.ExceptionContent.Add("第" + num + "行记录  是否含税格式有误，必须是【含税、不含税】之一！");
            }
            //证件号码
            Regex regCerficateID = new Regex("^\\d{15}|\\d{18}$");
            if (!regCerficateID.IsMatch(personal.CertificateID))
            {
                feedBack.ExceptionContent.Add("第" + num + "行记录  证件号码格式有误！");
            }
            //联系电话
            Regex regPhone = new Regex("/^0\\d{2,3}-?\\d{7,8}$/");
            Regex regMobile = new Regex("/^1[34578]\\d{9}$/");
            if (!regPhone.IsMatch(personal.Tele) && !regMobile.IsMatch(personal.Tele))
            {
                feedBack.ExceptionContent.Add("第" + num + "行记录  联系电话格式有误！");
            }
            //银行卡号
            Regex regAccountNumber = new Regex(" /^(\\d{4}[\\s\\-]?){4,5}\\d{3}$/g");
            if (personal.PaymentType.Equals("银行转账"))
            {
                if (!regAccountNumber.IsMatch(personal.AccountNumber))
                {
                    feedBack.ExceptionContent.Add("第" + num + "行记录  银行卡号格式有误！");
                }
            }
            if (feedBack.ExceptionContent.Count > 0)
            {
                list.Add(feedBack);
            }
            //若为所内：姓名、身份证验证
            feedBack = new ImportFeedBack();
            feedBack.ExceptionType = "所内所外姓名验证";
            String name = _TaxBaseByMonthRepository.GetNameByCerID(personal.CertificateID);
            if (name.Equals(""))
            {
                if (personal.PersonType.Equals("所内") && name.Equals(""))
                {
                    feedBack.ExceptionContent.Add("第" + num + "行记录  该人员不是所内人员，人员类型需要填写为【所外】！");
                }
            }
            else
            {
                if (personal.PersonType.Equals("所内") && !name.Equals(personal.Name))
                {
                    feedBack.ExceptionContent.Add("第" + num + "行记录  该该证件号码与人员不符，请检查！");
                }
                else {
                    if (personal.PersonType.Equals("所外"))
                    {
                        feedBack.ExceptionContent.Add("第" + num + "行记录  该人员是所内人员，人员类型需要填写为【所内】！");
                    }
                }
            }

            if (feedBack.ExceptionContent.Count > 0)
            {
                list.Add(feedBack);
            }

            //现金发放次数验证
            feedBack = new ImportFeedBack();
            feedBack.ExceptionType = "现金发放次数超过3次";
            int count = _TaxPerOrderRepository.GetCashCount(personal.CertificateID);
            if (count >= 3)
            {
                feedBack.ExceptionContent.Add("第" + num + "行记录  该人员本月已发放3次现金，若需再次发放，需在支付方式为【银行转账】的申请单中填报！");
            }
            if (feedBack.ExceptionContent.Count > 0)
            {
                list.Add(feedBack);
            }
            return list;
        }

        public class ImportFeedBack
        {
            public ImportFeedBack()
            {
                this.ExceptionContent = new List<String>();
            }

            public String ExceptionType { get; set; }
            public List<String> ExceptionContent { get; private set; }

        }
    }
}
