﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIS.UI.Features.Firearms.Licenses.Registrations
{
    public class LicenseListItemViewModel : ViewModelBase
    {
        public virtual Guid Id { get; set; }

        public virtual string Owner { get; set; }

        public virtual string Gun { get; set; }

        public virtual Nullable<DateTime> ExpiryDate { get; set; }
    }
}