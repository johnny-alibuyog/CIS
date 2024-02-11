using FluentNHibernate.Conventions;
using FluentNHibernate.Conventions.Instances;

namespace CIS.Data.Convention;

public class CustomHasManyConvention : IHasManyConvention
{
    public void Apply(IOneToManyCollectionInstance instance)
    {
        instance.Key.Column(instance.EntityType.Name + "Id");
        //instance.Cascade.AllDeleteOrphan();
    }
}
