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
    public interface IApplicationFormService
    {
        IQueryable<ApplicationForm> ApplicationForms { get; }
        OperationResult Insert(ApplicationFormVM model);
        OperationResult Update(ApplicationFormVM model);
        OperationResult Delete(List<string> list);
        OperationResult Delete(ApplicationForm model);
        OperationResult Update(ApplicationForm model);
        OperationResult Import(String fileName, Service.Excel.ColumnMap[] columns);

        int GetTotal(String userName, String search);

        IList<ApplicationForm> GetApplicationFromByUser(String userName, int start, int limit, String search, String sortName, String sortType);

        IList<ApplicationForm> GetApplicationFromByAdmin(int start, int limit, String search, String sortName, String sortType);

    }
}
