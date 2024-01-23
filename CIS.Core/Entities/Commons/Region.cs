using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using CIS.Core.Utilities.Extentions;

namespace CIS.Core.Entities.Commons
{
    public class Region : Entity<Guid>
    {
        private string _name;
        private ICollection<Province> _provinces = new Collection<Province>();

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

        public virtual ICollection<Province> Provinces
        {
            get { return _provinces; }
            set { SyncProvinces(value); }
        }

        public override string ToString()
        {
            return this.Name;
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
    }
}
