using System;
using System.Collections.Generic;
using System.Linq;
using BEYON.Component.Data.EF.Interface;
using BEYON.Component.Tools;
using BEYON.CoreBLL.Service.App.Interface;
using BEYON.Domain.Data.Repositories.App;
using BEYON.Domain.Model.App;
using BEYON.ViewModel.App;
using BEYON.CoreBLL.Service.Excel;
using EntityFramework.Extensions;


namespace BEYON.CoreBLL.Service.App
{
    public class TaskManageService : CoreServiceBase, ITaskManageService
    {
        private readonly ITaskManageRepository _TaskManageRepository;



        public TaskManageService(ITaskManageRepository taskManageRepository, IUnitOfWork unitOfWork)
            : base(unitOfWork)
        {
            this._TaskManageRepository = taskManageRepository;
        }
        public IQueryable<TaskManage> TaskManages
        {
            get { return _TaskManageRepository.Entities; }
        }

        public OperationResult Insert(TaskManageVM model)
        {
            try
            {
                TaskManage task = _TaskManageRepository.Entities.FirstOrDefault(c => c.TaskID == model.TaskID.Trim());
                if (task != null)
                {
                    return new OperationResult(OperationResultType.Warning, "数据库中已经存在相同的课题信息，请修改后重新提交！");
                }
                if (model.TaskName == null || model.TaskName.Trim() == "")
                    return new OperationResult(OperationResultType.Warning, "课题名称不能为空，请修改后重新提交！");
                var entity = new TaskManage
                {
                    TaskID = model.TaskID,
                    TaskName = model.TaskName,
                    TaskLeader = model.TaskLeader.Trim(),
                    AvailableFund = model.AvailableFund,
                    Deficit = model.Deficit,
                    UpdateDate = DateTime.Now
                };
                _TaskManageRepository.Insert(entity);

                return new OperationResult(OperationResultType.Success, "新增数据成功！");
            }
            catch
            {
                return new OperationResult(OperationResultType.Error, "新增数据失败，数据库插入数据时发生了错误!");
            }
        }
        public OperationResult Update(TaskManageVM model)
        {
            try
            {
                TaskManage task = _TaskManageRepository.Entities.FirstOrDefault(c => c.TaskID == model.TaskID.Trim());
                if (task == null)
                {
                    throw new Exception();
                }
                task.TaskID = model.TaskID;
                task.TaskName = model.TaskName;
                task.TaskLeader = model.TaskLeader.Trim();
                task.AvailableFund = model.AvailableFund;
                task.Deficit = model.Deficit;
                task.UpdateDate = DateTime.Now;
                _TaskManageRepository.Update(task);
                return new OperationResult(OperationResultType.Success, "更新数据成功！");
            }
            catch
            {
                return new OperationResult(OperationResultType.Error, "更新数据失败!");
            }
        }

        public OperationResult Delete(List<string> taskManageID)
        {
            try
            {
                if (taskManageID != null)
                {
                    int count = _TaskManageRepository.Delete(_TaskManageRepository.Entities.Where(c => taskManageID.Contains(c.TaskID)));
                    if (count > 0)
                    {
                        return new OperationResult(OperationResultType.Success, "删除数据成功！");
                    }
                    else
                    {
                        return new OperationResult(OperationResultType.Error, "删除数据失败!");
                    }
                }
                else
                {
                    return new OperationResult(OperationResultType.ParamError, "参数错误，请选择需要删除的数据!");
                }
            }
            catch
            {
                return new OperationResult(OperationResultType.Error, "删除数据失败!");
            }
        }
        public OperationResult Update(TaskManage model)
        {
            try
            {
                model.UpdateDate = DateTime.Now;
                _TaskManageRepository.Update(model);
                return new OperationResult(OperationResultType.Success, "更新课题数据成功！");
            }
            catch
            {
                return new OperationResult(OperationResultType.Error, "更新课题数据失败!");
            }
        }

        public OperationResult Delete(TaskManage model)
        {
            try
            {
                model.UpdateDate = DateTime.Now;
                _TaskManageRepository.Delete(model);
                return new OperationResult(OperationResultType.Success, "更新课题数据成功！");
            }
            catch
            {
                return new OperationResult(OperationResultType.Error, "更新课题数据失败!");
            }
        }

        public OperationResult Import(String fileName, Service.Excel.ColumnMap[] columns)
        {
            try
            {
                var items = ExcelService.GetObjects<TaskManage>(fileName, columns);
                _TaskManageRepository.InsertOrUpdate(items);
                return new OperationResult(OperationResultType.Success, "导入数据成功！");
            }
            catch (Exception ex)
            {
                return new OperationResult(OperationResultType.Error, "导入数据失败!");
            }
        }

        public TaskManage GetTaskByNumber(String projectNumber)
        {           
           return  _TaskManageRepository.GetTaskByNumber(projectNumber);           
        }

        public List<TaskManage> GetTaskByTaskLeader(String taskLeader)
        {
            return _TaskManageRepository.GetTaskByTaskLeader(taskLeader);
        }

        public OperationResult DeleteAll()
        {
            try
            {
                _TaskManageRepository.Delete(_TaskManageRepository.Entities);
                return new OperationResult(OperationResultType.Success, "清空数据成功！");
            }
            catch
            {
                return new OperationResult(OperationResultType.Error, "清空数据失败!");
            }
        }
    }
}
