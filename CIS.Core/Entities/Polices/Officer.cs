using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CIS.Core.Entities.Commons;
using CIS.Core.Utilities.Extentions;

namespace CIS.Core.Entities.Polices
{
    public class Officer
    {
        private Guid _id;
        private int _version;
        private Audit _audit;
        private Person _person;
        private Station _station;
        private Rank _rank;
        private string _position;
        private ImageBlob _signature;

        public virtual Guid Id
        {
            get { return _id; }
            set { _id = value; }
        }

        public virtual int Version
        {
            get { return _version; }
            protected set { _version = value; }
        }

        public virtual Audit Audit
        {
            get { return _audit; }
            set { _audit = value; }
        }

        public virtual Person Person
        {
            get { return _person; }
            set { _person = value; }
        }

        public virtual Station Station
        {
            get { return _station; }
            set { _station = value; }
        }

        public virtual Rank Rank
        {
            get { return _rank; }
            set { _rank = value; }
        }

        public virtual string Position
        {
            get { return _position; }
            set { _position = value.ToProperCase(); }
        }

        public virtual ImageBlob Signature
        {
            get { return _signature; }
            set { _signature = value; }
        }

        #region Methods

        public virtual void SerializeWith(Officer value)
        {
            this.Person = value.Person;
            this.Station = value.Station;
            this.Rank = value.Rank;
            this.Position = value.Position;
            this.Signature = value.Signature;
        }

        #endregion

        #region Conaructors

        public Officer()
        {
            _signature = new ImageBlob();
        }

        #endregion

        #region Equality Comparer

        private Nullable<int> _hashCode;

        public override bool Equals(object obj)
        {
            var that = obj as Officer;

            if (that == null)
                return false;

            if (that.Id == Guid.Empty && this.Id == Guid.Empty)
                return object.ReferenceEquals(that, this);

            return (that.Id == this.Id);
        }

        public override int GetHashCode()
        {
            if (_hashCode == null)
            {
                _hashCode = (this.Id != Guid.Empty)
                    ? this.Id.GetHashCode()
                    : base.GetHashCode();
            }

            return _hashCode.Value;
        }

        public static bool operator ==(Officer x, Officer y)
        {
            return Equals(x, y);
        }

        public static bool operator !=(Officer x, Officer y)
        {
            return !Equals(x, y);
        }

        #endregion
    }
}
