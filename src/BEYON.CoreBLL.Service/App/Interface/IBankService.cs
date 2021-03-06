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
    public interface IBankService
    {
        IQueryable<Bank> Banks { get; }
        OperationResult Insert(BankVM model);
        OperationResult Update(BankVM model);
        OperationResult Delete(List<string> list);
        OperationResult Delete(Bank model);
        OperationResult Update(Bank model);
        OperationResult Import(String fileName, Service.Excel.ColumnMap[] columns);
    }
}
