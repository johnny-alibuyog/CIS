using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using CIS.Core.Utilities.Extentions;

namespace CIS.Core.Entities.Commons
{
    public class City
    {
        private Guid _id;
        private string _name;
        private Province _province;
        private ICollection<Barangay> _barangays;

        public virtual Guid Id
        {
            get { return _id; }
            protected set { _id = value; }
        }

        public virtual string Name
        {
            get { return _name; }
            set { _name = value.ToProperCase(); }
        }

        public virtual Province Province
        {
            get { return _province; }
            set { _province = value; }
        }

        public virtual IEnumerable<Barangay> Barangays
        {
            get { return _barangays; }
            set { SyncBarangays(value); }
        }

        #region Methods

        public virtual void SerializeWith(City value)
        {
            this.Name = value.Name;
            this.Province = value.Province;
        }

        private void SyncBarangays(IEnumerable<Barangay> items)
        {
            foreach (var item in items)
                item.City = this;

            var itemsToInsert = items.Except(_barangays).ToList();
            var itemsToUpdate = _barangays.Where(x => items.Contains(x)).ToList();
            var itemsToRemove = _barangays.Except(items).ToList();

            // insert
            foreach (var item in itemsToInsert)
            {
                item.City = this;
                _barangays.Add(item);
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
                item.City = null;
                _barangays.Remove(item);
            }
        }


        public virtual void AddBarangay(Barangay item)
        {
            item.City = this;
            _barangays.Add(item);
        }


        #endregion

        #region Constructors

        public City()
        {
            _barangays = new Collection<Barangay>();
        }

        #endregion

        #region Equality Comparer

        private int? _hashCode;

        public override bool Equals(object obj)
        {
            var that = obj as City;

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

        public static bool operator ==(City x, City y)
        {
            return Equals(x, y);
        }

        public static bool operator !=(City x, City y)
        {
            return !Equals(x, y);
        }

        #endregion

        #region Method Override

        public override string ToString()
        {
            return this.Name;
        }

        #endregion
    }
}
