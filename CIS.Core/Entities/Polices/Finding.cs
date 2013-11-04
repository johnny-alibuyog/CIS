using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIS.Core.Entities.Polices
{
    public class Finding
    {
        private Guid _id;
        private string _finalfindings;
        private Amendment _amendment;
        private ICollection<Hit> _hits;

        public virtual Guid Id
        {
            get { return _id; }
            protected set { _id = value; }
        }

        public virtual string FinalFindings
        {
            get { return _finalfindings; }
            set { _finalfindings = value; }
        }

        public virtual Amendment Amendment
        {
            get { return _amendment; }
            set { _amendment = value; }
        }

        public virtual IEnumerable<Hit> Hits
        {
            get { return _hits; }
            set { SyncHits(value); }
        }

        private void SyncHits(IEnumerable<Hit> items)
        {
            foreach (var item in items)
                item.Finding = this;

            var itemsToInsert = items.Except(_hits).ToList();
            var itemsToUpdate = _hits.Where(x => items.Contains(x)).ToList();
            var itemsToRemove = _hits.Except(items).ToList();

            // insert
            foreach (var item in itemsToInsert)
            {
                _hits.Add(item);
            }

            // update
            foreach (var item in itemsToUpdate)
            {
                var value = items.Single(x => x == item);

                if (item is SuspectHit)
                    ((SuspectHit)item).SerializeWith((SuspectHit)value);

                if (item is ExpiredLicenseHit)
                    ((ExpiredLicenseHit)item).SerializeWith((ExpiredLicenseHit)value);
            }

            // delete
            foreach (var item in itemsToRemove)
            {
                _hits.Remove(item);
            }
        }

        public Finding()
        {
            _hits = new Collection<Hit>();
        }

        #region Methods

        private void SyncSuspectPartialMatches(IEnumerable<Hit> items)
        {
            var itemsToInsert = items.Except(_hits).ToList();
            var itemsToUpdate = _hits.Where(x => items.Contains(x)).ToList();
            var itemsToRemove = _hits.Except(items).ToList();

            // insert
            foreach (var item in itemsToInsert)
                _hits.Add(item);

            // update
            foreach (var item in itemsToUpdate)
            {
                var value = items.Single(x => x == item);
                item.IsIdentical = value.IsIdentical;
            }

            // delete
            foreach (var item in itemsToRemove)
                _hits.Remove(item);
        }

        #endregion

        #region Equality Comparer

        private Nullable<int> _hashCode;

        public override bool Equals(object obj)
        {
            var that = obj as Finding;

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

        public static bool operator ==(Finding x, Finding y)
        {
            return Equals(x, y);
        }

        public static bool operator !=(Finding x, Finding y)
        {
            return !Equals(x, y);
        }

        #endregion
    }
}
