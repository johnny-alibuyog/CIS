using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Markup;

namespace CIS.UI.Utilities.Extentions
{
    public class EnumerationExtention : MarkupExtension
    {
        private Type _enumType;


        public EnumerationExtention(Type enumType)
        {
            if (enumType == null)
                throw new ArgumentNullException("enumType");

            EnumType = enumType;
        }

        public Type EnumType
        {
            get { return _enumType; }
            private set
            {
                if (_enumType == value)
                    return;

                var enumType = Nullable.GetUnderlyingType(value) ?? value;

                if (enumType.IsEnum == false)
                    throw new ArgumentException("Type must be an Enum.");

                _enumType = value;
            }
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            var enumValues = Enum.GetValues(EnumType);

            return enumValues.OfType<object>()
                .Select(x => new EnumerationMember()
                {
                    Value = x,
                    Description = GetDescription(x)
                })
                .ToArray();

            //return (
            //  from object enumValue in enumValues
            //  select new EnumerationMember
            //  {
            //      Value = enumValue,
            //      Description = GetDescription(enumValue)
            //  }).ToArray();
        }

        private string GetDescription(object enumValue)
        {
            var descriptionAttribute = EnumType
              .GetField(enumValue.ToString())
              .GetCustomAttributes(typeof(DescriptionAttribute), false)
              .FirstOrDefault() as DescriptionAttribute;


            return descriptionAttribute != null
              ? descriptionAttribute.Description
              : enumValue.ToString();
        }

        public class EnumerationMember
        {
            public virtual string Description { get; set; }
            public virtual object Value { get; set; }
        }
    }
}
