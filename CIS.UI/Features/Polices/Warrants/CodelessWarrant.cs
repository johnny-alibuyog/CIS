using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIS.UI.Features.Polices.Warrants
{
    public class CodelessWarrant
    {
        private string _lastName;
        private string _middleName;
        private string _firstName;
        private string _suffix;
        private string _address;
        private string _case;
        private string _disposition;
        private Nullable<DateTime> _dateArrested;

        public virtual string LastName 
        {
            get { return _lastName; }
            set { _lastName = (value ?? string.Empty).Trim(); } 
        }

        public virtual string MiddleName
        {
            get { return _middleName; }
            set { _middleName = (value ?? string.Empty).Trim(); }
        }

        public virtual string FirstName
        {
            get { return _firstName; }
            set { _firstName = (value ?? string.Empty).Trim(); }
        }

        public virtual string Suffix
        {
            get { return _suffix; }
            set { _suffix = (value ?? string.Empty).Trim(); }
        }

        public virtual string Address
        {
            get { return _address; }
            set { _address = (value ?? string.Empty).Trim(); }
        }

        public virtual string Case
        {
            get { return _case; }
            set { _case = (value ?? string.Empty).Trim(); }
        }

        public virtual string Disposition
        {
            get { return _disposition; }
            set { _disposition = (value ?? string.Empty).Trim(); }
        }

        public virtual Nullable<DateTime> DateArrested
        {
            get { return _dateArrested; }
            set { _dateArrested = value; }
        }
    }
}
