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

namespace BEYON.CoreBLL.Service.App
{
    public class PersonalRecordService : CoreServiceBase, IPersonalRecordService
    {
        private readonly IPersonalRecordRepository _PersonalRecordRepository;



        public PersonalRecordService(IPersonalRecordRepository personalRecordRepository, IUnitOfWork unitOfWork)
            : base(unitOfWork)
        {
            this._PersonalRecordRepository = personalRecordRepository;
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
                
                return new OperationResult(OperationResultType.Success, "新增数据成功！");
            }
            catch(Exception ex)
            {
                return new OperationResult(OperationResultType.Error, "新增数据失败：" + ex.Message);
            }
        }
        public OperationResult Update(PersonalRecordVM model, bool isSave)
        {
            try
            {
                PersonalRecord personalRecord = _PersonalRecordRepository.Entities.FirstOrDefault(c => c.SerialNumber == model.SerialNumber &&  c.CertificateID == model.CertificateID.Trim());
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
            catch
            {
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
            catch
            {
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
            catch
            {
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
            catch
            {
                return new OperationResult(OperationResultType.Error, "更新数据失败!");
            }
        }

        public OperationResult Import(String fileName, Service.Excel.ColumnMap[] columns)
        {
            try
            {
                var items = ExcelService.GetObjects<PersonalRecord>(fileName, columns);
                _PersonalRecordRepository.InsertOrUpdate(items);
                return new OperationResult(OperationResultType.Success, "导入数据成功！");
            }
            catch (Exception ex)
            {
                return new OperationResult(OperationResultType.Error, "导入数据失败!");
            }
        }
    }
}
