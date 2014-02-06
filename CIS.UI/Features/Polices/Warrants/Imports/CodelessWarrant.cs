using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CIS.Core.Utilities.Extentions;

namespace CIS.UI.Features.Polices.Warrants.Imports
{
    public class CodelessWarrant
    {
        private string _prefix;
        private string _firstName;
        private string _middleName;
        private string _lastName;
        private string _suffix;
        private string _address;
        private string _case;
        private string _disposition;
        private Nullable<DateTime> _arrestedOn;

        public virtual string Prefix
        {
            get { return _prefix; }
            set { _prefix = value.ToProperCase(); }
        }

        public virtual string FirstName
        {
            get { return _firstName; }
            set { _firstName = value.ToProperCase(); }
        }

        public virtual string MiddleName
        {
            get { return _middleName; }
            set { _middleName = value.ToProperCase(); }
        }

        public virtual string LastName
        {
            get { return _lastName; }
            set { _lastName = value.ToProperCase(); }
        }
        
        public virtual string Suffix
        {
            get { return _suffix; }
            set { _suffix = value.ToProperCase(); }
        }

        public virtual string Address
        {
            get { return _address; }
            set { _address = value.ToProperCase(); }
        }

        public virtual string Case
        {
            get { return _case; }
            set { _case = value.ToProperCase(); }
        }

        public virtual string Disposition
        {
            get { return _disposition; }
            set { _disposition = value.ToProperCase(); }
        }

        public virtual Nullable<DateTime> ArrestedOn
        {
            get { return _arrestedOn; }
            set { _arrestedOn = value; }
        }

        public override string ToString()
        {
            return string.Format("{0} {1} {2} {3} {4}",
                this.Prefix ?? string.Empty,
                this.FirstName ?? string.Empty,
                this.MiddleName ?? string.Empty,
                this.LastName ?? string.Empty,
                this.Suffix ?? string.Empty
            )
            .ToProperCase();
        }
    }
}
