using System;
using System.Collections.Generic;
using System.Linq;
using BEYON.Component.Tools;
using BEYON.Domain.Model.Plot;
using BEYON.ViewModel.Plot;
using BEYON.ViewModel;

namespace BEYON.CoreBLL.Service.Plot.Interface
{
    public interface IExportService
    {
        String Export(string filePath, List<String> umrIds);
    }
}
