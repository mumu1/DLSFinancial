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
    public interface ITopContactsService
    {
        IQueryable<TopContacts> TopContactss { get; }
        OperationResult Insert(TopContactsVM model);

        OperationResult Insert(TopContacts model);
        OperationResult Update(TopContactsVM model);

        OperationResult Delete(List<TopContacts> list);
        OperationResult Delete(TopContacts model);
        OperationResult Update(TopContacts model);

        IList<TopContacts> GetTopContactsByUserID(String userID);

        IList<TopContacts> GetTopContactsByName(String name);
    }
}
