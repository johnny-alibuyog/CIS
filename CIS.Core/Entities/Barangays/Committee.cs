using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIS.Core.Entities.Barangays
{
    public class Committee
    {
        private string _id;
        private string _name;
        private Position _position;

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

        public virtual Position Position
        {
            get { return _position; }
            set { _position = value; }
        }

        public virtual void SerializeWith(Committee value)
        {
            this.Name = value.Name;
            this.Position = value.Position;
        }

        #region Constructor

        public Committee() { }

        public Committee(string id, string name) : this()
        {
            this.Id = id;
            this.Name = name;
        }

        #endregion

        #region Static Members

        public static Committee CommitteeOnEducationAndInformation = new Committee("CEI", "Committee on Education and Information");
        public static Committee CommitteeOnFinanceAndAppropriation = new Committee("CFA", "Committee on Finance and Appropriation");
        public static Committee CommitteeOnHealthAndSanitation = new Committee("CHS", "Committee on Health and Sanitation");
        public static Committee CommitteeOnInfrastractures = new Committee("CI", "Committee on Infrastractures");
        public static Committee CommitteeOnLawsRulesAndRegulations = new Committee("CLRR", "Committee on Laws, Rules and Regulations");
        public static Committee CommitteeOnLivelihodAndAgriculture = new Committee("CLA", "Committee on Livelihod and Agriculture");
        public static Committee CommitteeOnPeaceAndOrderAndPublicSafety = new Committee("CPOPS", "Committee on Peace and Order and Public Safety");
        public static IEnumerable<Committee> All = new Committee[]
        {
            Committee.CommitteeOnEducationAndInformation,
            Committee.CommitteeOnFinanceAndAppropriation,
            Committee.CommitteeOnHealthAndSanitation,
            Committee.CommitteeOnInfrastractures,
            Committee.CommitteeOnLawsRulesAndRegulations,
            Committee.CommitteeOnLivelihodAndAgriculture,
            Committee.CommitteeOnPeaceAndOrderAndPublicSafety,
        };

        #endregion

        #region Equality Comparer

        private Nullable<int> _hashCode;

        public override bool Equals(object obj)
        {
            var that = obj as Committee;

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

        public static bool operator ==(Committee x, Committee y)
        {
            return Equals(x, y);
        }

        public static bool operator !=(Committee x, Committee y)
        {
            return !Equals(x, y);
        }

        #endregion
    }
}
