using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIS.Core.Domain.Membership
{
    public class SuspectHit : Hit
    {
        private Suspect _suspect;

        public virtual Suspect Suspect
        {
            get { return _suspect; }
            set { _suspect = value; }
        }

        protected internal virtual void SerializeWith(SuspectHit value)
        {
            this.HitScore = value.HitScore;
            this.IsIdentical = value.IsIdentical;
            this.Suspect = value.Suspect;
        }
    }
}
