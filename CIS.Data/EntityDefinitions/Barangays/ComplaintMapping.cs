﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CIS.Core.Entities.Barangays;
using FluentNHibernate.Mapping;

namespace CIS.Data.EntityDefinitions.Barangays
{
    public class ComplaintMapping : ClassMap<Complaint>
    {
        public ComplaintMapping()
        {
            Id(x => x.Id);

            Map(x => x.Description);

            Map(x => x.DetailedSummary);
        }
    }
}