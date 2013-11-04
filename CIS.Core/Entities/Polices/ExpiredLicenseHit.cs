using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CIS.Core.Entities.Firearms;

namespace CIS.Core.Entities.Polices
{
    public class ExpiredLicenseHit : Hit
    {
        private License _licence;
        private Nullable<DateTime> _expiryDate;

        public virtual License License
        {
            get { return _licence; }
            set { _licence = value; }
        }

        public virtual Nullable<DateTime> ExpiryDate
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
