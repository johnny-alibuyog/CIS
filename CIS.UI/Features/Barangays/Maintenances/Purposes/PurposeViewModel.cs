using System;

namespace CIS.UI.Features.Barangays.Maintenances.Purposes;

public class PurposeViewModel : ViewModelBase
{
    public virtual Guid Id { get; set; }

    public virtual string Name { get; set; }
}
