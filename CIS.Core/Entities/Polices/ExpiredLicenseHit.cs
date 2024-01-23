using CIS.Core.Entities.Firearms;
using System;

namespace CIS.Core.Entities.Polices
{
    public class ExpiredLicenseHit : Hit
    {
        private License _licence;
        private DateTime? _expiryDate;

        public virtual License License
        {
            get { return _licence; }
            set { _licence = value; }
        }

        public virtual DateTime? ExpiryDate
        {
            get { return _expiryDate; }
            set { _expiryDate = value; }
        }

        protected internal virtual void SerializeWith(ExpiredLicenseHit value)
        {
            this.HitScore = value.HitScore;
            this.IsIdentical = value.IsIdentical;
            this.License = value.License;
            this.ExpiryDate = value.ExpiryDate;
        }
    }
}
