using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BEYON.Component.Tools;
using BEYON.Domain.Model.App;
using BEYON.ViewModel.App;


namespace BEYON.CoreBLL.Service.App.Interface
{
    public interface IPersonalRecordService
    {
        IQueryable<PersonalRecord> PersonalRecords { get; }
        OperationResult Insert(PersonalRecordVM model, bool isSave = true);
        OperationResult Update(PersonalRecordVM model, bool isSave = true);
        OperationResult Delete(List<string> list, bool isSave = true);

        OperationResult DeleteModel(List<PersonalRecord> list, bool isSave = true);
        OperationResult Delete(PersonalRecord model, bool isSave = true);
        OperationResult Update(PersonalRecord model, bool isSave = true);
        OperationResult Import(String fileName, Service.Excel.ImportData importData);
    }
}
