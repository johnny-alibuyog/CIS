using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIS.Core.Entities.Firearms
{
    public class Make
    {
        private Guid _id;
        private string _name;

        public virtual Guid Id
        {
            get { return _id; }
            set { _id = value; }
        }

        public virtual string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        #region Equality Comparer

        private Nullable<int> _hashCode;

        public override bool Equals(object obj)
        {
            var that = obj as Make;

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

        public static bool operator ==(Make x, Make y)
        {
            return Equals(x, y);
        }

        public static bool operator !=(Make x, Make y)
        {
            return !Equals(x, y);
        }

        #endregion
    }
}
