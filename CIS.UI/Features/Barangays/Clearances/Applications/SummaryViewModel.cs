using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using NHibernate.Validator.Constraints;
using ReactiveUI;
using System.Reactive.Linq;

namespace CIS.UI.Features.Barangays.Clearances.Applications
{
    public class SummaryViewModel : ViewModelBase
    {
        public virtual BitmapSource Picture { get; set; }

        public virtual BitmapSource RightThumb { get; set; }

        public virtual string FullName { get; set; }

        public virtual string Address { get; set; }

        public virtual string FinalFindings { get; set; }

        public virtual string ControlNumber { get; set; }

        [NotNullNotEmpty(Message = "Offical Receipt Number is mandatory.")]
        public virtual string OfficialReceiptNumber { get; set; }

        [NotNullNotEmpty(Message = "Tax Certificate Number is mandatory.")]
        public virtual string TaxCertificateNumber { get; set; }

        public virtual Nullable<decimal> ClearanceFee { get; set; }

        public virtual Nullable<DateTime> ApplicationDate { get; set; }

        public virtual Nullable<DateTime> IssuedDate { get; set; }

        public SummaryViewModel()
        {
            this.ApplicationDate = DateTime.Today;
            this.IssuedDate = DateTime.Today;
        }
    }
}
