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
    public class TopContactsService : CoreServiceBase, ITopContactsService
    {
        private readonly log4net.ILog _log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ITopContactsRepository _TopContactsRepository;

        public IList<TopContacts> GetTopContactsByUserID(String userID)
        {
            return _TopContactsRepository.GetTopContactsByUserID(userID);
        }

        public IList<TopContacts> GetTopContactsByName(String name, String userID)
        {
            return _TopContactsRepository.GetTopContactsByName(name, userID);
        }


        public TopContactsService(ITopContactsRepository topContactsRepository, IUnitOfWork unitOfWork)
            : base(unitOfWork)
        {
            this._TopContactsRepository = topContactsRepository;

        }
        public IQueryable<TopContacts> TopContactss
        {
            get { return _TopContactsRepository.Entities; }
        }

        public OperationResult Insert(TopContactsVM model)
        {
            try
            {
                String CertificateIDCk = GetReplaceString(model.CertificateID);
                String AccountNumberCk = GetReplaceString(model.AccountNumber);

                //1.检查是否有重复字段
                TopContacts[] topContacts = _TopContactsRepository.Entities.Where(w => w.UserID == model.UserID && w.CertificateID == CertificateIDCk && w.AccountNumber == AccountNumberCk).ToArray();
                if (topContacts != null && topContacts.Length > 0)
                {
                    return new OperationResult(OperationResultType.Warning, "常用领款人列表中已经存在相同的人员信息，请修改后重新提交！");
                }

                if (String.IsNullOrEmpty(model.CertificateID))
                    return new OperationResult(OperationResultType.Warning, "证件号码不能为空，请修改后重新提交！");
                //若姓名为英文字母，不需去除字符串中的空格，若为中文，需去除空格
                String nameFormat = "";
                if (IsEnCh(model.Name.Trim()))
                {
                    nameFormat = model.Name.Trim().Replace("\n", "").Replace("\t", "").Replace("\r", "");
                }
                else {
                    nameFormat = GetReplaceString(model.Name);
                }

                String idType = GetReplaceString(model.CertificateType);
                String birthVar = "";
                String genderVar = "";
                if ("外国护照".Equals(idType))
                {
                    birthVar = GetReplaceString(model.Birth);
                    genderVar = GetReplaceString(model.Gender);
                }

                var entity = new TopContacts
                {
                    UserID = model.UserID,
                    Name = nameFormat,
                    CertificateID = GetReplaceString(model.CertificateID),
                    CertificateType = GetReplaceString(model.CertificateType),
                    Company = GetReplaceString(model.Company),
                    Tele = GetReplaceString(model.Tele),
                    PersonType = GetReplaceString(model.PersonType),
                    Nationality = GetReplaceString(model.Nationality),
                    Title = GetReplaceString(model.Title),
                    Bank = GetReplaceString(model.Bank),
                    BankDetailName = GetReplaceString(model.BankDetailName),
                    ProvinceCity = GetReplaceString(model.ProvinceCity),
                    Gender = genderVar,
                    Birth = birthVar,
                    AccountNumber = GetReplaceString(model.AccountNumber),
                    UpdateDate = DateTime.Now
                };
                _TopContactsRepository.Insert(entity);

                return new OperationResult(OperationResultType.Success, "新增数据成功！", entity);

            }
            catch (Exception ex)
            {
                _log.Error(ex);
                return new OperationResult(OperationResultType.Error, "新增数据失败：" + ex.Message);
            }

        }

        public OperationResult Insert(TopContacts model)
        {
            try
            {
                String CertificateIDCk = GetReplaceString(model.CertificateID);
                String AccountNumberCk = GetReplaceString(model.AccountNumber);

                //检查是否有重复字段
                TopContacts[] topContacts = _TopContactsRepository.Entities.Where(w => w.UserID == model.UserID && w.CertificateID == CertificateIDCk && w.AccountNumber == AccountNumberCk).ToArray();
                if (topContacts != null && topContacts.Length > 0)
                {
                    return new OperationResult(OperationResultType.Warning, "常用领款人列表中已经存在相同的人员信息，请修改后重新提交！");
                }
                //若姓名为英文字母，不需去除字符串中的空格，若为中文，需去除空格
                String nameFormat = "";
                if (IsEnCh(model.Name.Trim()))
                {
                    nameFormat = model.Name.Trim().Replace("\n", "").Replace("\t", "").Replace("\r", "");
                }
                else
                {
                    nameFormat = GetReplaceString(model.Name);
                }

                String idType = GetReplaceString(model.CertificateType);
                String birthVar = "";
                String genderVar = "";
                if ("外国护照".Equals(idType))
                {
                    birthVar = GetReplaceString(model.Birth);
                    genderVar = GetReplaceString(model.Gender);
                }

                var entity = new TopContacts
                {
                    UserID = model.UserID,
                    Name = nameFormat,
                    CertificateID = GetReplaceString(model.CertificateID),
                    CertificateType = GetReplaceString(model.CertificateType),
                    Company = GetReplaceString(model.Company),
                    Tele = GetReplaceString(model.Tele),
                    PersonType = GetReplaceString(model.PersonType),
                    Nationality = GetReplaceString(model.Nationality),
                    Title = GetReplaceString(model.Title),
                    Bank = GetReplaceString(model.Bank),
                    BankDetailName = GetReplaceString(model.BankDetailName),
                    ProvinceCity = GetReplaceString(model.ProvinceCity),
                    AccountNumber = GetReplaceString(model.AccountNumber),
                    Gender = genderVar,
                    Birth = birthVar,
                    UpdateDate = DateTime.Now
                };

                TopContacts[] topContacts2 = _TopContactsRepository.Entities.Where(w => w.UserID == model.UserID && w.CertificateID == CertificateIDCk).ToArray();
                if (topContacts2 != null && topContacts2.Length > 0)
                {
                    if (!String.IsNullOrEmpty(AccountNumberCk))
                    {
                        foreach (var t in topContacts2)
                        {
                            if (t.AccountNumber.Equals(AccountNumberCk))
                            {
                                _TopContactsRepository.Delete(t);
                            }
                        }
                        _TopContactsRepository.Insert(entity);
                        return new OperationResult(OperationResultType.Success, "新增数据成功！", entity);
                    }
                    else {
                        return new OperationResult(OperationResultType.Success, "新增数据成功！", entity);
                    }
                }
                else {
                    _TopContactsRepository.Insert(entity);
                    return new OperationResult(OperationResultType.Success, "新增数据成功！", entity);
                }

            }
            catch (Exception ex)
            {
                _log.Error(ex);
                return new OperationResult(OperationResultType.Error, "新增数据失败：" + ex.Message);
            }
        }

        private String GetReplaceString(String modelString)
        {
            if (String.IsNullOrEmpty(modelString))
            {
                return "";
            }
            else
            {
                return modelString.Trim().Replace("\n", "").Replace(" ", "").Replace("\t", "").Replace("\r", "");
            }

        }

        /// <summary>  
        /// 判断输入的字符串是否只包含英文字母      
        public  bool IsEnCh(string input)
        {
            string pattern = @"^[a-zA-Z \./']+$";
            Regex regex = new Regex(pattern);
            return regex.IsMatch(input);
        }   

        public OperationResult Update(TopContactsVM model)
        {
            try
            {
                String CertificateIDCk = GetReplaceString(model.CertificateID);
                String AccountNumberCk = GetReplaceString(model.AccountNumber);

                TopContacts topContacts = _TopContactsRepository.Entities.FirstOrDefault(c => c.UserID == model.UserID);
                if (topContacts == null)
                {
                    throw new Exception();
                }
                //若姓名为英文字母，不需去除字符串中的空格，若为中文，需去除空格
                String nameFormat = "";
                if (IsEnCh(model.Name.Trim()))
                {
                    nameFormat = model.Name.Trim().Replace("\n", "").Replace("\t", "").Replace("\r", "");
                }
                else
                {
                    nameFormat = GetReplaceString(model.Name);
                }

                String idType = GetReplaceString(model.CertificateType);
                String birthVar = "";
                String genderVar = "";
                if ("外国护照".Equals(idType))
                {
                    birthVar = GetReplaceString(model.Birth);
                    genderVar = GetReplaceString(model.Gender);
                }

                topContacts.UserID = model.UserID;
                topContacts.Name = nameFormat;
                topContacts.CertificateID = GetReplaceString(model.CertificateID);
                topContacts.CertificateType = GetReplaceString(model.CertificateType);
                topContacts.Company = GetReplaceString(model.Company);
                topContacts.Tele = GetReplaceString(model.Tele);
                topContacts.PersonType = GetReplaceString(model.PersonType);
                topContacts.Nationality = GetReplaceString(model.Nationality);
                topContacts.Title = GetReplaceString(model.Title);
                topContacts.Bank = GetReplaceString(model.Bank);
                topContacts.BankDetailName = GetReplaceString(model.BankDetailName);
                topContacts.ProvinceCity = GetReplaceString(model.ProvinceCity);
                topContacts.AccountNumber = GetReplaceString(model.AccountNumber);
                topContacts.Gender = genderVar;
                topContacts.Birth = birthVar;
                topContacts.UpdateDate = DateTime.Now;
                _TopContactsRepository.Update(topContacts);
                return new OperationResult(OperationResultType.Success, "更新数据成功！");
            }
            catch (Exception ex)
            {
                _log.Error(ex);
                return new OperationResult(OperationResultType.Error, "更新数据失败!");
            }
        }


        public OperationResult Delete(List<TopContacts> list)
        {
            try
            {
                if (list.Count > 0)
                {
                    foreach (var item in list)
                    {
                        _TopContactsRepository.Delete(item);
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

        public OperationResult Update(TopContacts model)
        {
            try
            {
                //若姓名为英文字母，不需去除字符串中的空格，若为中文，需去除空格
                String nameFormat = "";
                if (IsEnCh(model.Name.Trim()))
                {
                    nameFormat = model.Name.Trim().Replace("\n", "").Replace("\t", "").Replace("\r", "");
                }
                else
                {
                    nameFormat = GetReplaceString(model.Name);
                }
                model.UpdateDate = DateTime.Now;
                model.AccountNumber = GetReplaceString(model.AccountNumber);
                model.BankDetailName = GetReplaceString(model.BankDetailName);
                model.ProvinceCity = GetReplaceString(model.ProvinceCity);
                model.Name = nameFormat;
                model.Nationality = GetReplaceString(model.Nationality);
                model.Tele = GetReplaceString(model.Tele);
                model.CertificateID = GetReplaceString(model.CertificateID);
                _TopContactsRepository.Update(model);              
                return new OperationResult(OperationResultType.Success, "更新数据成功！");
            }
            catch
            {
                return new OperationResult(OperationResultType.Error, "更新数据失败!");
            }
        }

        public OperationResult Delete(TopContacts model)
        {
            try
            {
                model.UpdateDate = DateTime.Now;
                _TopContactsRepository.Delete(model);
                return new OperationResult(OperationResultType.Success, "删除数据成功！");
            }
            catch (Exception ex)
            {
                _log.Error(ex);
                return new OperationResult(OperationResultType.Error, "删除数据失败!");
            }
        }

    }
}
