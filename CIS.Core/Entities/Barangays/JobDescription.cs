using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIS.Core.Entities.Barangays
{
    public class JobDescription
    {
        private Guid _id;
        private string _name;
        private Position _position;

        public virtual Guid Id
        {
            get { return _id; }
            protected set { _id = value; }
        }

        public virtual string Name
        {
            get { return _name; }
            protected set { _name = value; }
        }

        public virtual Position Position
        {
            get { return _position; }
            set { _position = value; }
        }

        public virtual void SerializeWith(JobDescription value)
        {
            this.Name = value.Name;
            this.Position = value.Position;
        }

        #region Constructor

        public JobDescription() { }

        public JobDescription(string name) : this()
        {
            this.Name = name;
        }

        #endregion

        #region Equality Comparer

        private Nullable<int> _hashCode;

        public override bool Equals(object obj)
        {
            var that = obj as JobDescription;

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

        public static bool operator ==(JobDescription x, JobDescription y)
        {
            return Equals(x, y);
        }

        public static bool operator !=(JobDescription x, JobDescription y)
        {
            return !Equals(x, y);
        }

        #endregion
    }
}
