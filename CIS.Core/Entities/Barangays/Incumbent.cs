using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using CIS.Core.Entities.Commons;

namespace CIS.Core.Entities.Barangays
{
    public class Incumbent
    {
        private Guid _id;
        private int _version;
        private Audit _audit;
        private Office _office;
        private ICollection<Official> _officials;
        private Nullable<DateTime> _date;

        public virtual Guid Id
        {
            get { return _id; }
            protected set { _id = value; }
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

        public virtual IEnumerable<Official> Officials
        {
            get { return _officials; }
            set { SyncOfficials(value); }
        }

        public virtual Nullable<DateTime> Date
        {
            get { return _date; }
            set { _date = value; }
        }

        #region Methods

        private void SyncOfficials(IEnumerable<Official> items)
        {
            var itemsToInsert = items.Except(_officials).ToList();
            var itemsToUpdate = _officials.Where(x => items.Contains(x)).ToList();
            var itemsToRemove = _officials.Except(items).ToList();

            // insert
            foreach (var item in itemsToInsert)
            {
                _officials.Add(item);
            }

            // update
            foreach (var item in itemsToUpdate)
            {
                var value = items.Single(x => x == item);
                item.SerializeWith(value);
            }

            // delete
            foreach (var item in itemsToRemove)
            {
                _officials.Remove(item);
            }
        }

        #endregion

        #region Constructors

        public Incumbent()
        {
            _officials = new Collection<Official>();
            _date = DateTime.Today;
        }

        #endregion

        #region Equality Comparer

        private Nullable<int> _hashCode;

        public override bool Equals(object obj)
        {
            var that = obj as Incumbent;

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

        public static bool operator ==(Incumbent x, Incumbent y)
        {
            return Equals(x, y);
        }

        public static bool operator !=(Incumbent x, Incumbent y)
        {
            return !Equals(x, y);
        }

        #endregion
    }
}
