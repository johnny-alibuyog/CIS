using System;

namespace CIS.UI.Features.Membership.Maintenance.Purpose;

public class PurposeViewModel : ViewModelBase
{
    public virtual Guid Id { get; set; }

    public virtual string Name { get; set; }
}
