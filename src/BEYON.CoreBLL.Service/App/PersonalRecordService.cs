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
                var items = ExcelService.GetObjects<PersonalRecord>(fileName, columns);
                if (importData != null)
                {
                    String serialNumber = importData.Parameters[0].Value;
                    String paymentType = importData.Parameters[1].Value;
                    foreach (var item in items)
                    {
                        item.SerialNumber = serialNumber;
                        item.PaymentType = paymentType;
                    }
                }
                _PersonalRecordRepository.InsertOrUpdate(items);
                return new OperationResult(OperationResultType.Success, "导入数据成功！");
            }

            catch (Exception ex)
            {
                _log.Error(ex);
                return new OperationResult(OperationResultType.Error, "导入数据失败!");
            }
        }

        public List<ImportFeedBack> ValidatePersonalRecord(PersonalRecord record, int num)
        {
            List<ImportFeedBack> list = new List<ImportFeedBack>();
            ImportFeedBack feedBack = null;
            //非空验证
            feedBack = new ImportFeedBack();
            feedBack.ExceptionType = "空值";
            if (record.PaymentType.Equals("银行转账"))
            {
                if (record.Name.Equals("") || record.Name == null)
                {
                    feedBack.ExceptionContent.Add("第" + num + "行记录  姓名为空！");
                }
                if (record.CertificateType.Equals("") || record.CertificateType == null)
                {
                    feedBack.ExceptionContent.Add("第" + num + "行记录  证件类型为空！");
                }
                if (record.CertificateID.Equals("") || record.CertificateID == null)
                {
                    feedBack.ExceptionContent.Add("第" + num + "行记录  证件号码为空！");
                }
                if (record.Company.Equals("") || record.Company == null)
                {
                    feedBack.ExceptionContent.Add("第" + num + "行记录  单位为空！");
                }
                if (record.Tele.Equals("") || record.Tele == null)
                {
                    feedBack.ExceptionContent.Add("第" + num + "行记录  电话号码为空！");
                }
                if (record.PersonType.Equals("") || record.PersonType == null)
                {
                    feedBack.ExceptionContent.Add("第" + num + "行记录  人员类型为空！");
                }
                if (record.Nationality.Equals("") || record.Nationality == null)
                {
                    feedBack.ExceptionContent.Add("第" + num + "行记录  国籍为空！");
                }
                if (record.Title.Equals("") || record.Title == null)
                {
                    feedBack.ExceptionContent.Add("第" + num + "行记录  职称为空！");
                }
                if (record.TaxOrNot.Equals("") || record.TaxOrNot == null)
                {
                    feedBack.ExceptionContent.Add("第" + num + "行记录  是否含税为空！");
                }
                if (record.Amount.Equals("") || record.Amount == null)
                {
                    feedBack.ExceptionContent.Add("第" + num + "行记录  金额为空！");
                }
                if (record.Bank.Equals("") || record.Bank == null)
                {
                    feedBack.ExceptionContent.Add("第" + num + "行记录  开户银行为空！");
                }
                if (record.AccountNumber.Equals("") || record.AccountNumber == null)
                {
                    feedBack.ExceptionContent.Add("第" + num + "行记录  银行账号为空！");
                }
                if (!record.Bank.Equals("工商银行"))
                {
                    if (record.BankDetailName.Equals("") || record.BankDetailName == null)
                    {
                        feedBack.ExceptionContent.Add("第" + num + "行记录  开户行详细名称为空！");
                    }
                }
            }
            else if (record.PaymentType.Equals("现金支付"))
            {
                if (record.Name.Equals("") || record.Name == null)
                {
                    feedBack.ExceptionContent.Add("第" + num + "行记录  姓名为空！");
                }
                if (record.CertificateType.Equals("") || record.CertificateType == null)
                {
                    feedBack.ExceptionContent.Add("第" + num + "行记录  证件类型为空！");
                }
                if (record.CertificateID.Equals("") || record.CertificateID == null)
                {
                    feedBack.ExceptionContent.Add("第" + num + "行记录  证件号码为空！");
                }
                if (record.Company.Equals("") || record.Company == null)
                {
                    feedBack.ExceptionContent.Add("第" + num + "行记录  单位为空！");
                }
                if (record.Tele.Equals("") || record.Tele == null)
                {
                    feedBack.ExceptionContent.Add("第" + num + "行记录  电话号码为空！");
                }
                if (record.PersonType.Equals("") || record.PersonType == null)
                {
                    feedBack.ExceptionContent.Add("第" + num + "行记录  人员类型为空！");
                }
                if (record.Nationality.Equals("") || record.Nationality == null)
                {
                    feedBack.ExceptionContent.Add("第" + num + "行记录  国籍为空！");
                }
                if (record.Title.Equals("") || record.Title == null)
                {
                    feedBack.ExceptionContent.Add("第" + num + "行记录  职称为空！");
                }
                if (record.TaxOrNot.Equals("") || record.TaxOrNot == null)
                {
                    feedBack.ExceptionContent.Add("第" + num + "行记录  是否含税为空！");
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
            if (!regCerficateType.Contains(record.CertificateType))
            {
                feedBack.ExceptionContent.Add("第" + num + "行记录  证件类型格式有误，必须是【居民身份证、中国护照、外国护照、军官证、士兵证、武警警官证、港澳居民来往内地通行证、台湾居民来往大陆通行证、香港身份证、台湾身份证、澳门身份证、外国人永久居留证】之一！");
            }
            //人员类型
            String[] regPersonType = { "所内", "所外" };
            if (!regPersonType.Contains(record.PersonType))
            {
                feedBack.ExceptionContent.Add("第" + num + "行记录  人员类型格式有误，必须是【所内、所外】之一！");
            }
            //是否含税
            String[] regTaxOrNot = { "含税", "不含税" };
            if (!regTaxOrNot.Contains(record.TaxOrNot))
            {
                feedBack.ExceptionContent.Add("第" + num + "行记录  是否含税格式有误，必须是【含税、不含税】之一！");
            }
            //证件号码
            Regex regCerficateID = new Regex("^\\d{15}|\\d{18}$");
            if (!regCerficateID.IsMatch(record.CertificateID))
            {
                feedBack.ExceptionContent.Add("第" + num + "行记录  证件号码格式有误！");
            }
            //联系电话
            Regex regPhone = new Regex("/^0\\d{2,3}-?\\d{7,8}$/");
            Regex regMobile = new Regex("/^1[34578]\\d{9}$/");
            if (!regPhone.IsMatch(record.Tele) && !regMobile.IsMatch(record.Tele))
            {
                feedBack.ExceptionContent.Add("第" + num + "行记录  联系电话格式有误！");
            }
            //银行卡号
            Regex regAccountNumber = new Regex(" /^(\\d{4}[\\s\\-]?){4,5}\\d{3}$/g");
            if (record.PaymentType.Equals("银行转账"))
            {
                if (!regAccountNumber.IsMatch(record.AccountNumber))
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
            String name = _TaxBaseByMonthRepository.GetNameByCerID(record.CertificateID);
            if (name.Equals(""))
            {
                if (record.PersonType.Equals("所内") && name.Equals(""))
                {
                    feedBack.ExceptionContent.Add("第" + num + "行记录  该人员不是所内人员，人员类型需要填写为【所外】！");
                }
            }
            else
            {
                if (record.PersonType.Equals("所内") && !name.Equals(record.Name))
                {
                    feedBack.ExceptionContent.Add("第" + num + "行记录  该该证件号码与人员不符，请检查！");
                }
                else {
                    if (record.PersonType.Equals("所外")) {
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
            int count = _TaxPerOrderRepository.GetCashCount(record.CertificateID);
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
            public String ExceptionType { get; set; }
            public List<String> ExceptionContent { get; set; }

        }
    }
}
