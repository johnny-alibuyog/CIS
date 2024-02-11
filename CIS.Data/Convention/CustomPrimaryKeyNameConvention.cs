using FluentNHibernate.Conventions;
using FluentNHibernate.Conventions.Instances;

namespace CIS.Data.Convention;

public class CustomPrimaryKeyNameConvention : IIdConvention
{
    public void Apply(IIdentityInstance instance)
    {
        instance.Column(instance.EntityType.Name + "Id");
    }
}
