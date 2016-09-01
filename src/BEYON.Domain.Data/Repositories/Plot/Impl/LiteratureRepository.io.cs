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
    public partial class LiteratureRepository 
    {
        public IList<Literature> Export(IList<String> umrids)
        {
            var q = from p in Context.literatures
                    where umrids.Contains(p.UmrID)
                    select p;
            return q.ToList();
        }

        public void InsertOrUpdate(IQueryable<Literature> umrcovers)
        {
            if (umrcovers == null)
                return;

            foreach (var item in umrcovers)
            {
                //Literature oldCover = Context.literatures.Where(t => t.UmrID == item.UmrID).FirstOrDefault();
                var query = from draft in Context.literatures
                            where draft.UmrID == item.UmrID && draft.LiteratureID == item.LiteratureID
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
