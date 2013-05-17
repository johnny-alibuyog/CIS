using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIS.Core.Entities.Firearms
{
    public class Gun
    {
        private string _model;
        private string _caliber;
        private string _serialNumber;
        private Kind _kind;
        private Make _make;

        public virtual string Model
        {
            get { return _model; }
            set { _model = value; }
        }

        public virtual string Caliber
        {
            get { return _caliber; }
            set { _caliber = value; }
        }

        public virtual string SerialNumber
        {
            get { return _serialNumber; }
            set { _serialNumber = value; }
        }

        public virtual Kind Kind
        {
            get { return _kind; }
            set { _kind = value; }
        }

        public virtual Make Make
        {
            get { return _make; }
            set { _make = value; }
        }

        #region Equality Comparer

        private Nullable<int> _hashCode;

        public override bool Equals(object obj)
        {
            var that = obj as Gun;

            if (that == null)
                return false;

            if (that.Model != this.Model)
                return false;

            if (that.Caliber != this.Caliber)
                return false;

            if (that.SerialNumber != this.SerialNumber)
                return false;

            if (that.Kind != this.Kind)
                return false;

            if (that.Make != this.Make)
                return false;

            return true;

            //if (that.Id == Guid.Empty && this.Id == Guid.Empty)
            //    return object.ReferenceEquals(that, this);

            //return (that.Id == this.Id);
        }

        public override int GetHashCode()
        {
            if (_hashCode == null)
            {
                unchecked
                {
                    _hashCode = 17;
                    _hashCode = _hashCode * 23 + (this.Model != null ? this.Model.GetHashCode() : 0);
                    _hashCode = _hashCode * 23 + (this.Caliber != null ? this.Caliber.GetHashCode() : 0);
                    _hashCode = _hashCode * 23 + (this.SerialNumber != null ? this.SerialNumber.GetHashCode() : 0);
                    _hashCode = _hashCode * 23 + (this.Kind != null ? this.Kind.GetHashCode() : 0);
                    _hashCode = _hashCode * 23 + (this.Make != null ? this.Make.GetHashCode() : 0);
                }
            }

            return _hashCode.Value;
        }

        public static bool operator ==(Gun x, Gun y)
        {
            return Equals(x, y);
        }

        public static bool operator !=(Gun x, Gun y)
        {
            return !Equals(x, y);
        }

        #endregion
    }
}
