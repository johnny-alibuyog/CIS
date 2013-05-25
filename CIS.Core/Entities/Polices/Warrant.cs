using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using CIS.Core.Entities.Commons;

namespace CIS.Core.Entities.Polices
{
    public class Warrant
    {
        private Guid _id;
        //private int _version;
        private Audit _audit;
        private string _warrantCode;
        private string _caseNumber;
        private string _crime;
        private string _description;
        private string _remarks;
        private decimal _bailAmount;
        private Nullable<DateTime> _issuedOn;
        private string _issuedBy;
        private Address _issuedAt;
        private ICollection<Suspect> _suspects;

        public virtual Guid Id
        {
            get { return _id; }
            protected set { _id = value; }
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

        public virtual string WarrantCode
        {
            get { return _warrantCode; }
            set { _warrantCode = value; }
        }

        public virtual string CaseNumber
        {
            get { return _caseNumber; }
            set { _caseNumber = value; }
        }

        public virtual string Crime
        {
            get { return _crime; }
            set { _crime = value; }
        }

        public virtual string Description
        {
            get { return _description; }
            set { _description = value; }
        }

        public virtual string Remarks
        {
            get { return _remarks; }
            set { _remarks = value; }
        }

        public virtual decimal BailAmount
        {
            get { return _bailAmount; }
            set { _bailAmount = value; }
        }

        public virtual Nullable<DateTime> IssuedOn
        {
            get { return _issuedOn; }
            set { _issuedOn = value; }
        }

        public virtual string IssuedBy
        {
            get { return _issuedBy; }
            set { _issuedBy = value; }
        }

        public virtual Address IssuedAt
        {
            get { return _issuedAt; }
            set { _issuedAt = value; }
        }

        public virtual IEnumerable<Suspect> Suspects
        {
            get { return _suspects; }
            set { SyncSuspects(value); }
        }

        #region Methods

        private void SyncSuspects(IEnumerable<Suspect> items)
        {
            foreach (var item in items)
                item.Warrant = this;

            var itemsToInsert = items.Except(_suspects).ToList();
            var itemsToUpdate = _suspects.Where(x => items.Contains(x)).ToList();
            var itemsToRemove = _suspects.Except(items).ToList();

            // insert
            foreach (var item in itemsToInsert)
            {
                item.Warrant = this;
                _suspects.Add(item);
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
                item.Warrant = null;
                _suspects.Remove(item);
            }
        }

        public virtual void AddSuspect(Suspect item)
        {
            item.Warrant = this;
            _suspects.Add(item);
        }

        public virtual void DeleteSuspect(Suspect item)
        {
            item.Warrant = null;
            _suspects.Remove(item);
        }

        #endregion

        #region Constructors

        public Warrant()
        {
            _suspects = new Collection<Suspect>();
        }

        #endregion

        #region Equality Comparer

        private Nullable<int> _hashCode;

        public override bool Equals(object obj)
        {
            var that = obj as Warrant;

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

        public static bool operator ==(Warrant x, Warrant y)
        {
            return Equals(x, y);
        }

        public static bool operator !=(Warrant x, Warrant y)
        {
            return !Equals(x, y);
        }

        #endregion
    }
}
