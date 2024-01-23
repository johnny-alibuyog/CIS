using System;

namespace CIS.UI.Features.Barangays.Blotters.MasterList;

public class BlotterListItemViewModel : ViewModelBase
{
    public virtual Guid Id { get; set; }

    public virtual string Complaint { get; set; }

    public virtual DateTime? FiledOn { get; set; }

    public virtual string[] ConcernedPersons { get; set; }

    public virtual string ConcernedPersonsDisplay 
    {
        get
        {
            return this.ConcernedPersons != null
              ? string.Join(", ", this.ConcernedPersons)
              : string.Empty;
        }
    }

    public virtual ConcernedPersonType ConcernedPersonType { get; set; }

    public BlotterListItemViewModel()
    {
        ConcernedPersons = new string[] { };
    }
}
