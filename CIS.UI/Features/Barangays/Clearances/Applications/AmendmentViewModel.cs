using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NHibernate.Validator.Constraints;

namespace CIS.UI.Features.Barangays.Clearances.Applications
{
    public class AmendmentViewModel : ViewModelBase
    {
        public virtual Guid ApproverUserId { get; set; }

        public virtual string DocumentNumber { get; set; }

        [NotNullNotEmpty(Message = "Reason is mandatory.")]
        public virtual string Reason { get; set; }

        public virtual string Remarks { get; set; }

        public virtual bool IsApproved
        {
            get { return this.ApproverUserId != Guid.Empty; }
        }
    }
}
