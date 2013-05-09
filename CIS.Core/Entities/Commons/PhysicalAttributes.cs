using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CIS.Core.Entities.Commons
{
    public class PhysicalAttributes
    {
        private string _hair;
        private string _eyes;
        private string _build;
        private string _complexion;
        private string _scarsAndMarks;
        private string _race;
        private string _nationality;

        public virtual string Hair
        {
            get { return _hair; }
            set { _hair = value; }
        }

        public virtual string Eyes
        {
            get { return _eyes; }
            set { _eyes = value; }
        }

        public virtual string Build
        {
            get { return _build; }
            set { _build = value; }
        }

        public virtual string Complexion
        {
            get { return _complexion; }
            set { _complexion = value; }
        }

        public virtual string ScarsAndMarks
        {
            get { return _scarsAndMarks; }
            set { _scarsAndMarks = value; }
        }

        public virtual string Race
        {
            get { return _race; }
            set { _race = value; }
        }

        public virtual string Nationality
        {
            get { return _nationality; }
            set { _nationality = value; }
        }

        #region Constructors

        public PhysicalAttributes() { }

        public PhysicalAttributes(string hair, string eyes, string build, string complexion, string scarsAndMarks, string race, string nationality)
        {
            _hair = hair;
            _eyes = eyes;
            _build = build;
            _complexion = complexion;
            _scarsAndMarks = scarsAndMarks;
            _race = race;
            _nationality = nationality;
        }

        #endregion

        #region Equality Comparer

        private Nullable<int> _hashCode;

        public override bool Equals(object obj)
        {
            var that = obj as PhysicalAttributes;

            if (that == null)
                return false;

            if (that.Hair != this.Hair)
                return false;

            if (that.Eyes != this.Eyes)
                return false;

            if (that.Build != this.Build)
                return false;

            if (that.Complexion != this.Complexion)
                return false;

            if (that.ScarsAndMarks != this.ScarsAndMarks)
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
                    _hashCode = _hashCode * 23 + (this.Hair != null ? this.Hair.GetHashCode() : 0);
                    _hashCode = _hashCode * 23 + (this.Eyes != null ? this.Eyes.GetHashCode() : 0);
                    _hashCode = _hashCode * 23 + (this.Build != null ? this.Build.GetHashCode() : 0);
                    _hashCode = _hashCode * 23 + (this.Complexion != null ? this.Complexion.GetHashCode() : 0);
                    _hashCode = _hashCode * 23 + (this.ScarsAndMarks != null ? this.ScarsAndMarks.GetHashCode() : 0);
                }
            }

            return _hashCode.Value;
        }

        public static bool operator ==(PhysicalAttributes x, PhysicalAttributes y)
        {
            return Equals(x, y);
        }

        public static bool operator !=(PhysicalAttributes x, PhysicalAttributes y)
        {
            return !Equals(x, y);
        }

        #endregion
    }
}
