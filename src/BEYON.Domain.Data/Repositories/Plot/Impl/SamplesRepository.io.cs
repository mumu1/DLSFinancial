using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Threading.Tasks;
using log4net;
using BEYON.Component.Data.EF;
using BEYON.Component.Data.EF.Interface;
using BEYON.Domain.Model.Plot;

namespace BEYON.Domain.Data.Repositories.Plot.Impl
{
    public partial class SamplesRepository
    {
        public IList<Samples> Export(IList<String> umrids)
        {
            var q = from p in Context.Sampless
                    where umrids.Contains(p.UmrID)
                    select p;
            return q.ToList();
        }

        public void InsertOrUpdate(IQueryable<Samples> umrcovers)
        {
            if (umrcovers == null)
                return;

            foreach (var item in umrcovers)
            {
                //Samples oldCover = Context.Sampless.Where(t => t.UmrID == item.UmrID).FirstOrDefault();
                var query = from draft in Context.Sampless
                            where draft.UmrID == item.UmrID && draft.SampleID == item.SampleID
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
