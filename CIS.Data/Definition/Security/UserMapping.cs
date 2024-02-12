using CIS.Core.Domain.Security;
using CIS.Data.Common.Extention;
using FluentNHibernate.Mapping;

namespace CIS.Data.Definition.Security;

public class UserMapping : ClassMap<User>
{
    public UserMapping()
    {
        OptimisticLock.Version();

        Id(x => x.Id);

        Version(x => x.Version);

        Map(x => x.Username)
            .Unique();

        Map(x => x.Password);

        Map(x => x.Email);

        Component(x => x.Person);

        HasMany(x => x.Roles)
            .Access.CamelCaseField(Prefix.Underscore)
            .Schema(GetType().ParseSchema())
            .Table("UsersRoles")
            .Element("Role")
            .AsSet();

        //HasManyToMany(x => x.Roles)
        //    .Access.CamelCaseField(Prefix.Underscore)
        //    .Schema(GetType().ParseSchema())
        //    .Table("UsersRoles")
        //    .AsSet();
    }
}
