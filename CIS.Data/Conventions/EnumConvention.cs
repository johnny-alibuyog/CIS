
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Conventions;
using FluentNHibernate.Conventions.AcceptanceCriteria;
using FluentNHibernate.Conventions.Inspections;
using FluentNHibernate.Conventions.Instances;

namespace CIS.Data.Conventions
{
    public class EnumConvention : IPropertyConvention, IPropertyConventionAcceptance
    {
        #region IPropertyConvention Members

        public void Apply(IPropertyInstance instance)
        {
            //instance.CustomType(instance.Property.PropertyType); // map to int
            instance.Length(50);
        }

        #endregion

        #region IPropertyConventionAcceptance Members

        public void Accept(IAcceptanceCriteria<IPropertyInspector> criteria)
        {
            criteria.Expect(
                x => x.Property.PropertyType.IsEnum ||
                (
                    x.Property.PropertyType.IsGenericType &&
                    x.Property.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>) &&
                    x.Property.PropertyType.GetGenericArguments()[0].IsEnum
                )
            );
        }

        #endregion
    }
}
