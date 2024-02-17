﻿using System;
using System.Linq;
using NHibernate.Validator.Constraints;
using ReactiveUI;

namespace CIS.UI.Features.Membership.Registration.Applications;

public class FindingViewModel : ViewModelBase
{
    private static readonly string NoDerogatoryRemarks = "No Derogatory Records/Information";

    public virtual Guid Id { get; set; }

    [Valid]
    public virtual AmendmentViewModel Amendment { get; set; }

    public virtual IReactiveList<HitViewModel> Hits { get; set; }

    public virtual IReactiveDerivedList<HitViewModel> IdentifiedHits { get; set; }

    public virtual HitViewModel SelectedHit { get; set; }

    public virtual bool HasHits => this.Hits != null && this.Hits.Count() > 0;

    public virtual bool HasAmendments => this.Amendment != null;

    public virtual string Evaluate()
    {
        if (this.Amendment != null)
            return this.Amendment.Remarks;

        if (this.IdentifiedHits.Count() > 0)
            return string.Join("\n", this.IdentifiedHits);

        return NoDerogatoryRemarks;
    }

    public FindingViewModel()
    {
        this.Amendment = new AmendmentViewModel();
        this.ObservableForProperty(x => x.Amendment).Subscribe(amendment =>
        {
            amendment.Value?
                .IsValidObservable()
                .Subscribe(o => this.Revalidate());

            this.Revalidate();
        });

        this.Hits = new ReactiveList<HitViewModel>();
        this.Hits.ChangeTrackingEnabled = true;
        this.Hits.ItemChanged.Subscribe(_ =>
        {
            var hasIdentifiedHit = this.Hits.Any(x => x.IsIdentifiedHit == false);
            if (hasIdentifiedHit)
            {
                this.Amendment ??= new AmendmentViewModel();

                var identifiedHits = this.Hits.Where(x => x.IsIdentifiedHit);
                this.Amendment.Remarks = identifiedHits.Any()
                    ? string.Join("\n", identifiedHits)
                    : NoDerogatoryRemarks;
            }
            else
            {
                this.Amendment = null;
            }
        });

        this.IdentifiedHits = this.Hits.CreateDerivedCollection(x => x, x => x.IsIdentifiedHit);
    }
}