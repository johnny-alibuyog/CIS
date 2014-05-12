using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CIS.Core.Entities.Commons;

namespace CIS.Core.Entities.Barangays
{
    public class Official
    {
        private Guid _id;
        private int _version;
        private Audit _audit;
        private Person _person;
        private Position _position;
        private Committee _committee;
        private Incumbent _incumbent;
        private ImageBlob _picture;
        private ImageBlob _signature;
        private bool _isActive;

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

        public virtual Position Position
        {
            get { return _position; }
            set { _position = value; }
        }

        public virtual Committee Committee
        {
            get { return _committee; }
            set { _committee = value; }
        }

        public virtual Incumbent Incumbent
        {
            get { return _incumbent; }
            set { _incumbent = value; }
        }

        public virtual ImageBlob Picture
        {
            get { return _picture; }
            set { _picture = value; }
        }

        public virtual ImageBlob Signature
        {
            get { return _signature; }
            set { _signature = value; }
        }

        public virtual bool IsActive
        {
            get { return _isActive; }
            set { _isActive = value; }
        }

        #region Methods

        public virtual void SerializeWith(Official value)
        {
            this.Person = value.Person;
            this.Position = value.Position;
            this.Committee = value.Committee;
            this.Incumbent = value.Incumbent;
            this.Picture.Image = value.Picture.Image;
            this.Signature.Image = value.Signature.Image;
            this.IsActive = value.IsActive;
        }

        #endregion

        #region Constructor

        public Official()
        {
            _person = new Person();
            _picture = new ImageBlob();
            _signature = new ImageBlob();
        }

        #endregion

        #region Equality Comparer

        private Nullable<int> _hashCode;

        public override bool Equals(object obj)
        {
            var that = obj as Official;

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

        public static bool operator ==(Official x, Official y)
        {
            return Equals(x, y);
        }

        public static bool operator !=(Official x, Official y)
        {
            return !Equals(x, y);
        }

        #endregion

    }
}
