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
        private readonly ITopContactsService _TopContactsService;
        private readonly ITaxPerOrderRepository _TaxPerOrderRepository;
        private readonly ITaxBaseByMonthRepository _TaxBaseByMonthRepository;



        public PersonalRecordService(IPersonalRecordRepository personalRecordRepository, ITaxPerOrderRepository taxPerOrderRepository, ITaxBaseByMonthRepository taxBaseByMonthRepository,ITopContactsService topContactsService, IUnitOfWork unitOfWork)
            : base(unitOfWork)
        {
            this._PersonalRecordRepository = personalRecordRepository;
            this._TaxPerOrderRepository = taxPerOrderRepository;
            this._TaxBaseByMonthRepository = taxBaseByMonthRepository;
            this._TopContactsService = topContactsService;
        }
        public IQueryable<PersonalRecord> PersonalRecords
        {
            get { return _PersonalRecordRepository.Entities; }
        }

        public OperationResult Insert(PersonalRecordVM model, bool isSave)
        {
            try
            {
                string userid = ((System.Web.Security.FormsIdentity)(System.Web.HttpContext.Current.User.Identity)).Ticket.UserData;
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
                    Name = model.Name.Trim().Replace("\n", "").Replace("\t", "").Replace("\r", ""),
                    CertificateID = GetReplaceString(model.CertificateID),
                    CertificateType = GetReplaceString(model.CertificateType),
                    Company = GetReplaceString(model.Company),
                    Tele = GetReplaceString(model.Tele),
                    PersonType = GetReplaceString(model.PersonType),
                    Nationality = GetReplaceString(model.Nationality),
                    Title = GetReplaceString(model.Title),
                    Amount = model.Amount,
                    TaxOrNot = GetReplaceString(model.TaxOrNot),
                    Bank = GetReplaceString(model.Bank),
                    BankDetailName = GetReplaceString(model.BankDetailName),
                    AccountName = GetReplaceString(model.AccountName),
                    AccountNumber = GetReplaceString(model.AccountNumber),
                    PaymentType = GetReplaceString(model.PaymentType),
                    UpdateDate = DateTime.Now
                };
                _PersonalRecordRepository.Insert(entity, isSave);

                //保存时同步向常用领款人表TopContacts中保存
                TopContacts contact = new TopContacts();
                contact.UserID = userid;
                contact.Name = GetReplaceString(model.Name);
                contact.CertificateType = GetReplaceString(model.CertificateType);
                contact.CertificateID = GetReplaceString(model.CertificateID);
                contact.Nationality = GetReplaceString(model.Nationality);
                contact.PersonType = GetReplaceString(model.PersonType);
                contact.Tele = GetReplaceString(model.Tele);
                contact.Title = GetReplaceString(model.Title);
                contact.Company = GetReplaceString(model.Company);
                if (!String.IsNullOrEmpty(model.Bank))
                {
                    contact.Bank = GetReplaceString(model.Bank);
                }
                if (!String.IsNullOrEmpty(model.BankDetailName))
                {
                    contact.BankDetailName = GetReplaceString(model.BankDetailName);
                }
                if (!String.IsNullOrEmpty(model.AccountNumber))
                {
                    contact.AccountNumber = GetReplaceString(model.AccountNumber);
                }
                _TopContactsService.Insert(contact);    

                return new OperationResult(OperationResultType.Success, "新增数据成功！", entity);
            }
            catch (Exception ex)
            {
                _log.Error(ex);
                return new OperationResult(OperationResultType.Error, "新增数据失败：" + ex.Message);
            }
        }

        private String GetReplaceString(String modelString) { 
            if(String.IsNullOrEmpty(modelString)){
                return modelString;
            }else{
                return modelString.Trim().Replace("\n", "").Replace(" ", "").Replace("\t", "").Replace("\r", "");
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
                personalRecord.Name = model.Name.Trim().Replace("\n", "").Replace("\t", "").Replace("\r", "");
                personalRecord.CertificateID = GetReplaceString(model.CertificateID);
                personalRecord.CertificateType = GetReplaceString(model.CertificateType);
                personalRecord.Company = GetReplaceString(model.Company);
                personalRecord.Tele = GetReplaceString(model.Tele);
                personalRecord.PersonType = GetReplaceString(model.PersonType);
                personalRecord.Nationality = GetReplaceString(model.Nationality);
                personalRecord.Title = GetReplaceString(model.Title);
                personalRecord.Amount = model.Amount;
                personalRecord.TaxOrNot = GetReplaceString(model.TaxOrNot);
                personalRecord.Bank = GetReplaceString(model.Bank);
                personalRecord.BankDetailName = GetReplaceString(model.BankDetailName);
                personalRecord.AccountName = GetReplaceString(model.AccountName);
                personalRecord.AccountNumber = GetReplaceString(model.AccountNumber);
                personalRecord.PaymentType = GetReplaceString(model.PaymentType);
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

        public OperationResult DeleteModel(List<PersonalRecord> list , bool isSave)
        {
            try
            {
                if (list.Count > 0)
                {
                    foreach(var item in list){
                        _PersonalRecordRepository.Delete(item);
                    }
                }                                 
                return new OperationResult(OperationResultType.Success, "删除数据成功！");              
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
                string userid = ((System.Web.Security.FormsIdentity)(System.Web.HttpContext.Current.User.Identity)).Ticket.UserData;
                
                var columns = importData == null ? null : importData.Columns;
                var maps = ImportUtil.GetColumns(columns, new PersonalRecord());
                var items = ExcelService.GetObjects(fileName, columns);
                if (importData != null)
                {
                    String serialNumber = importData.Parameters[0].Value;
                    String paymentType = importData.Parameters[1].Value;
                    int num = 1;
                    foreach (var item in items)
                    {
                        if (CheckAllNull(item[0]) && CheckAllNull(item[1]) && CheckAllNull(item[2]) && CheckAllNull(item[3]) && CheckAllNull(item[4]) && CheckAllNull(item[5]) && CheckAllNull(item[6]) && CheckAllNull(item[7]) && CheckAllNull(item[8]))
                        {
                            continue;
                        }
                        PersonalRecord record = new PersonalRecord();
                        record.SerialNumber = serialNumber;
                        record.PaymentType = paymentType;
                        if (paymentType.Equals("银行转账")) {
                            if (item.Count < 13)
                            {
                                return new OperationResult(OperationResultType.Error, "导入数据失败", "<li>请增加银行账户等相关信息或修改为现金支付类型</li>");
                            }

                            record.AccountName = item[0];
                            record.Bank = item[10];
                            record.AccountNumber = item[11];
                            record.BankDetailName = item[12];
                        }
                        
                        List<ImportFeedBack> errors = ValidatePersonalRecord(item,  num++, maps, ref record);
                        if(errors.Count > 0)
                        {
                            return new OperationResult(OperationResultType.Error, "导入数据失败", ImportUtil.ParseToHtml(errors));
                        }

                        //插入或更新数据
                        _PersonalRecordRepository.InsertOrUpdate(record);

                        //保存时同步向常用领款人表TopContacts中保存
                        TopContacts contact = new TopContacts();
                        contact.UserID = userid;
                        contact.Name = GetReplaceString(record.Name);
                        contact.CertificateType = GetReplaceString(record.CertificateType);
                        contact.CertificateID = GetReplaceString(record.CertificateID);
                        contact.Nationality = GetReplaceString(record.Nationality);
                        contact.PersonType = GetReplaceString(record.PersonType);
                        contact.Tele = GetReplaceString(record.Tele);
                        contact.Title = GetReplaceString(record.Title);
                        contact.Company = GetReplaceString(record.Company);
                        if (!String.IsNullOrEmpty(record.Bank))
                        {
                            contact.Bank = GetReplaceString(record.Bank);
                        }
                        if (!String.IsNullOrEmpty(record.BankDetailName))
                        {
                            contact.BankDetailName = GetReplaceString(record.BankDetailName);
                        }
                        if (!String.IsNullOrEmpty(record.AccountNumber))
                        {
                            contact.AccountNumber = GetReplaceString(record.AccountNumber);
                        }
                        _TopContactsService.Insert(contact);    
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
                return new OperationResult(OperationResultType.Error, "导入数据失败!", ImportUtil.ParseToHtml(new List<ImportFeedBack>() { feedBack }));
            }
        }

        private Boolean CheckAllNull(String item)
        {
            if (String.IsNullOrEmpty(item))
            {
                return true;
            }
            else
            {
                return false;
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
                            if (String.IsNullOrEmpty(ImportUtil.GetValue(record, map, property.Name)))
                            {
                                feedBack.ExceptionContent.Add(String.Format("第{0}行记录  {1}为空！", num, map[property.Name]));
                            }
                            else
                            {
                                property.SetValue(personal, ImportUtil.GetValue(record, map, property.Name));
                            }
                            break;
                        case "BankDetailName":
                            if (!ImportUtil.GetValue(record, map, "Bank").Equals("工商银行"))
                            {
                                if (String.IsNullOrEmpty(ImportUtil.GetValue(record, map, property.Name)))
                                {
                                    feedBack.ExceptionContent.Add(String.Format("第{0}行记录  {1}为空！", num, map[property.Name]));
                                }
                                else
                                {
                                    property.SetValue(personal, ImportUtil.GetValue(record, map, property.Name));
                                }
                            }
                            break;
                        case "Amount":
                            try
                            {
                                var value = Convert.ToDouble(ImportUtil.GetValue(record, map, property.Name));
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
                            if (String.IsNullOrEmpty(ImportUtil.GetValue(record, map, property.Name)))
                            {
                                feedBack.ExceptionContent.Add(String.Format("第{0}行记录  {1}为空！", num, map[property.Name]));
                            }
                            else
                            {
                                property.SetValue(personal, ImportUtil.GetValue(record, map, property.Name));
                            }
                            break;
                        case "Amount":
                            if (String.IsNullOrEmpty(ImportUtil.GetValue(record, map, property.Name)))
                            {
                                feedBack.ExceptionContent.Add(String.Format("第{0}行记录  {1}为空！", num, map[property.Name]));
                            }
                            else
                            {
                                try
                                {
                                    var value = Convert.ToDouble(ImportUtil.GetValue(record, map, property.Name));
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
           // Regex regCerficateID = new Regex("^\\d{15}|\\d{18}$");
            //Regex regCerficateID = new Regex("^[1-9]\\d{5}[1-9]9\\d{4}3[0-1]\\d{4}$|^[1-9]\\d{5}[1-9]9\\d{4}[0-2][0-9]\\d{4}$|^[1-9]\\d{5}[1-9]9\\d{4}3[0-1]\\d{3}X$|^[1-9]\\d{5}[1-9]9\\d{4}[0-2][0-9]\\d{3}X$|^[1-9]\\d{5}\\d{4}3[0-1]\\d{4}$|^[1-9]\\d{5}\\d{4}[0-2][0-9]\\d{4}$|^[1-9]\\d{5}\\d{4}3[0-1]\\d{3}X$|^[1-9]\\d{5}\\d{4}[0-2][0-9]\\d{3}X$");
            //^[1-9]\d{5}[1-9]9\d{4}3[0-1]\d{4}$|^[1-9]\d{5}[1-9]9\d{4}[0-2][0-9]\d{4}$|^[1-9]\d{5}[1-9]9\d{4}3[0-1]\d{3}X$|^[1-9]\d{5}[1-9]9\d{4}[0-2][0-9]\d{3}X$|^[1-9]\d{5}\d{4}3[0-1]\d{4}$|^[1-9]\d{5}\d{4}[0-2][0-9]\d{4}$|^[1-9]\d{5}\d{4}3[0-1]\d{3}X$|^[1-9]\d{5}\d{4}[0-2][0-9]\d{3}X$

            if (!String.IsNullOrEmpty(personal.CertificateType) && personal.CertificateType.Equals("居民身份证"))
            {
                if (personal.CertificateID.Length == 18) {
                    if (CheckIDCard18(personal.CertificateID) == false && CheckIDCardFormat(personal.CertificateID) == false)
                    {
                        feedBack.ExceptionContent.Add("第" + num + "行记录  证件号码格式有误！");
                    }
                }
                else if (personal.CertificateID.Length == 15)
                {
                    if (CheckIDCard15(personal.CertificateID) == false && CheckIDCardFormat(personal.CertificateID) == false)
                    {
                        feedBack.ExceptionContent.Add("第" + num + "行记录  证件号码格式有误！");
                    }
                }
            }
            //联系电话
            Regex regPhone = new Regex("^0\\d{2,3}-?\\d{7,8}$");
            Regex regMobile = new Regex("^1[34578]\\d{9}$");
           // if (!regPhone.IsMatch(personal.Tele) && !regMobile.IsMatch(personal.Tele))
            if (String.IsNullOrEmpty(personal.Tele) || System.Text.RegularExpressions.Regex.IsMatch(personal.Tele, @"^(\d{3,4}-)?\d{6,8}$"))
            {
                feedBack.ExceptionContent.Add("第" + num + "行记录  联系电话格式有误！若为座机请填写区号，如01082306380");
            }
            //银行卡号
            Regex regAccountNumber1 = new Regex("^(\\d{4}[\\s\\-]?){4,5}\\d{3}$");
            Regex regAccountNumber2 = new Regex("^(\\d{16}|\\d{17}|\\d{18}|\\d{19}|\\d{20}|\\d{21})$");
            if (!String.IsNullOrEmpty(personal.PaymentType) && personal.PaymentType.Equals("银行转账"))
            {
                if (String.IsNullOrEmpty(personal.AccountNumber) || !regAccountNumber1.IsMatch(personal.AccountNumber))
                {
                    if (String.IsNullOrEmpty(personal.AccountNumber) || !regAccountNumber2.IsMatch(personal.AccountNumber))
                    {
                        feedBack.ExceptionContent.Add("第" + num + "行记录  银行卡号格式有误！");
                    }
                }
            }
            if (feedBack.ExceptionContent.Count > 0)
            {
                list.Add(feedBack);
            }
            //若为所内：姓名、身份证验证
            feedBack = new ImportFeedBack();
            feedBack.ExceptionType = "所内所外姓名验证";
            String nameAndStatus = _TaxBaseByMonthRepository.GetNameByCerID(personal.CertificateID);
            string[] names = nameAndStatus.Split(',');
            if (names[1].Equals("upper"))
            {
                feedBack.ExceptionContent.Add("第" + num + "行记录  该人员证件号码中的字母需从小写修改为大写！");
            }
            else if (names[1].Equals("lower"))
            {
                feedBack.ExceptionContent.Add("第" + num + "行记录  该人员证件号码中的字母需从大写修改为小写！");
            }
            if (String.IsNullOrEmpty(names[0]) && !String.IsNullOrEmpty(personal.PersonType))
            {
                if (personal.PersonType.Equals("所内") && names[0].Equals(""))
                {
                    feedBack.ExceptionContent.Add("第" + num + "行记录  该人员不是所内人员，人员类型需要填写为【所外】！");
                }
            }
            else
            {
                if (!String.IsNullOrEmpty(personal.PersonType))
                {
                    if (personal.PersonType.Equals("所内") && !names[0].Equals(personal.Name.Trim()))
                    {
                        feedBack.ExceptionContent.Add("第" + num + "行记录  该证件号码与人员不符，请检查！");
                    }
                    else
                    {
                        if (personal.PersonType.Equals("所外"))
                        {
                            feedBack.ExceptionContent.Add("第" + num + "行记录  该人员是所内人员，人员类型需要填写为【所内】！");
                        }
                    }
                }
            }

            if (feedBack.ExceptionContent.Count > 0)
            {
                list.Add(feedBack);
            }

            //现金发放次数验证
            /*
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
             * */
            return list;
        }

        private bool CheckIDCard18(string Id)
        {
            long n = 0;
            if (long.TryParse(Id.Remove(17), out n) == false || n < Math.Pow(10, 16) || long.TryParse(Id.Replace('x', '0').Replace('X', '0'), out n) == false)
            {
                return false;//数字验证
            }

            string address = "11x22x35x44x53x12x23x36x45x54x13x31x37x46x61x14x32x41x50x62x15x33x42x51x63x21x34x43x52x64x65x71x81x82x91";

            if (address.IndexOf(Id.Remove(2)) == -1)
            {
                return false;//省份验证
            }
            string birth = Id.Substring(6, 8).Insert(6, "-").Insert(4, "-");
            DateTime time = new DateTime();
            if (DateTime.TryParse(birth, out time) == false)
            {
                return false;//生日验证
            }
            string[] arrVarifyCode = ("1,0,x,9,8,7,6,5,4,3,2").Split(',');
            string[] Wi = ("7,9,10,5,8,4,2,1,6,3,7,9,10,5,8,4,2").Split(',');
            char[] Ai = Id.Remove(17).ToCharArray();
            int sum = 0;
            for (int i = 0; i < 17; i++)
            {
                sum += int.Parse(Wi[i]) * int.Parse(Ai[i].ToString());
            }
            int y = -1;
            Math.DivRem(sum, 11, out y);
            if (arrVarifyCode[y] != Id.Substring(17, 1).ToLower())
            {
                return false;//校验码验证
            }
            return true;//符合GB11643-1999标准

        }

        private bool CheckIDCard15(string Id)
        {
            long n = 0;
            if (long.TryParse(Id, out n) == false || n < Math.Pow(10, 14))
            {
                return false;//数字验证

            }

            string address = "11x22x35x44x53x12x23x36x45x54x13x31x37x46x61x14x32x41x50x62x15x33x42x51x63x21x34x43x52x64x65x71x81x82x91";

            if (address.IndexOf(Id.Remove(2)) == -1)
            {
                return false;//省份验证
            }

            string birth = Id.Substring(6, 6).Insert(4, "-").Insert(2, "-");
            DateTime time = new DateTime();

            if (DateTime.TryParse(birth, out time) == false)
            {
                return false;//生日验证
            }

            return true;//符合15位身份证标准

        }

        //仅格式验证，为了防止以上两种验证均不通过的情况
        private bool CheckIDCardFormat(string Id)
        {
            Regex regCID = new Regex("^[1-9]\\d{7}((0\\d)|(1[0-2]))(([0|1|2]\\d)|3[0-1])\\d{3}$|^[1-9]\\d{5}[1-9]\\d{3}((0\\d)|(1[0-2]))(([0|1|2]\\d)|3[0-1])\\d{3}([0-9]|X)$");
            if (!regCID.IsMatch(Id))
            {
                return false;
            }
            return true;
        }
    }
}
