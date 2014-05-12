using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CIS.Core.Entities.Commons;

namespace CIS.Core.Entities.Barangays
{
    public abstract class Hit
    {
        private Guid _id;
        private Finding _finding;
        private HitScore _hitScore;
        private bool _isIdentical;

        public virtual Guid Id
        {
            get { return _id; }
            protected set { _id = value; }
        }

        public virtual Finding Finding
        {
            get { return _finding; }
            set { _finding = value; }
        }

        public virtual HitScore HitScore
        {
            get { return _hitScore; }
            set { _hitScore = value; }
        }

        public virtual bool IsIdentical
        {
            get { return _isIdentical; }
            set { _isIdentical = value; }
        }

        #region Equality Comparer

        private Nullable<int> _hashCode;

        public override bool Equals(object obj)
        {
            var that = obj as Amendment;

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

        public static bool operator ==(Hit x, Hit y)
        {
            return Equals(x, y);
        }

        public static bool operator !=(Hit x, Hit y)
        {
            return !Equals(x, y);
        }

        #endregion
    }
}
