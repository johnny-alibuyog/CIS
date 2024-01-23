using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace CIS.Core.Entities.Barangays;

public class Position : Entity<string>
{
    private string _name;
    private ICollection<Committee> _committees = new Collection<Committee>();

    public virtual string Name
    {
        get => _name;
        protected set => _name = value;
    }

    public virtual IEnumerable<Committee> Committees
    {
        get => _committees;
        set => SyncCommittees(value);
    }

    public Position() { }

    public Position(string id, string name)
        : this()
    {
        _id = id;
        _name = name;
    }

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

    #region Static Members

    public static readonly Position BarangayCaptain = new("C", "Captain");
    public static readonly Position BarangayCouncilor = new("L", "Councilor")
    {
        Committees = [
            Committee.CommitteeOnEducationAndInformation,
            Committee.CommitteeOnFinanceAndAppropriation,
            Committee.CommitteeOnHealthAndSanitation,
            Committee.CommitteeOnInfrastractures,
            Committee.CommitteeOnLawsRulesAndRegulations,
            Committee.CommitteeOnLivelihodAndAgriculture,
            Committee.CommitteeOnPeaceAndOrderAndPublicSafety,
        ]
    };
    public static readonly Position BarangaySecretary = new("S", "Secretary");
    public static readonly Position BarangayTreasurer = new("T", "Treasurer");
    public static readonly Position Kagawad = new("K", "Kagawad");
    public static readonly Position SKChairman = new("SKC", "SK Chairman");

    public static readonly IEnumerable<Position> All = [
        Position.BarangayCaptain,
        Position.BarangayCouncilor,
        Position.BarangaySecretary,
        Position.BarangayTreasurer,
        Position.Kagawad,
        Position.SKChairman
    ];

    #endregion
}
