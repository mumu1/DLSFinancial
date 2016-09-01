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
    public partial class PointsRepository 
    {
        public IList<Points> Export(IList<String> umrids)
        {
            var q = from p in Context.Pointss
                    where umrids.Contains(p.UmrID)
                    select p;
            return q.ToList();
        }

        public void InsertOrUpdate(IQueryable<Points> umrcovers)
        {
            if (umrcovers == null)
                return;

            foreach (var item in umrcovers)
            {
                //Points oldCover = Context.Pointss.Where(t => t.UmrID == item.UmrID).FirstOrDefault();
                var query = from draft in Context.Pointss
                            where draft.UmrID == item.UmrID && draft.PointID == item.PointID
                            select draft;
                var oldCover = query.FirstOrDefault();

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
