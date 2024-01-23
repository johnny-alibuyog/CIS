using System;

namespace CIS.UI.Features.Firearms.Maintenances.Makes;

public class MakeViewModel(Guid id, string name) : ViewModelBase
{
    public virtual Guid Id { get; set; } = id;

    public virtual string Name { get; set; } = name;
}
