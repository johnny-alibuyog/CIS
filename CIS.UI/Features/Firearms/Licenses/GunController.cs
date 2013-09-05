using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CIS.Core.Entities.Firearms;
using CIS.UI.Utilities.Extentions;
using NHibernate;
using NHibernate.Linq;

namespace CIS.UI.Features.Firearms.Licenses
{
    public class GunController : ControllerBase<GunViewModel>
    {
        public GunController(GunViewModel viewModel) : base(viewModel) 
        {
            using (var session = this.SessionFactory.OpenSession())
            using (var transaction = session.BeginTransaction())
            {
                var makeQuery = session.Query<Make>().Cacheable().ToFuture();
                var kindQuery = session.Query<Kind>().Cacheable().ToFuture();

                this.ViewModel.Makes = makeQuery
                    .Select(x => new Lookup<Guid>(x.Id, x.Name))
                    .ToReactiveList();

                this.ViewModel.Kinds = kindQuery
                    .Select(x => new Lookup<Guid>(x.Id, x.Name))
                    .ToReactiveList();

                transaction.Commit();
            }
        }
    }
}
