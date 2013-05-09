using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;

namespace CIS.Core.Entities.Commons
{
    public class Region
    {
        private Guid _id;
        private string _name;
        private ICollection<Province> _provinces;

        public virtual Guid Id
        {
            get { return _id; }
            protected set { _id = value; }
        }

        public virtual string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        public virtual ICollection<Province> Provinces
        {
            get { return _provinces; }
            set { SyncProvinces(value); }
        }

        #region Methods

        private void SyncProvinces(ICollection<Province> items)
        {
            foreach (var item in items)
                item.Region = this;

            var itemsToInsert = items.Except(_provinces).ToList();
            var itemsToUpdate = _provinces.Where(x => items.Contains(x)).ToList();
            var itemsToRemove = _provinces.Except(items).ToList();

            // insert
            foreach (var item in itemsToInsert)
            {
                item.Region = this;
                _provinces.Add(item);
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
                item.Region = null;
                _provinces.Remove(item);
            }
        }

        public virtual void AddProvice(Province item)
        {
            item.Region = this;
            _provinces.Add(item);
        }

        #endregion

        #region Constructors

        public Region()
        {
            _provinces = new Collection<Province>();
        }

        #endregion

        #region Equality Comparer

        private Nullable<int> _hashCode;

        public override bool Equals(object obj)
        {
            var that = obj as Region;

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

        public static bool operator ==(Region x, Region y)
        {
            return Equals(x, y);
        }

        public static bool operator !=(Region x, Region y)
        {
            return !Equals(x, y);
        }

        #endregion

        #region Method Overrides

        public override string ToString()
        {
            return this.Name;
        }

        #endregion
    }
}