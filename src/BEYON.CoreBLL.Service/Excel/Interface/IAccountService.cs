﻿using System;
using System.Linq;
using BEYON.Component.Tools;
using BEYON.Domain.Model.Member;
using BEYON.ViewModel;

namespace BEYON.CoreBLL.Service.Excel.Interface
{
    /// <summary>
    /// 账户模块业务接口
    /// </summary>
    public interface IApplyPrintService 
    {
        String ApplyExcel(String filePath, String serialNumber);
    }
}
