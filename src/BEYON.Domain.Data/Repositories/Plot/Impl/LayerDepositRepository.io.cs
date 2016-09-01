using BEYON.Component.Data.EF;
using BEYON.Component.Data.EF.Interface;
using BEYON.Domain.Model.Plot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BEYON.Domain.Data.Repositories.Plot.Impl
{
    public partial class LayerDepositRepository 
    {
        public IList<LayerDeposit> Export(IList<String> umrids)
        {
            var q = from p in Context.LayerDeposits
                    where umrids.Contains(p.UmrID)
                    select p;
            return q.ToList();
        }

        public void InsertOrUpdate(IQueryable<LayerDeposit> umrcovers)
        {
            if (umrcovers == null)
                return;

            foreach (var item in umrcovers)
            {
                LayerDeposit oldCover = Context.LayerDeposits.Where(t => t.UmrID == item.UmrID).FirstOrDefault();
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
