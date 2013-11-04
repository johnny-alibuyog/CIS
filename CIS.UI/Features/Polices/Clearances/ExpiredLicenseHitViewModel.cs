using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CIS.Core.Entities.Polices;
using CIS.UI.Features.Commons.Addresses;
using CIS.UI.Features.Commons.Persons;
using CIS.UI.Features.Firearms.Licenses;

namespace CIS.UI.Features.Polices.Clearances
{
    public class ExpiredLicenseHitViewModel : HitViewModel
    {
        public virtual Guid LicenseId { get; set; }

        public virtual PersonViewModel Person { get; set; }

        public virtual AddressViewModel Address { get; set; }

        public virtual GunViewModel Gun { get; set; }

        public virtual string LicenseNumber { get; set; }

        public virtual Nullable<DateTime> ExpiryDate { get; set; }

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
