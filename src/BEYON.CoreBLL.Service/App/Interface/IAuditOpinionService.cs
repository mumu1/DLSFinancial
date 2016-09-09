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
    public interface IAuditOpinionService
    {
        IQueryable<AuditOpinion> AuditOpinions { get; }
        OperationResult Insert(AuditOpinionVM model);
        OperationResult Update(AuditOpinionVM model);
        OperationResult Delete(List<string> list);
        OperationResult Delete(AuditOpinion model);
        OperationResult Update(AuditOpinion model);
        OperationResult Import(String fileName, Service.Excel.ColumnMap[] columns);
    }
}
