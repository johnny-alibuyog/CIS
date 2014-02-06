using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NHibernate.Validator.Constraints;
using ReactiveUI;

namespace CIS.UI.Features.Barangays.Clearances.Applications
{
    public class FindingViewModel : ViewModelBase
    {
        public virtual Guid Id { get; set; }

        [Valid]
        public virtual AmendmentViewModel Amendment { get; set; }

        public virtual IReactiveList<HitViewModel> Hits { get; set; }

        public virtual IReactiveDerivedList<HitViewModel> IdentifiedHits { get; set; }

        public virtual HitViewModel SelectedHit { get; set; }

        public virtual bool HasHits
        {
            get { return this.Hits != null && this.Hits.Count() > 0; }
        }

        public virtual bool HasAmendments
        {
            get { return this.Amendment != null; }
        }

        public virtual string Evaluate()
        {
            return this.IdentifiedHits.Count() > 0
                ? string.Join("\n", this.IdentifiedHits)
                : "No Derogatory Records/Information";
        }

        public FindingViewModel()
        {
            this.Amendment = new AmendmentViewModel();
            this.ObservableForProperty(x => x.Amendment).Subscribe(x =>
            {
                var amendment = x.Value;
                if (amendment != null)
                {
                    amendment.IsValidObservable()
                        .Subscribe(o => this.Revalidate());
                }

                this.Revalidate();
            });

            this.Hits = new ReactiveList<HitViewModel>();
            this.Hits.ChangeTrackingEnabled = true;
            this.Hits.ItemChanged.Subscribe(_ =>
            {
                var hasIdentifiedHit = this.Hits.Any(x => x.IsIdentifiedHit == false);
                if (hasIdentifiedHit)
                {
                    if (this.Amendment == null)
                        this.Amendment = new AmendmentViewModel();
                }
                else
                {
                    this.Amendment = null;
                }
            });

            this.IdentifiedHits = this.Hits.CreateDerivedCollection(x => x, x => x.IsIdentifiedHit);
        }
    }
}
