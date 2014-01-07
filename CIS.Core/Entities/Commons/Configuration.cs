using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIS.Core.Entities.Commons
{
    public class Configuration
    {
        private Guid _id;
        private IDictionary<string, string> _properties;

        public virtual Guid Id
        {
            get { return _id; }
            protected set { _id = value; }
        }

        public virtual IDictionary<string, string> Properties
        {
            get { return _properties; }
            set { _properties = value; }
        }

        protected virtual string GetPropertyValue(string key)
        {
            return this.Properties.ContainsKey(key) ? this.Properties[key] : null;
        }

        protected virtual IEnumerable<string> GetListPropertyValue(string key)
        {
            return this.Properties.Where(x => x.Key.StartsWith(key)).Select(x => x.Value);
        }

        protected virtual void SetPropertyValue(string key, string value)
        {
            if (this.Properties.ContainsKey(key))
                this.Properties[key] = value;
            else
                this.Properties.Add(key, value);
        }

        protected virtual void SetListPropertyValue(string key, IEnumerable<string> items)
        {
            var originalItems = this.GetListPropertyValue(key);
            var itemsToInsert = items.Except(originalItems).ToList();
            var itemsToRemove = originalItems.Except(items).ToList();

            // insert
            foreach (var item in itemsToInsert)
                this.Properties.Add(key + item, item);

            // remove
            foreach (var item in itemsToRemove)
                this.Properties.Remove(key + item);
        }

        public Configuration()
        {
            _properties = new Dictionary<string, string>();
        }

        #region Equality Comparer

        private Nullable<int> _hashCode;

        public override bool Equals(object obj)
        {
            var that = obj as Configuration;

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

        public static bool operator ==(Configuration x, Configuration y)
        {
            return Equals(x, y);
        }

        public static bool operator !=(Configuration x, Configuration y)
        {
            return !Equals(x, y);
        }

        #endregion
    }
}
