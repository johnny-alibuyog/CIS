using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CIS.Core.Entities.Commons;

namespace CIS.Core.Entities.Barangays
{
    public class Complaint
    {
        private Guid _id;
        private string _description;
        private string _detailedSummary;
        private Blotter _blotter;

        public virtual Guid Id
        {
            get { return _id; }
            set { _id = value; }
        }

        public virtual string Description
        {
            get { return _description; }
            set { _description = value; }
        }

        public virtual string DetailedSummary
        {
            get { return _detailedSummary; }
            set { _detailedSummary = value; }
        }

        public virtual Blotter Blotter
        {
            get { return _blotter; }
            set { _blotter = value; }
        }

        #region Equality Comparer

        private Nullable<int> _hashCode;

        public override bool Equals(object obj)
        {
            var that = obj as Complaint;

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

        public static bool operator ==(Complaint x, Complaint y)
        {
            return Equals(x, y);
        }

        public static bool operator !=(Complaint x, Complaint y)
        {
            return !Equals(x, y);
        }

        #endregion

    }
}
