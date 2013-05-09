using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace CIS.UI.Features.Polices.Clearances
{
    public class SummaryViewModel : ViewModelBase
    {
        public virtual BitmapSource Picture { get; set; }

        public virtual string FullName { get; set; }

        public virtual string Address { get; set; }

        public virtual string BirthPlace { get; set; }

        public virtual Nullable<DateTime> BirthDate { get; set; }

        public virtual string Findings { get; set; }

        public virtual string Validity { get; set; }

        public virtual string OfficialReceiptNumber { get; set; }

        public virtual string TaxCertificateNumber { get; set; }

        public virtual DateTime IssuedOn { get; set; }

        public SummaryViewModel()
        {
            IssuedOn = DateTime.Today;
        }
    }
}
