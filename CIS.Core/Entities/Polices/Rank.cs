using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CIS.Core.Entities.Polices
{
    public class Rank
    {
        private string _id;
        private string _name;
        private RankCategory _category;

        public virtual string Id
        {
            get { return _id; }
            set { _id = value; }
        }

        public virtual string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        public virtual RankCategory Category
        {
            get { return _category; }
            set { _category = value; }
        }

        #region Constructors

        public Rank() { }

        public Rank(string id, string name, RankCategory category)
        {
            _id = id;
            _name = name;
            _category = category;
        }

        #endregion

        #region Static Members

        public static readonly Rank PoliceDirectorGeneral = new Rank("P D/Gen.", "Police Director General", RankCategory.CommissionedOfficer);
        public static readonly Rank PoliceDeputyDirectorGeneral = new Rank("P D/DGen.", "Police Deputy Director General", RankCategory.CommissionedOfficer);
        public static readonly Rank PoliceDirector = new Rank("P Dir.", "Police Director", RankCategory.CommissionedOfficer);
        public static readonly Rank PoliceChiefSuperintendent = new Rank("P C/Supt.", "Police Chief Superintendent", RankCategory.CommissionedOfficer);
        public static readonly Rank PoliceSeniorSuperintendent = new Rank("P S/Supt.", "Police Senior Superintendent", RankCategory.CommissionedOfficer);
        public static readonly Rank PoliceSuperintendent = new Rank("P Supt.", "Police Superintendent", RankCategory.CommissionedOfficer);
        public static readonly Rank PoliceChiefInspector = new Rank("P C/Insp.", "Police Chief Inspector", RankCategory.CommissionedOfficer);
        public static readonly Rank PoliceSeniorInspector = new Rank("P S/Insp.", "Police Senior Inspector", RankCategory.CommissionedOfficer);
        public static readonly Rank PoliceInspector = new Rank("P Insp.", "Police Inspector", RankCategory.CommissionedOfficer);

        public static readonly Rank SeniorPoliceOfficerIV = new Rank("SPO4", "Senior Police Officer IV", RankCategory.NonCommissionedOfficer);
        public static readonly Rank SeniorPoliceOfficerIII = new Rank("SPO3", "Senior Police Officer III", RankCategory.NonCommissionedOfficer);
        public static readonly Rank SeniorPoliceOfficerII = new Rank("SPO2", "Senior Police Officer II", RankCategory.NonCommissionedOfficer);
        public static readonly Rank SeniorPoliceOfficerI = new Rank("SPO1", "Senior Police Officer I", RankCategory.NonCommissionedOfficer);
        public static readonly Rank PoliceOfficerIII = new Rank("PO3", "Police Officer III", RankCategory.NonCommissionedOfficer);
        public static readonly Rank PoliceOfficerII = new Rank("PO2", "Police Officer II", RankCategory.NonCommissionedOfficer);
        public static readonly Rank PoliceOfficerI = new Rank("PO1", "Police Officer I", RankCategory.NonCommissionedOfficer);

        public static readonly Rank[] All = new Rank[]
        {
            PoliceDirectorGeneral,
            PoliceDeputyDirectorGeneral,
            PoliceDirector,
            PoliceChiefSuperintendent,
            PoliceSeniorSuperintendent,
            PoliceSuperintendent,
            PoliceChiefInspector,
            PoliceSeniorInspector,
            PoliceInspector,
            SeniorPoliceOfficerIV,
            SeniorPoliceOfficerIII,
            SeniorPoliceOfficerII,
            SeniorPoliceOfficerI,
            PoliceOfficerIII,
            PoliceOfficerII,
            PoliceOfficerI
        };

        #endregion

        #region Equality Comparer

        private Nullable<int> _hashCode;

        public override bool Equals(object obj)
        {
            var that = obj as Rank;

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

        public static bool operator ==(Rank x, Rank y)
        {
            return Equals(x, y);
        }

        public static bool operator !=(Rank x, Rank y)
        {
            return !Equals(x, y);
        }

        #endregion
    }
}
