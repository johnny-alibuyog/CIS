using CIS.Core.Entities.Commons;
using System;

namespace CIS.UI.Features.Barangays.Blotters.MasterList;

public class BlotterCitizenViewModel : ViewModelBase
{
    public virtual Guid Id { get; set; }

    public virtual string Name { get; set; }

    public virtual Gender? Gender { get; set; }
}
