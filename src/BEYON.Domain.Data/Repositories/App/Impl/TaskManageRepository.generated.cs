﻿//------------------------------------------------------------------------------
// <auto-generated>
//     此代码由工具生成。
//     对此文件的更改可能会导致不正确的行为，并且如果
//     重新生成代码，这些更改将会丢失。
//	   如存在本生成代码外的新需求，请在相同命名空间下创建同名分部类进行实现。
// </auto-generated>
//
// <copyright file="UserRepository.generated.cs">
//		Copyright(c)2013 Beyon.All rights reserved.
//		CLR版本：4.5.1
//		开发组织：北京博阳世通信息技术有限公司
//		公司网站：http://www.beyondb.com.cn
//		所属工程：BEYON.Domain.Data
//		生成时间：2016-03-01 16:09
// </copyright>
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BEYON.Component.Data;
using BEYON.Component.Data.EF;
using BEYON.Component.Data.EF.Interface;
using BEYON.Domain.Model.App;


namespace BEYON.Domain.Data.Repositories.App.Impl
{
	/// <summary>
    ///   仓储操作层实现——用户信息
    /// </summary>
    public partial class TaskManageRepository : EFRepositoryBase<TaskManage, Int32>, ITaskManageRepository
    {
        public TaskManageRepository(IUnitOfWork unitOfWork)
            : base()
        { }

        public TaskManage GetTaskByNumber(String projectNumber) {
            //TaskManage taskManage = null;
            var taskManage = from p in Context.TaskManages.Where(w => w.TaskID == projectNumber)
                             select new { TaskID = p.TaskID, TaskName = p.TaskName, TaskLeader = p.TaskLeader, AvailableFund = p.AvailableFund, Deficit = p.Deficit };
            var lists = taskManage.ToList();

            if (lists.Count > 0)
                return new TaskManage()
                {
                    TaskID = lists[0].TaskID,
                    TaskName = lists[0].TaskName,
                    TaskLeader = lists[0].TaskLeader,
                    AvailableFund = lists[0].AvailableFund,
                    Deficit = lists[0].Deficit
                };
            else
                return null;
        }
        public List<TaskManage> GetTaskByTaskLeader(String taskLeader)
        {
            //TaskManage taskManage = null;
            var taskManage = from p in Context.TaskManages.Where(w => w.TaskLeader == taskLeader)
                             select new { TaskID = p.TaskID, TaskName = p.TaskName, TaskLeader = p.TaskLeader, AvailableFund = p.AvailableFund, Deficit = p.Deficit };
            var listsTemp = taskManage.ToList();
            List<TaskManage> list = new List<TaskManage>();
            for (var i = 0; i < listsTemp.Count; i++)
            {
                TaskManage t = new TaskManage();

                t.TaskID = listsTemp[i].TaskID;
                t.TaskName = listsTemp[i].TaskName;
                t.TaskLeader = listsTemp[i].TaskLeader;
                t.AvailableFund = listsTemp[i].AvailableFund;
                t.Deficit = listsTemp[i].Deficit;
                list.Add(t);
            }           
            return list;
                       
        }
     }
}
