using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CIS.Core.Entities.Commons;
using CIS.Core.Utilities.Extentions;
using NHibernate.Validator.Constraints;

namespace CIS.UI.Features.Commons.Persons
{
    public class BasicPersonViewModel : ViewModelBase
    {
        [NotNullNotEmpty(Message = "First name is mandatory.")]
        public virtual string FirstName { get; set; }

        public virtual string MiddleName { get; set; }

        [NotNullNotEmpty(Message = "Last name is mandatory.")]
        public virtual string LastName { get; set; }

        public virtual string Suffix { get; set; }

        public virtual string FullName
        {
            get 
            { 
                return string.Format("{0} {1} {2}", 
                    this.FirstName ?? string.Empty, 
                    this.MiddleName ?? string.Empty, 
                    this.LastName ?? string.Empty
                )
                .ToProperCase(); 
            }
        }

        public override string ToString()
        {
            return this.FullName;
        }

        public override object SerializeWith(object instance)
        {
            if (instance == null)
                return null;

            if (instance is BasicPersonViewModel)
            {
                var source = instance as BasicPersonViewModel;
                var target = this;

                target.FirstName = source.FirstName;
                target.MiddleName = source.MiddleName;
                target.LastName = source.LastName;
                target.Suffix = source.Suffix;
                return target;
            }
            else if (instance is Person)
            {
                var source = instance as Person;
                var target = this;

                target.FirstName = source.FirstName;
                target.MiddleName = source.MiddleName;
                target.LastName = source.LastName;
                target.Suffix = source.Suffix;
                return target;
            }

            return null;
        }

        public override object DeserializeInto(object instance)
        {
            if (instance == null)
                return null;

            if (instance is BasicPersonViewModel)
            {
                var source = this;
                var target = instance as BasicPersonViewModel;

                target.SerializeWith(source);
                return target;
            }
            else if (instance is Person)
            {
                var source = this;
                var target = instance as Person;

                target.FirstName = source.FirstName;
                target.MiddleName = source.MiddleName;
                target.LastName = source.LastName;
                target.Suffix = source.Suffix;

                return target;
            }

            return null;
        }
    }
}
