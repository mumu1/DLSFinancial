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
    public class DepartmentService : CoreServiceBase, IDepartmentService
    {
        private readonly IDepartmentRepository _DepartmentRepository;



        public DepartmentService(IDepartmentRepository departmentRepository, IUnitOfWork unitOfWork)
            : base(unitOfWork)
        {
            this._DepartmentRepository = departmentRepository;
        }
        public IQueryable<Department> Departments
        {
            get { return _DepartmentRepository.Entities; }
        }

        public OperationResult Insert(DepartmentVM model)
        {
            try
            {
                Department department = _DepartmentRepository.Entities.FirstOrDefault(c => c.DepartmentCode == model.DepartmentCode.Trim());
                if (department != null)
                {
                    return new OperationResult(OperationResultType.Warning, "数据库中已经存在相同的部门信息，请修改后重新提交！");
                }
                if (model.DepartmentName == null || model.DepartmentName.Trim() == "")
                    return new OperationResult(OperationResultType.Warning, "部门名称不能为空，请修改后重新提交！");
                var entity = new Department
                {
                    DepartmentCode = model.DepartmentCode,
                    DepartmentName = model.DepartmentName,
                    UpdateDate = DateTime.Now
                };
                _DepartmentRepository.Insert(entity);

                return new OperationResult(OperationResultType.Success, "新增数据成功！");
            }
            catch
            {
                return new OperationResult(OperationResultType.Error, "新增数据失败，数据库插入数据时发生了错误!");
            }
        }
        public OperationResult Update(DepartmentVM model)
        {
            try
            {
                Department department = _DepartmentRepository.Entities.FirstOrDefault(c => c.DepartmentCode == model.DepartmentCode.Trim());
                if (department == null)
                {
                    throw new Exception();
                }
                department.DepartmentName = model.DepartmentName;
                department.DepartmentCode = model.DepartmentCode;
                department.UpdateDate = DateTime.Now;
                _DepartmentRepository.Update(department);
                return new OperationResult(OperationResultType.Success, "更新数据成功！");
            }
            catch
            {
                return new OperationResult(OperationResultType.Error, "更新数据失败!");
            }
        }

        public OperationResult Delete(List<string> departmentCode)
        {
            try
            {
                if (departmentCode != null)
                {
                    int count = _DepartmentRepository.Delete(_DepartmentRepository.Entities.Where(c => departmentCode.Contains(c.DepartmentCode)));
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
        public OperationResult Update(Department model)
        {
            try
            {
                model.UpdateDate = DateTime.Now;
                _DepartmentRepository.Update(model);
                return new OperationResult(OperationResultType.Success, "更新部门数据成功！");
            }
            catch
            {
                return new OperationResult(OperationResultType.Error, "更新部门数据失败!");
            }
        }

        public OperationResult Delete(Department model)
        {
            try
            {
                model.UpdateDate = DateTime.Now;
                _DepartmentRepository.Delete(model);
                return new OperationResult(OperationResultType.Success, "更新部门数据成功！");
            }
            catch
            {
                return new OperationResult(OperationResultType.Error, "更新开户银行数据失败!");
            }
        }

        public OperationResult Import(String fileName, Service.Excel.ColumnMap[] columns)
        {
            try
            {
                var items = ExcelService.GetObjects<Department>(fileName, columns);
                _DepartmentRepository.InsertOrUpdate(items);
                return new OperationResult(OperationResultType.Success, "导入数据成功！");
            }
            catch (Exception ex)
            {
                return new OperationResult(OperationResultType.Error, "导入数据失败!");
            }
        }
    }
}
