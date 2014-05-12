using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CIS.Core.Entities.Memberships;

namespace CIS.Core.Entities.Barangays
{
    public class Amendment
    {
        private Guid _id;
        private User _approver;
        private string _documentNumber;
        private string _reason;
        private string _remarks;

        public virtual Guid Id
        {
            get { return _id; }
            protected set { _id = value; }
        }

        public virtual User Approver
        {
            get { return _approver; }
            set { _approver = value; }
        }

        public virtual string DocumentNumber
        {
            get { return _documentNumber; }
            set { _documentNumber = value; }
        }

        public virtual string Reason
        {
            get { return _reason; }
            set { _reason = value; }
        }

        public virtual string Remarks
        {
            get { return _remarks; }
            set { _remarks = value; }
        }

        #region Equality Comparer

        private Nullable<int> _hashCode;

        public override bool Equals(object obj)
        {
            var that = obj as Amendment;

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

        public static bool operator ==(Amendment x, Amendment y)
        {
            return Equals(x, y);
        }

        public static bool operator !=(Amendment x, Amendment y)
        {
            return !Equals(x, y);
        }

        #endregion
    }
}
