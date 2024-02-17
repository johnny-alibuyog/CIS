﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CIS.Core.Domain.Common;
using FluentNHibernate.Mapping;

namespace CIS.Data.Definition.Common
{
    public class FaxMapping : SubclassMap<Fax>
    {
        public FaxMapping()
        {
            DiscriminatorValue("Fax");
        }
    }
}