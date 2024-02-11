using CIS.Core.Domain.Common;
using CIS.UI.Features.Common.Address;
using CIS.UI.Features.Common.Person;
using System;

namespace CIS.UI.Features.Membership.Registration.Applications
{
    public class SuspectHitViewModel : HitViewModel
    {
        public virtual Guid SuspectId { get; set; }

        public virtual PersonViewModel Suspect { get; set; }

        public virtual string WarrantCode { get; set; }

        public virtual string CaseNumber { get; set; }

        public virtual string Crime { get; set; }

        public virtual string Description { get; set; }

        public virtual string Remarks { get; set; }

        public virtual string BailAmount { get; set; }

        public virtual string IssuedBy { get; set; }

        public virtual AddressViewModel IssuedAt { get; set; }

        public virtual DateTime? IssuedOn { get; set; }

        public override HitScore HitScore
        {
            get { return this.ComputeHitScore(this.Applicant, this.Suspect); }
        }

        public override string DisplayTitle
        {
            get { return "Suspect"; }
        }

        public override string DisplayResult
        {
            get { return this.Crime; }
        }

        public override string DisplayLabel
        {
            get { return this.Crime; }
        }

        public SuspectHitViewModel()
        {
            this.Applicant = new PersonViewModel();
            this.Suspect = new PersonViewModel();
            this.IssuedAt = new AddressViewModel();
        }
    }
}
