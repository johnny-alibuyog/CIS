using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CIS.Core.Entities.Commons;

namespace CIS.Core.Entities.Commons
{
    public class Picture
    {
        private Guid _id;
        private ImageBlob _image;

        public virtual Guid Id
        {
            get { return _id; }
            protected set { _id = value; }
        }

        public virtual ImageBlob Image
        {
            get { return _image; }
            set { _image = value; }
        }

        public Picture()
        {
            Image = new ImageBlob();
        }

        #region Equality Comparer

        private Nullable<int> _hashCode;

        public override bool Equals(object obj)
        {
            var that = obj as Picture;

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

        public static bool operator ==(Picture x, Picture y)
        {
            return Equals(x, y);
        }

        public static bool operator !=(Picture x, Picture y)
        {
            return !Equals(x, y);
        }

        #endregion
    }
}
