namespace CIS.UI.Features.Barangays.Maintenances;

public class MaintenanceMessage(string identifier)
{
    public virtual string Identifier { get; set; } = identifier;
}
