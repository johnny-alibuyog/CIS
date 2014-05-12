using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIS.Core.Entities.Barangays
{
    public class BlotterHit : Hit
    {
        private Citizen _respondent;
        private Blotter _blotter;

        public virtual Citizen Respondent
        {
            get { return _respondent; }
            set { _respondent = value; }
        }

        public virtual Blotter Blotter
        {
            get { return _blotter; }
            set { _blotter = value; }
        }

        protected internal virtual void SerializeWith(BlotterHit value)
        {
            this.HitScore = value.HitScore;
            this.IsIdentical = value.IsIdentical;
            this.Respondent = value.Respondent;
            this.Blotter = value.Blotter;
        }
    }
}
