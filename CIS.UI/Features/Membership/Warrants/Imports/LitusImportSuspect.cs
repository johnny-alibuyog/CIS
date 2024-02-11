using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIS.UI.Features.Membership.Warrants.Imports
{
    public class LitusImportSuspect 
    {
        private string _warrantCode;
        private string _prefix;
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

        public virtual string Prefix
        {
            get { return _prefix; }
            set { _prefix = (value ?? string.Empty).Trim(); }
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

        internal static LitusImportSuspect OfDataRow(DataRow row)
        {
            return new()
            {
                WarrantCode = row.Field<string>("WA_CODE"),
                FirstName = row.Field<string>("FNAME"),
                MiddleName = row.Field<string>("MNAME"),
                LastName = row.Field<string>("LNAME"),
                Suffix = row.Field<string>("SUF"),
                AlsoKnownAs = row.Field<string>("ALIAS"),
            };
        }
    }
}
