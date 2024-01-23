using CIS.Core.Entities.Commons;
using CIS.UI.Features.Commons.Addresses;
using CIS.UI.Features.Commons.Persons;
using CIS.UI.Features.Firearms.Licenses.Registrations;
using System;

namespace CIS.UI.Features.Polices.Clearances.Applications
{
    public class ExpiredLicenseHitViewModel : HitViewModel
    {
        public virtual Guid LicenseId { get; set; }

        public virtual PersonViewModel Person { get; set; }

        public virtual AddressViewModel Address { get; set; }

        public virtual GunViewModel Gun { get; set; }

        public virtual string LicenseNumber { get; set; }

        public virtual DateTime? ExpiryDate { get; set; }

        public override HitScore HitScore
        {
            get { return this.ComputeHitScore(this.Applicant, this.Person); }
        }

        public override string DisplayTitle
        {
            get { return "Expired License"; }
        }

        public override string DisplayResult
        {
            get
            {
                return string.Format("Expired Firearm Lincense - FA Lic. No. {0} - {1}", 
                    this.LicenseNumber, this.ExpiryDate.Value.ToString("MMM-dd-yyyy"));
            }
        }

        public override string DisplayLabel
        {
            get { return "Expired Lincense"; }
        }

        public ExpiredLicenseHitViewModel()
        {
            this.Applicant = new PersonViewModel();
            this.Person = new PersonViewModel();
            this.Address = new AddressViewModel();
            this.Gun = new GunViewModel();
        }
    }
}
