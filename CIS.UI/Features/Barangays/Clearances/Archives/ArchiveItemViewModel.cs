﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIS.UI.Features.Barangays.Clearances.Archives
{
    public class ArchiveItemViewModel //: ViewModelBase
    {
        public virtual Guid Id { get; set; }

        public virtual Nullable<DateTime> IssueDate { get; set; }

        public virtual string OfficalReceiptNumber { get; set; }

        public virtual string ControlNumber { get; set; }

        public virtual string Applicant { get; set; }

        public virtual string Purpose { get; set; }
    }
}
