using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CIS.Core.Entities.Commons
{
    public class Barangay
    {
        private Guid _id;
        private string _name;
        private City _city;
        private Nullable<AreaClass> _areaClass;
        private Nullable<int> _population;

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

        public virtual City City
        {
            get { return _city; }
            set { _city = value; }
        }

        public virtual Nullable<AreaClass> AreaClass
        {
            get { return _areaClass; }
            set { _areaClass = value; }
        }

        public virtual Nullable<int> Population
        {
            get { return _population; }
            set { _population = value; }
        }

        #region Methods

        public virtual void SerializeWith(Barangay value)
        {
            this.Name = value.Name;
            this.City = value.City;
            this.Population = value.Population;
            this.AreaClass = value.AreaClass;
        }

        #endregion

        #region Equality Comparer

        private Nullable<int> _hashCode;

        public override bool Equals(object obj)
        {
            var that = obj as Barangay;

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

        public static bool operator ==(Barangay x, Barangay y)
        {
            return Equals(x, y);
        }

        public static bool operator !=(Barangay x, Barangay y)
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
