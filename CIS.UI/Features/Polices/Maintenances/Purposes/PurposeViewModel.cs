using System;

namespace CIS.UI.Features.Polices.Maintenances.Purposes;

public class PurposeViewModel : ViewModelBase
{
    public virtual Guid Id { get; set; }

    public virtual string Name { get; set; }
}
