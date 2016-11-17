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
    public class SafeguardTimeService : CoreServiceBase, ISafeguardTimeService
    {
        private readonly ISafeguardTimeRepository _SafeguardTimeRepository;



        public SafeguardTimeService(ISafeguardTimeRepository safeguardTimeRepository, IUnitOfWork unitOfWork)
            : base(unitOfWork)
        {
            this._SafeguardTimeRepository = safeguardTimeRepository;
        }
        public IQueryable<SafeguardTime> SafeguardTimes
        {
            get { return _SafeguardTimeRepository.Entities; }
        }

        public string GetSafeguardTime() {
            //若没有特别设定指定一到五日系统维护，普通用户无法登陆
            var now = DateTime.Now;
            var startTime = new DateTime(now.Year, now.Month, 1);
            var endTime = new DateTime(now.Year, now.Month, 6);
            //从数据库表SafeguardTime获取用户保存的系统维护时间
            var saveStartTime = this.SafeguardTimes.First().StartTime;
            var saveEndTime = this.SafeguardTimes.First().EndTime;
            if (now <= saveStartTime)
            {
                startTime = saveStartTime;
                endTime = saveEndTime;
            }
            string safeguardTime = "[" + startTime.ToShortDateString() + "," + endTime.ToShortDateString() + "]";
            return safeguardTime;
        }

        public OperationResult Insert(SafeguardTime model)
        {
            try
            {
                SafeguardTime safeguardTime = _SafeguardTimeRepository.Entities.FirstOrDefault(c => c.StartTime == model.StartTime);
                if (safeguardTime != null)
                {
                    return new OperationResult(OperationResultType.Warning, "数据库中已经存在相同的银行信息，请修改后重新提交！");
                }
                var entity = new SafeguardTime
                {
                    StartTime = model.StartTime,
                    EndTime = model.EndTime,
                    UpdateDate = DateTime.Now
                };
                _SafeguardTimeRepository.Insert(entity);

                return new OperationResult(OperationResultType.Success, "新增数据成功！");
            }
            catch
            {
                return new OperationResult(OperationResultType.Error, "新增数据失败，数据库插入数据时发生了错误!");
            }
        }
        public OperationResult Update(SafeguardTime model)
        {
            try
            {
                SafeguardTime safeguardTime = _SafeguardTimeRepository.Entities.FirstOrDefault();
                if (safeguardTime == null)
                {
                    throw new Exception();
                }
                safeguardTime.StartTime = model.StartTime;
                safeguardTime.EndTime = model.EndTime;
                safeguardTime.UpdateDate = DateTime.Now;
                _SafeguardTimeRepository.Update(safeguardTime);
                return new OperationResult(OperationResultType.Success, "更新数据成功！");
            }
            catch
            {
                return new OperationResult(OperationResultType.Error, "更新数据失败!");
            }
        }

   
    }
}
