using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using CIS.Core.Utilities.Extentions;

namespace CIS.Core.Entities.Commons
{
    public class City : Entity<Guid>
    {
        private string _name;
        private Province _province;
        private ICollection<Barangay> _barangays = new Collection<Barangay>();

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

        public override string ToString()
        {
            return this.Name;
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
    }
}
