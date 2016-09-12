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
        OperationResult Insert(PersonalRecordVM model);
        OperationResult Update(PersonalRecordVM model);
        OperationResult Delete(List<string> list);
        OperationResult Delete(PersonalRecord model);
        OperationResult Update(PersonalRecord model);
        OperationResult Import(String fileName, Service.Excel.ColumnMap[] columns);
    }
}
