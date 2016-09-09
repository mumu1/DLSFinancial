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
    public interface ITitleService
    {
        IQueryable<Title> Titles { get; }
        OperationResult Insert(TitleVM model);
        OperationResult Update(TitleVM model);
        OperationResult Delete(List<string> list);
        OperationResult Delete(Title model);
        OperationResult Update(Title model);
        OperationResult Import(String fileName, Service.Excel.ColumnMap[] columns);
    }
}
