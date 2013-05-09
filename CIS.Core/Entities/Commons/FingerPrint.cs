using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CIS.Core.Entities.Commons;

namespace CIS.Core.Entities.Commons
{
    public class FingerPrint
    {
        private Guid _id;
        private Blob _rightThumb;
        private Blob _rightIndex;
        private Blob _rightMiddle;
        private Blob _rightRing;
        private Blob _rightPinky;
        private Blob _leftThumb;
        private Blob _leftIndex;
        private Blob _leftMiddle;
        private Blob _leftRing;
        private Blob _leftPinky;

        public virtual Guid Id
        {
            get { return _id; }
            protected set { _id = value; }
        }

        public virtual Blob RightThumb
        {
            get { return _rightThumb; }
            set { _rightThumb = value; }
        }

        public virtual Blob RightIndex
        {
            get { return _rightIndex; }
            set { _rightIndex = value; }
        }

        public virtual Blob RightMiddle
        {
            get { return _rightMiddle; }
            set { _rightMiddle = value; }
        }

        public virtual Blob RightRing
        {
            get { return _rightRing; }
            set { _rightRing = value; }
        }

        public virtual Blob RightPinky
        {
            get { return _rightPinky; }
            set { _rightPinky = value; }
        }

        public virtual Blob LeftThumb
        {
            get { return _leftThumb; }
            set { _leftThumb = value; }
        }

        public virtual Blob LeftIndex
        {
            get { return _leftIndex; }
            set { _leftIndex = value; }
        }

        public virtual Blob LeftMiddle
        {
            get { return _leftMiddle; }
            set { _leftMiddle = value; }
        }

        public virtual Blob LeftRing
        {
            get { return _leftRing; }
            set { _leftRing = value; }
        }

        public virtual Blob LeftPinky
        {
            get { return _leftPinky; }
            set { _leftPinky = value; }
        }

        #region Equality Comparer

        private Nullable<int> _hashCode;

        public override bool Equals(object obj)
        {
            var that = obj as FingerPrint;

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

        public static bool operator ==(FingerPrint x, FingerPrint y)
        {
            return Equals(x, y);
        }

        public static bool operator !=(FingerPrint x, FingerPrint y)
        {
            return !Equals(x, y);
        }

        #endregion
    }
}
