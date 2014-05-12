using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIS.UI.Features.Barangays.Blotters.MasterList
{
    public enum ConcernedPersonType
    {
        Complainant,
        Respondent,
        Witness
    }

    public class BlotterListCriteriaViewModel : ViewModelBase
    {
        public virtual ConcernedPersonType SearchPersonBy { get; set; } 

        public virtual string FirstName { get; set; }

        public virtual string MiddleName { get; set; }

        public virtual string LastName { get; set; }

        public BlotterListCriteriaViewModel()
        {
            this.SearchPersonBy = ConcernedPersonType.Complainant;
        }
    }
}
