using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CIS.Core.Entities.Commons;
using CIS.Core.Utilities.Extentions;

namespace CIS.Core.Entities.Commons
{
    public class PersonBasic
    {
        private string _prefix;
        private string _firstName;
        private string _middleName;
        private string _lastName;
        private string _suffix;

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

        public virtual string Fullname
        {
            get { return this.GetFullName(); }
        }

        public virtual void SerializeWith(PersonBasic value)
        {
            this.Prefix = value.Prefix;
            this.FirstName = value.FirstName;
            this.MiddleName = value.MiddleName;
            this.LastName = value.LastName;
            this.Suffix = value.Suffix;
        }

        //public virtual void SerializeWith(PersonBasic value)
        //{
        //    this.FirstName = value.FirstName;
        //    this.MiddleName = value.MiddleName;
        //    this.LastName = value.LastName;
        //    this.Suffix = value.Suffix;
        //}

        private string GetFullName()
        {
            return
            (
                (!string.IsNullOrWhiteSpace(this.Prefix) ? this.Prefix : string.Empty) +
                (!string.IsNullOrWhiteSpace(this.FirstName) ? " " + this.FirstName : string.Empty) +
                (!string.IsNullOrWhiteSpace(this.MiddleName) ? " " + this.MiddleName : string.Empty) +
                (!string.IsNullOrWhiteSpace(this.LastName) ? " " + this.LastName : string.Empty) +
                (!string.IsNullOrWhiteSpace(this.Suffix) ? " " + this.Suffix : string.Empty)
            )
            .Trim();
        }

        public override string ToString()
        {
            return this.GetFullName();
        }
    }
}
