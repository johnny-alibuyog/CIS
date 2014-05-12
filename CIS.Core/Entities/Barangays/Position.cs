using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace CIS.Core.Entities.Barangays
{
    public class Position
    {
        private string _id;
        private string _name;
        private ICollection<Committee> _committees;

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

        public virtual IEnumerable<Committee> Committees
        {
            get { return _committees; }
            set { SyncCommittees(value); }
        }

        #region Constructors

        public Position() 
        {
            _committees = new Collection<Committee>();
        }

        public Position(string id, string name)
            : this()
        {
            _id = id;
            _name = name;
        }

        #endregion

        #region Methods

        private void SyncCommittees(IEnumerable<Committee> items)
        {
            foreach (var item in items)
                item.Position = this;

            var itemsToInsert = items.Except(_committees).ToList();
            var itemsToUpdate = _committees.Where(x => items.Contains(x)).ToList();
            var itemsToRemove = _committees.Except(items).ToList();

            // insert
            foreach (var item in itemsToInsert)
            {
                item.Position = this;
                _committees.Add(item);
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
                item.Position = null;
                _committees.Remove(item);
            }
        }

        #endregion

        #region Static Members

        public static readonly Position BarangayCaptain = new Position("C", "Captain");
        public static readonly Position BarangayCouncilor = new Position("L", "Councilor")
        {
            Committees = new Collection<Committee>()
            {
                Committee.CommitteeOnEducationAndInformation,
                Committee.CommitteeOnFinanceAndAppropriation,
                Committee.CommitteeOnHealthAndSanitation,
                Committee.CommitteeOnInfrastractures,
                Committee.CommitteeOnLawsRulesAndRegulations,
                Committee.CommitteeOnLivelihodAndAgriculture,
                Committee.CommitteeOnPeaceAndOrderAndPublicSafety,
            }
        };
        public static readonly Position BarangaySecretary = new Position("S", "Secretary");
        public static readonly Position BarangayTreasurer = new Position("T", "Treasurer");
        public static readonly Position Kagawad = new Position("K", "Kagawad");
        public static readonly Position SKChairman = new Position("SKC", "SK Chairman");

        public static readonly IEnumerable<Position> All = new Position[] 
        { 
            Position.BarangayCaptain, 
            Position.BarangayCouncilor,
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
