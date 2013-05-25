using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIS.UI.Features.Polices.Warrants
{
    public class ImportSuspectViewModel 
    {
        private string _warrantCode;
        private string _firstName;
        private string _middleName;
        private string _lastName;
        private string _suffix;
        private string _alsoKnownAs;

        public virtual string WarrantCode
        {
            get { return _warrantCode; }
            set { _warrantCode = (value ?? string.Empty).Trim(); }
        }

        public virtual string FirstName
        {
            get { return _firstName; }
            set { _firstName = (value ?? string.Empty).Trim(); }
        }

        public virtual string MiddleName
        {
            get { return _middleName; }
            set { _middleName = (value ?? string.Empty).Trim(); }
        }

        public virtual string LastName
        {
            get { return _lastName; }
            set { _lastName = (value ?? string.Empty).Trim(); }
        }

        public virtual string Suffix
        {
            get { return _suffix; }
            set { _suffix = (value ?? string.Empty).Trim(); }
        }

        public virtual string AlsoKnownAs
        {
            get { return _alsoKnownAs; }
            set { _alsoKnownAs = (value ?? string.Empty).Trim(); }
        }
    }
}
