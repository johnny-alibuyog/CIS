using System.Collections.Generic;

namespace CIS.Core.Entities.Barangays;

public class Committee : Entity<string>
{
    private string _name;
    private Position _position;

    public virtual string Name
    {
        get => _name;
        protected set => _name = value;
    }

    public virtual Position Position
    {
        get => _position;
        set => _position = value;
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
}
