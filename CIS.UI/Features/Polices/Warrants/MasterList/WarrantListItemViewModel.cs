using System;

namespace CIS.UI.Features.Polices.Warrants.MasterList
{
    public class WarrantListItemViewModel : ViewModelBase
    {
        public virtual Guid Id { get; set; }
        public virtual Guid SuspectId { get; set; }
        public virtual string Suspect { get; set; }
        public virtual string Crime { get; set; }
        public virtual DateTime? IssuedOn { get; set; }
    }
}
