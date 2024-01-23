using System;

namespace CIS.UI.Features.Firearms.Licenses.Registrations
{
    public class LicenseListItemViewModel : ViewModelBase
    {
        public virtual Guid Id { get; set; }

        public virtual string Owner { get; set; }

        public virtual string Gun { get; set; }

        public virtual DateTime? ExpiryDate { get; set; }
    }
}
