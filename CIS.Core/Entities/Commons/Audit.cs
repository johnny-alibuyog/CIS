﻿using System;
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
            protected set { _createdBy = value; }
        }

        public virtual string UpdatedBy
        {
            get { return _updatedBy; }
            protected set { _updatedBy = value; }
        }

        public virtual Nullable<DateTime> CreatedOn
        {
            get { return _createdOn; }
            protected set { _createdOn = value; }
        }

        public virtual Nullable<DateTime> UpdatedOn
        {
            get { return _updatedOn; }
            protected set { _updatedOn = value; }
        }

        #region Static Members

        public static Audit Create(string createdBy, DateTime createdOn)
        {
            return new Audit()
            {
                CreatedBy = createdBy,
                CreatedOn = createdOn
            };
        }

        public static Audit Create(string updatedBy, DateTime updatedOn, Audit currentAudit)
        {
            return new Audit()
            {
                CreatedBy = currentAudit != null 
                    ? currentAudit.CreatedBy : updatedBy,
                CreatedOn = currentAudit != null 
                    ? currentAudit.CreatedOn : updatedOn,
                UpdatedBy = updatedBy,
                UpdatedOn = updatedOn
            };
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
