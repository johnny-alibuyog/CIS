using CIS.Data.Commons.Extentions;
using FluentNHibernate.Conventions;
using FluentNHibernate.Conventions.AcceptanceCriteria;
using FluentNHibernate.Conventions.Inspections;
using FluentNHibernate.Conventions.Instances;
using System.Data.Entity.Design.PluralizationServices;
using System.Globalization;

namespace CIS.Data.Conventions;

public class CustomTableNameConvention : IClassConvention, IClassConventionAcceptance
{
    private readonly PluralizationService _pluralizer = PluralizationService.CreateService(new CultureInfo("en-US"));

    public void Apply(IClassInstance instance)
    {
        //var schema = instance.EntityType.Namespace.Split('.').Last();
        var schema = instance.EntityType.ParseSchema();
        var tableName = _pluralizer.Pluralize(instance.EntityType.Name);

        instance.Schema(schema);
        instance.Table(tableName);
    }

    public void Accept(IAcceptanceCriteria<IClassInspector> criteria)
    {
        //criteria.Expect(x => x.TableName, Is.Not.Set);
    }
}
