using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CIS.Core.Entities.Barangays
{
    public class Position
    {
        private string _id;
        private string _name;

        public virtual string Id
        {
            get { return _id; }
            protected set { _id = value; }
        }

        public virtual string Name
        {
            get { return _name; }
            protected set { _name = value; }
        }

        #region Constructors

        public Position() { }

        public Position(string id, string name)
        {
            _id = id;
            _name = name;
        }

        #endregion

        #region Static Members

        public static readonly Position BarangayCaptain = new Position("BC", "Barangay Captain");
        public static readonly Position BarangaySecretary = new Position("BS", "Barangay Secretary");
        public static readonly Position BarangayTreasurer = new Position("BT", "Barangay Treasurer");
        public static readonly Position Kagawad = new Position("K", "Kagawad");
        public static readonly Position SKChairman = new Position("SKC", "SK Chairman");
        public static readonly IEnumerable<Position> All = new Position[] 
        { 
            Position.BarangayCaptain, 
            Position.BarangaySecretary,
            Position.BarangayTreasurer, 
            Position.Kagawad, 
            Position.SKChairman
        };

        #endregion

        #region Equality Comparer

        private Nullable<int> _hashCode;

        public override bool Equals(object obj)
        {
            var that = obj as Position;

            if (that == null)
                return false;

            if (string.IsNullOrWhiteSpace(that.Id) && string.IsNullOrWhiteSpace(this.Id))
                return object.ReferenceEquals(that, this);

            return (that.Id == this.Id);
        }

        public override int GetHashCode()
        {
            if (_hashCode == null)
            {
                _hashCode = (!string.IsNullOrWhiteSpace(this.Id))
                    ? this.Id.GetHashCode()
                    : base.GetHashCode();
            }

            return _hashCode.Value;
        }

        public static bool operator ==(Position x, Position y)
        {
            return Equals(x, y);
        }

        public static bool operator !=(Position x, Position y)
        {
            return !Equals(x, y);
        }

        #endregion
    }
}
