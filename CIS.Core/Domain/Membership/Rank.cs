using CIS.Core.Utility.Extention;

namespace CIS.Core.Domain.Membership;

public class Rank : Entity<string>
{
    private readonly bool _enableProperCasing = false;
    
    private string _name;
    private RankCategory _category;

    public virtual string Name
    {
        get => _name;
        set => _name = _enableProperCasing ? value.ToProperCase() : value;
    }

    public virtual RankCategory Category
    {
        get => _category;
        set => _category = value;
    }

    public Rank() { }

    public Rank(string id, string name, RankCategory category, bool enableProperCasing = false)
    {
        this._enableProperCasing = enableProperCasing;

        this.Id = id;
        this.Name = name;
        this.Category = category;
    }

    public static readonly Rank PoliceDirectorGeneral = new("P D/Gen.", "Police Director General", RankCategory.CommissionedOfficer);
    public static readonly Rank PoliceDeputyDirectorGeneral = new("P D/DGen.", "Police Deputy Director General", RankCategory.CommissionedOfficer);
    public static readonly Rank PoliceDirector = new("P Dir.", "Police Director", RankCategory.CommissionedOfficer);
    public static readonly Rank PoliceChiefSuperintendent = new("P C/Supt.", "Police Chief Superintendent", RankCategory.CommissionedOfficer);
    public static readonly Rank PoliceSeniorSuperintendent = new("P S/Supt.", "Police Senior Superintendent", RankCategory.CommissionedOfficer);
    public static readonly Rank PoliceSuperintendent = new("P Supt.", "Police Superintendent", RankCategory.CommissionedOfficer);
    public static readonly Rank PoliceChiefInspector = new("P C/Insp.", "Police Chief Inspector", RankCategory.CommissionedOfficer);
    public static readonly Rank PoliceSeniorInspector = new("P S/Insp.", "Police Senior Inspector", RankCategory.CommissionedOfficer);
    public static readonly Rank PoliceInspector = new("P Insp.", "Police Inspector", RankCategory.CommissionedOfficer);

    public static readonly Rank SeniorPoliceOfficerIV = new("SPO4", "Senior Police Officer IV", RankCategory.NonCommissionedOfficer);
    public static readonly Rank SeniorPoliceOfficerIII = new("SPO3", "Senior Police Officer III", RankCategory.NonCommissionedOfficer);
    public static readonly Rank SeniorPoliceOfficerII = new("SPO2", "Senior Police Officer II", RankCategory.NonCommissionedOfficer);
    public static readonly Rank SeniorPoliceOfficerI = new("SPO1", "Senior Police Officer I", RankCategory.NonCommissionedOfficer);
    public static readonly Rank PoliceOfficerIII = new("PO3", "Police Officer III", RankCategory.NonCommissionedOfficer);
    public static readonly Rank PoliceOfficerII = new("PO2", "Police Officer II", RankCategory.NonCommissionedOfficer);
    public static readonly Rank PoliceOfficerI = new("PO1", "Police Officer I", RankCategory.NonCommissionedOfficer);

    public static readonly Rank[] All =
    [
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
    ];
}
