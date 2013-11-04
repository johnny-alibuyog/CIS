using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CIS.Core.Entities.Polices;
using CIS.UI.Features.Commons.Addresses;
using CIS.UI.Features.Commons.Persons;
using CIS.UI.Utilities.Extentions;

namespace CIS.UI.Features.Polices.Clearances
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

        public virtual Nullable<DateTime> IssuedOn { get; set; }

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
