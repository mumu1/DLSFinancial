/************************************
 * 描述：尚未添加描述
 * 作者：林永恒
 * 日期：2016/7/7 16:48:54  
*************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using BEYON.Domain.Model.Plot;
using BEYON.ViewModel.Plot;

namespace BEYON.Domain.Data.Repositories.Plot.Impl
{
    public partial class UmrcoverRepository
    {
        public IList<Umrcover> Export(IList<String> umrids)
        {
            var q = from p in Context.Umrcovers
                    where umrids.Contains(p.UmrID)
                    select p;
            return q.ToList();
        }

        public void InsertOrUpdate(IQueryable<Umrcover> umrcovers)
        {
            if (umrcovers == null)
                return;

            foreach (var item in umrcovers)
            {
                Umrcover oldCover = Context.Umrcovers.Where(t => t.UmrID == item.UmrID).FirstOrDefault();
                if (oldCover == null)
                {
                    this.Insert(item);
                }
                else
                {
                    if (oldCover.UpdateDate < item.UpdateDate)
                    {
                        item.Id = oldCover.Id;
                        this.Update(item);
                    }

                }
            }
        }
    }
}
