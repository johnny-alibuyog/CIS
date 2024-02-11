using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIS.Core.Entities.Commons
{
    public class BodyStatistics
    {
        private string _height;
        private string _weight;

        public virtual string Height
        {
            get { return _height; }
            set { _height = value; }
        }

        public virtual string Weight
        {
            get { return _weight; }
            set { _weight = value; }
        }

        #region Constructors

        public BodyStatistics() { }

        public BodyStatistics(string height, string weight)
        {
            _height = height;
            _weight = weight;
        }

        #endregion

        #region Equality Comparer

        private Nullable<int> _hashCode;

        public override bool Equals(object obj)
        {
            var that = obj as BodyStatistics;

            if (that == null)
                return false;

            if (that.Height != this.Height)
                return false;

            if (that.Weight != this.Weight)
                return false;

            return true;
        }

        public override int GetHashCode()
        {
            if (_hashCode == null)
            {
                unchecked
                {
                    _hashCode = 17;
                    _hashCode = _hashCode * 23 + (this.Height != null ? this.Height.GetHashCode() : 0);
                    _hashCode = _hashCode * 23 + (this.Weight != null ? this.Weight.GetHashCode() : 0);
                }
            }

            return _hashCode.Value;
        }

        public static bool operator ==(BodyStatistics x, BodyStatistics y)
        {
            return Equals(x, y);
        }

        public static bool operator !=(BodyStatistics x, BodyStatistics y)
        {
            return !Equals(x, y);
        }

        #endregion
    }
}
