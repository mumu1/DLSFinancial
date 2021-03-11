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
    public interface ITaskManageService
    {
        IQueryable<TaskManage> TaskManages { get; }
        OperationResult Insert(TaskManageVM model);
        OperationResult Update(TaskManageVM model);
        OperationResult Delete(List<string> list);
        OperationResult Delete(TaskManage model);
        OperationResult Update(TaskManage model);
        OperationResult Import(String fileName, Service.Excel.ColumnMap[] columns);

        TaskManage GetTaskByNumber(String projectNumber);
        List<TaskManage> GetTaskByTaskLeader(String taskLeader);
        OperationResult DeleteAll();
    }
}
