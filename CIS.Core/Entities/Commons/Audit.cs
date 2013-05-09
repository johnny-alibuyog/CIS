using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CIS.Core.Entities.Commons
{
    public class Audit
    {
        private string _createdBy;
        private string _updatedBy;
        private Nullable<DateTime> _createdOn;
        private Nullable<DateTime> _updatedOn;

        public virtual string CreatedBy
        {
            get { return _createdBy; }
        }

        public virtual string UpdatedBy
        {
            get { return _updatedBy; }
        }

        public virtual Nullable<DateTime> CreatedOn
        {
            get { return _createdOn; }
        }

        public virtual Nullable<DateTime> UpdatedOn
        {
            get { return _updatedOn; }
        }

        #region Constructors

        public Audit()
        {
            _createdBy = string.Empty;
            _createdOn = DateTime.Now;
        }

        public Audit(string createdBy, DateTime createdOn)
        {
            _createdBy = createdBy;
            _createdOn = createdOn;
        }

        public Audit(Audit currentAudit, string updatedBy, DateTime updatedOn)
        {
            _createdBy = currentAudit != null ? currentAudit.CreatedBy : updatedBy;
            _createdOn = currentAudit != null ? currentAudit.CreatedOn : updatedOn;

            _updatedBy = updatedBy;
            _updatedOn = updatedOn;
        }

        #endregion

        #region Equality Comparer

        private Nullable<int> _hashCode;

        public override bool Equals(object obj)
        {
            var that = obj as Audit;

            if (that == null)
                return false;

            if (that.CreatedBy != this.CreatedBy)
                return false;

            if (that.CreatedOn != this.CreatedOn)
                return false;

            if (that.UpdatedBy != this.UpdatedBy)
                return false;

            if (that.UpdatedOn != this.UpdatedOn)
                return false;

            return true;
        }

        public override int GetHashCode()
        {
            if (_hashCode == null)
            {
                unchecked
                {
                    _hashCode = 17;
                    _hashCode = _hashCode * 23 + (this.CreatedBy != null ? this.CreatedBy.GetHashCode() : 0);
                    _hashCode = _hashCode * 23 + (this.CreatedOn != null ? this.CreatedOn.GetHashCode() : 0);
                    _hashCode = _hashCode * 23 + (this.UpdatedBy != null ? this.UpdatedBy.GetHashCode() : 0);
                    _hashCode = _hashCode * 23 + (this.UpdatedOn != null ? this.UpdatedOn.GetHashCode() : 0);
                }
            }

            return _hashCode.Value;
        }

        public static bool operator ==(Audit x, Audit y)
        {
            return Equals(x, y);
        }

        public static bool operator !=(Audit x, Audit y)
        {
            return !Equals(x, y);
        }

        #endregion
    }
}
