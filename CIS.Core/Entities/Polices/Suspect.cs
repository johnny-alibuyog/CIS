using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using CIS.Core.Entities.Commons;

namespace CIS.Core.Entities.Polices
{
    public class Suspect
    {
        private Guid _id;
        //private int _version;
        private Audit _audit;
        private Warrant _warrant;
        private Nullable<ArrestStatus> _arrestStatus;
        private Nullable<DateTime> _arrestDate;
        private Nullable<long> _dataStoreId;
        private string _disposition;
        private Person _person;
        private Address _address;
        private PhysicalAttributes _physicalAttributes;
        private ICollection<string> _aliases;
        private ICollection<string> _occupations;

        public virtual Guid Id
        {
            get { return _id; }
            set { _id = value; }
        }

        //public virtual int Version
        //{
        //    get { return _version; }
        //    protected set { _version = value; }
        //}

        public virtual Audit Audit
        {
            get { return _audit; }
            protected set { _audit = value; }
        }

        public virtual Warrant Warrant
        {
            get { return _warrant; }
            set { _warrant = value; }
        }

        public virtual Nullable<ArrestStatus> ArrestStatus
        {
            get { return _arrestStatus; }
            set { _arrestStatus = value; }
        }

        public virtual Nullable<DateTime> ArrestDate
        {
            get { return _arrestDate; }
            set { _arrestDate = value; }
        }

        public virtual Nullable<long> DataStoreId
        {
            get { return _dataStoreId; }
            set { _dataStoreId = value; }
        }

        public virtual string Disposition
        {
            get { return _disposition; }
            set { _disposition = value; }
        }

        public virtual Person Person
        {
            get { return _person; }
            set { _person = value; }
        }

        public virtual Address Address
        {
            get { return _address; }
            set { _address = value; }
        }

        public virtual PhysicalAttributes PhysicalAttributes
        {
            get { return _physicalAttributes; }
            set { _physicalAttributes = value; }
        }

        public virtual IEnumerable<string> Aliases
        {
            get { return _aliases; }
            set { SyncAliases(value); }
        }

        public virtual IEnumerable<string> Occupations
        {
            get { return _occupations; }
            set { SyncOccupations(value); }
        }

        #region Methods

        public virtual void SerializeWith(Suspect value)
        {
            this.Warrant = value.Warrant;
            this.ArrestStatus = value.ArrestStatus;
            this.Person = value.Person;
            this.Address = value.Address;
            this.PhysicalAttributes = value.PhysicalAttributes;
            this.Aliases = value.Aliases;
            this.Occupations = value.Occupations;
        }

        private void SyncAliases(IEnumerable<string> items)
        {
            items = items.Where(x => !string.IsNullOrWhiteSpace(x));

            var itemsToInsert = items.Except(_aliases).ToList();
            var itemsToRemove = _aliases.Except(items).ToList();

            // insert
            foreach (var item in itemsToInsert)
                _aliases.Add(item);

            // delete
            foreach (var item in itemsToRemove)
                _aliases.Remove(item);
        }

        private void SyncOccupations(IEnumerable<string> items)
        {
            var itemsToInsert = items = items.Where(x => !string.IsNullOrWhiteSpace(x));

            items.Except(_occupations).ToList();
            var itemsToRemove = _occupations.Except(items).ToList();

            // insert
            foreach (var item in itemsToInsert)
                _occupations.Add(item);

            // delete
            foreach (var item in itemsToRemove)
                _occupations.Remove(item);
        }

        #endregion

        #region Constructors

        public Suspect()
        {
            _aliases = new Collection<string>();
            _occupations = new Collection<string>();
        }

        #endregion

        #region Equality Comparer

        private Nullable<int> _hashCode;

        public override bool Equals(object obj)
        {
            var that = obj as Suspect;

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

        public static bool operator ==(Suspect x, Suspect y)
        {
            return Equals(x, y);
        }

        public static bool operator !=(Suspect x, Suspect y)
        {
            return !Equals(x, y);
        }

        #endregion
    }
}
