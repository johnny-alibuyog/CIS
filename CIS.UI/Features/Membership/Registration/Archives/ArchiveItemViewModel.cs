using System;

namespace CIS.UI.Features.Membership.Registration
{
    public class ArchiveItemViewModel //: ViewModelBase
    {
        public virtual Guid Id { get; set; }

        public virtual DateTime? IssueDate { get; set; }

        public virtual string OfficalReceiptNumber { get; set; }

        public virtual string ControlNumber { get; set; }

        public virtual string Applicant { get; set; }

        public virtual string Purpose { get; set; }
    }
}
