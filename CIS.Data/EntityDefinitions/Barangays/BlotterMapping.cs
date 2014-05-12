using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CIS.Core.Entities.Barangays;
using CIS.Data.Commons.Extentions;
using FluentNHibernate.Mapping;

namespace CIS.Data.EntityDefinitions.Barangays
{
    public class BlotterMapping : ClassMap<Blotter>
    {
        public BlotterMapping()
        {
            OptimisticLock.Version();

            Id(x => x.Id);

            Version(x => x.Version);

            Component(x => x.Audit);

            Map(x => x.Complaint);

            Map(x => x.Details);

            Map(x => x.Remarks);

            Map(x => x.Status);

            Map(x => x.FiledOn);

            Map(x => x.OccuredOn);

            Component(x => x.Address);

            References(x => x.Incumbent);

            HasManyToMany(x => x.Officials)
                .Access.CamelCaseField(Prefix.Underscore)
                .Schema(GetType().ParseSchema())
                .Table("BlottersOfficials")
                //.Cascade.SaveUpdate()
                .AsSet();

            HasManyToMany(x => x.Complainants)
                .Access.CamelCaseField(Prefix.Underscore)
                .Schema(GetType().ParseSchema())
                .Table("BlottersComplainants")
                .Cascade.SaveUpdate()
                .AsSet();

            HasManyToMany(x => x.Respondents)
                .Access.CamelCaseField(Prefix.Underscore)
                .Schema(GetType().ParseSchema())
                .Table("BlottersRespondents")
                .Cascade.SaveUpdate()
                .AsSet();

            HasManyToMany(x => x.Witnesses)
                .Access.CamelCaseField(Prefix.Underscore)
                .Schema(GetType().ParseSchema())
                .Table("BlottersWitnesses")
                .Cascade.SaveUpdate()
                .AsSet();
        }
    }
}
