using FluentNHibernate.Conventions;
using FluentNHibernate.Conventions.Instances;
using System.Data.Entity.Design.PluralizationServices;
using System.Globalization;

namespace CIS.Data.Conventions;

// Note: underscore was included to make this class on top of the naming order as it affects
//       the ussage sequence
public class _CustomJoinedSubclassConvention : IJoinedSubclassConvention
{
    private readonly PluralizationService _pluralizationService;

    public _CustomJoinedSubclassConvention()
    {
        _pluralizationService = PluralizationService.CreateService(new CultureInfo("en-US"));
    }

    public void Apply(IJoinedSubclassInstance instance)
    {
        instance.Table(_pluralizationService.Pluralize(instance.EntityType.Name));
        instance.Key.Column(instance.EntityType.Name + "Id");
    }
}
