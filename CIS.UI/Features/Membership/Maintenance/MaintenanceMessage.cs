namespace CIS.UI.Features.Membership.Maintenance;

public class MaintenanceMessage(string identifier)
{
    public virtual string Identifier { get; set; } = identifier;
}
