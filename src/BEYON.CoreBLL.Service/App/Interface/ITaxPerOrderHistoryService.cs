﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BEYON.Component.Tools;
using BEYON.Domain.Model.App;
using BEYON.ViewModel.App;


namespace BEYON.CoreBLL.Service.App.Interface
{
    public interface ITaxPerOrderHistoryService
    {
        IQueryable<TaxPerOrderHistory> TaxPerOrderHistorys { get; }
        OperationResult Insert(TaxPerOrderHistory model, bool isSave = true);
        OperationResult Delete(List<string> list, bool isSave = true);
        OperationResult Delete(TaxPerOrderHistory model, bool isSave = true);
        OperationResult Update(TaxPerOrderHistory model, bool isSave = true);

        void InsertOrUpdate(TaxPerOrderHistory contact);

        int GetTotal(String search);
        IList<TaxPerOrderHistory> GetAllData(String search, String sortName, String sortType, int start, int limit);
    }
}
