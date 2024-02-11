using CIS.Core.Domain.Common;
using FluentNHibernate.Mapping;
using NHibernate.Validator.Cfg.Loquacious;

namespace CIS.Data.Definition.Common;

public class BarcodeDefinition
{
    public class Mapping : ClassMap<Barcode>
    {
        public Mapping()
        {
            Id(x => x.Id);

            References(x => x.Image)
                .Cascade.All()
                .Fetch.Join();

            Map(x => x.Text);
        }
    }

    public class Validaton : ValidationDef<Barcode>
    {
        public Validaton()
        {
            Define(x => x.Id);

            Define(x => x.Image)
                .NotNullable()
                .And.IsValid();

            Define(x => x.Text)
                .NotNullableAndNotEmpty()
                .And.MaxLength(150);

        }
    }
}
