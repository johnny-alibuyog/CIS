namespace CIS.UI.Features.Polices.Maintenances;

public class MaintenanceMessage(string identifier)
{
    public virtual string Identifier { get; set; } = identifier;
}
