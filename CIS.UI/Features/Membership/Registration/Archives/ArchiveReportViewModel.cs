﻿using System;
using System.Collections.Generic;

namespace CIS.UI.Features.Membership.Registration.Archives;

public class ArchiveReportViewModel : ViewModelBase
{
    public virtual string Office { get; set; }

    public virtual string Station { get; set; }

    public virtual string Location { get; set; }

    public virtual bool FilterDate { get; set; }

    public virtual DateTime FromDate { get; set; }

    public virtual DateTime ToDate { get; set; }

    public virtual IList<ArchiveItemViewModel> Items { get; set; }

}