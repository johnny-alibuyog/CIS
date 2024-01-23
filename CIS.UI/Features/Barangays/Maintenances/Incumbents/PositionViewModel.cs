using ReactiveUI;

namespace CIS.UI.Features.Barangays.Maintenances.Incumbents;

public class PositionViewModel : Lookup<string>
{
    public virtual IReactiveList<Lookup<string>> Committees { get; set; }

    public PositionViewModel() { }

    public PositionViewModel(string id, string name) : base(id, name) { }
}
