using System;
using System.Collections.Generic;
using System.Data.Entity.Design.PluralizationServices;
using System.Globalization;
using System.Linq;
using System.Text;
using FluentNHibernate.Conventions;
using FluentNHibernate.Conventions.Instances;

namespace CIS.Data.Conventions
{
    public class CustomHasManyConvention : IHasManyConvention
    {
        private readonly PluralizationService _pluralizationService;

        public CustomHasManyConvention()
        {
            _pluralizationService = PluralizationService.CreateService(new CultureInfo("en-US"));
        }

        public void Apply(IOneToManyCollectionInstance instance)
        {
            instance.Key.Column(instance.EntityType.Name + "Id");
            //instance.Cascade.AllDeleteOrphan();
        }
    }
}
