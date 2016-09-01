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
    public partial class PhotosRepository 
    {
        public IList<Photos> Export(IList<String> umrids)
        {
            var q = from p in Context.Photoss
                    where umrids.Contains(p.UmrID)
                    select p;
            return q.ToList();
        }

        public void InsertOrUpdate(IQueryable<Photos> umrcovers)
        {
            if (umrcovers == null)
                return;

            foreach (var item in umrcovers)
            {
                //Photos oldCover = Context.Photoss.Where(t => t.UmrID == item.UmrID).FirstOrDefault();
                var query = from draft in Context.Photoss
                            where draft.UmrID == item.UmrID && draft.P_ID == item.P_ID
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
