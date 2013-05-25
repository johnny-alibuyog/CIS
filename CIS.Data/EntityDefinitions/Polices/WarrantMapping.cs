using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CIS.Core.Entities.Polices;
using FluentNHibernate.Mapping;

namespace CIS.Data.EntityDefinition.Polices
{
    public class WarrantMapping : ClassMap<Warrant>
    {
        public WarrantMapping()
        {
            //OptimisticLock.Version();

            Id(x => x.Id);

            //Version(x => x.Version);

            Component(x => x.Audit);

            Map(x => x.WarrantCode)
                .Index("WarrantCodeIndex");

            Map(x => x.CaseNumber);

            Map(x => x.Crime);

            Map(x => x.Description);

            Map(x => x.Remarks);

            Map(x => x.BailAmount)
                .Precision(25)
                .Scale(4);

            Map(x => x.IssuedOn);

            Map(x => x.IssuedBy);

            Component(x => x.IssuedAt)
                .ColumnPrefix("IssuedAt");

            HasMany(x => x.Suspects)
                .Access.CamelCaseField(Prefix.Underscore)
                .Cascade.AllDeleteOrphan()
                .Not.KeyNullable()
                .Not.KeyUpdate()
                .Inverse()
                .AsSet();
        }
    }
}
