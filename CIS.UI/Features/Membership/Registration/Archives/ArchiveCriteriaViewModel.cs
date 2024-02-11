using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIS.UI.Features.Membership.Registration
{
    public class ArchiveCriteriaViewModel : ViewModelBase
    {
        public virtual string FirstName { get; set; }

        public virtual string MiddleName { get; set; }

        public virtual string LastName { get; set; }

        public virtual bool FilterDate { get; set; }

        public virtual DateTime FromDate { get; set; }

        public virtual DateTime ToDate { get; set; }

        public ArchiveCriteriaViewModel()
        {
            this.FirstName = string.Empty;
            this.MiddleName = string.Empty;
            this.LastName = string.Empty;
            this.FilterDate = true;
            this.FromDate = DateTime.Today;
            this.ToDate = DateTime.Today;
        }
    }
}