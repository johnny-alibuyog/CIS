using CIS.Core.Domain.Membership;
using FluentNHibernate.Mapping;

namespace CIS.Data.Definition.Membership;

public class MemberMembershipInfoDefinition
{
    public class Mapping : ClassMap<MemberMembershipInfo>
    {
        public Mapping()
        {
            Id(x => x.Id)
                .GeneratedBy.Foreign("MembershipInfo");

            HasOne(x => x.Member)
                .Constrained();

            References(x => x.Status);

            References(x => x.Position);

            HasMany(x => x.Developments)
                .Access.CamelCaseField(Prefix.Underscore)
                .Cascade.AllDeleteOrphan()
                .Not.KeyNullable()
                .Not.KeyUpdate()
                .Inverse()
                .AsSet();
        }
    }
}
