using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CIS.Core.Entities.Barangays;
using CIS.Core.Entities.Commons;
using CIS.UI.Features.Commons.Addresses;
using CIS.UI.Features.Commons.Persons;

namespace CIS.UI.Features.Barangays.Clearances.Applications
{
    public class BlotterHitViewModel: HitViewModel
    {
        public virtual Guid BlotterId { get; set; }

        public virtual Guid CitizenId { get; set; }

        public virtual PersonViewModel Respondent { get; set; }

        public virtual string Complaint { get; set; }

        public virtual string Details { get; set; }

        public virtual string Remarks { get; set; }

        public virtual Nullable<BlotterStatus> Status { get; set; }

        public virtual AddressViewModel Address { get; set; }

        public virtual Nullable<DateTime> OccuredOn { get; set; }

        public override HitScore HitScore
        {
            get { return this.ComputeHitScore(this.Applicant, this.Respondent); }
        }

        public override string DisplayTitle
        {
            get { return "Blotter"; }
        }

        public override string DisplayResult
        {
            get { return this.Complaint; }
        }

        public override string DisplayLabel
        {
            get { return this.Complaint; }
        }

        public BlotterHitViewModel()
        {
            this.Applicant = new PersonViewModel();
            this.Respondent = new PersonViewModel();
            this.Address = new AddressViewModel();
        }

    }
}
