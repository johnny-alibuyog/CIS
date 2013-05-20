﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CIS.Core.Entities.Commons;

namespace CIS.Core.Entities.Commons
{
    public class FingerPrint
    {
        private Guid _id;
        private ImageBlob _rightThumb;
        private ImageBlob _rightIndex;
        private ImageBlob _rightMiddle;
        private ImageBlob _rightRing;
        private ImageBlob _rightPinky;
        private ImageBlob _leftThumb;
        private ImageBlob _leftIndex;
        private ImageBlob _leftMiddle;
        private ImageBlob _leftRing;
        private ImageBlob _leftPinky;

        public virtual Guid Id
        {
            get { return _id; }
            protected set { _id = value; }
        }

        public virtual ImageBlob RightThumb
        {
            get { return _rightThumb; }
            set { _rightThumb = value; }
        }

        public virtual ImageBlob RightIndex
        {
            get { return _rightIndex; }
            set { _rightIndex = value; }
        }

        public virtual ImageBlob RightMiddle
        {
            get { return _rightMiddle; }
            set { _rightMiddle = value; }
        }

        public virtual ImageBlob RightRing
        {
            get { return _rightRing; }
            set { _rightRing = value; }
        }

        public virtual ImageBlob RightPinky
        {
            get { return _rightPinky; }
            set { _rightPinky = value; }
        }

        public virtual ImageBlob LeftThumb
        {
            get { return _leftThumb; }
            set { _leftThumb = value; }
        }

        public virtual ImageBlob LeftIndex
        {
            get { return _leftIndex; }
            set { _leftIndex = value; }
        }

        public virtual ImageBlob LeftMiddle
        {
            get { return _leftMiddle; }
            set { _leftMiddle = value; }
        }

        public virtual ImageBlob LeftRing
        {
            get { return _leftRing; }
            set { _leftRing = value; }
        }

        public virtual ImageBlob LeftPinky
        {
            get { return _leftPinky; }
            set { _leftPinky = value; }
        }

        #region Constructors

        public FingerPrint()
        {
            RightThumb = new ImageBlob();
            RightIndex = new ImageBlob();
            RightMiddle = new ImageBlob();
            RightRing = new ImageBlob();
            RightPinky = new ImageBlob();
            LeftThumb = new ImageBlob();
            LeftIndex = new ImageBlob();
            LeftMiddle = new ImageBlob();
            LeftRing = new ImageBlob();
            LeftPinky = new ImageBlob();
        }

        #endregion

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
