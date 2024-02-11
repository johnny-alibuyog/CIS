namespace CIS.Core.Domain.Membership;

public class SignatoryRole : Entity<string>
{
    private string _name;

    public virtual string Name
    {
        get => _name;
        set => _name = value;
    }

    public SignatoryRole() { }

    public SignatoryRole(string id, string name)
    {
        this.Id = id;
        this.Name = name;
    }

    public override string ToString()
    {
        return this.Name;
    }

    public static SignatoryRole Sponsor = new("Sponsor", "Sponsor");
    public static SignatoryRole Endorser = new("Endorser", "Endorser");
    public static SignatoryRole Concurrer = new("Concurrer", "Concurrer");
    public static SignatoryRole Approver = new("Approver", "Approver");
}
