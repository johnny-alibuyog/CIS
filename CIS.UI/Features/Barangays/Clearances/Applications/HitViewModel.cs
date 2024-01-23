using CIS.Core.Entities.Commons;
using CIS.Core.Utilities.Extentions;
using CIS.UI.Features.Commons.Persons;
using System;

namespace CIS.UI.Features.Barangays.Clearances.Applications
{
    public class HitViewModel : ViewModelBase
    {
        public virtual Guid Id { get; set; }

        public virtual bool IsIdentifiedHit { get; set; }

        public virtual PersonViewModel Applicant { get; set; }

        public virtual PersonViewModel Defendant { get; set; }

        public virtual string Charge { get; set; }

        public virtual DateTime? FiledOn { get; set; }

        public virtual HitScore HitScore { get { return this.ComputeHitScore(this.Applicant, this.Defendant); } }

        public virtual string DisplayTitle { get { return "Blotter";} }

        public virtual string DisplayLabel { get { return this.Charge; } }

        public virtual string DisplayResult { get { return this.Charge; } }

        public HitViewModel()
        {
            this.IsIdentifiedHit = true;
        }

        protected HitScore ComputeHitScore(PersonViewModel person1, PersonViewModel person2)
        {
            if (person1 == null || person2 == null)
                return HitScore.Partial;

            if (person1.FirstName.IsEqualTo(person2.FirstName) &&
                person1.MiddleName.IsEqualTo(person2.MiddleName) &&
                person1.LastName.IsEqualTo(person2.LastName))
                return HitScore.Perfect;
            else
                return HitScore.Partial;
        }

        public override string ToString()
        {
            return this.DisplayResult;
        }
    }
}
