using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace CIS.Core.Entities.Commons
{
    public class Province
    {
        private Guid _id;
        private string _name;
        private Region _region;
        private ICollection<City> _cities;

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

        public virtual Region Region
        {
            get { return _region; }
            set { _region = value; }
        }

        public virtual IEnumerable<City> Cities
        {
            get { return _cities; }
            set { SyncCities(value); }
        }

        #region Methods

        private void SyncCities(IEnumerable<City> items)
        {
            foreach (var item in items)
                item.Province = this;
            
            var itemsToInsert = items.Except(_cities).ToList();
            var itemsToUpdate = _cities.Where(x => items.Contains(x)).ToList();
            var itemsToRemove = _cities.Except(items).ToList();

            // insert
            foreach (var item in itemsToInsert)
            {
                item.Province = this;
                _cities.Add(item);
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
                item.Province = null;
                _cities.Remove(item);
            }
        }

        public virtual void SerializeWith(Province value)
        {
            this.Name = value.Name;
            this.Region = value.Region;
        }

        public virtual void AddCity(City item)
        {
            item.Province = this;
            _cities.Add(item);
        }

        #endregion

        #region Constructors

        public Province()
        {
            _cities = new Collection<City>();
        }

        #endregion

        #region Equality Comparer

        private Nullable<int> _hashCode;

        public override bool Equals(object obj)
        {
            var that = obj as Province;

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

        public static bool operator ==(Province x, Province y)
        {
            return Equals(x, y);
        }

        public static bool operator !=(Province x, Province y)
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
