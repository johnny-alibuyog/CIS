using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIS.UI.Features.Barangays.Blotters.MasterList
{
    public class BlotterListItemViewModel : ViewModelBase
    {
        public virtual Guid Id { get; set; }

        public virtual string Complaint { get; set; }

        public virtual Nullable<DateTime> FiledOn { get; set; }

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
}
