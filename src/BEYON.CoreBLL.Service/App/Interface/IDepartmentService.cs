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
    public interface IDepartmentService
    {
        IQueryable<Department> Departments { get; }
        OperationResult Insert(DepartmentVM model);
        OperationResult Update(DepartmentVM model);
        OperationResult Delete(List<string> list);
        OperationResult Delete(Department model);
        OperationResult Update(Department model);
        OperationResult Import(String fileName, Service.Excel.ColumnMap[] columns);
    }
}
