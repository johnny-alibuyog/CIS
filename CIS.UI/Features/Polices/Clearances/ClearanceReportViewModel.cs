using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace CIS.UI.Features.Polices.Clearances
{
    public class ClearanceReportViewModel : ViewModelBase
    {
        public virtual string Applicant { get; set; }
        public virtual string Address { get; set; }
        public virtual BitmapSource Picture { get; set; }
        public virtual BitmapSource RightThumb { get; set; }
        public virtual BitmapSource LeftThumb { get; set; }
        public virtual string Purpose { get; set; }
        public virtual string PartialMatchFindings { get; set; }
        public virtual string PerfectMatchFindings { get; set; }

        public virtual BitmapSource Barcode { get; set; }
        public virtual BitmapSource Logo { get; set; }
        public virtual string IssuedAt { get; set; }
        public virtual string Office { get; set; }
        public virtual string Station { get; set; }
        public virtual string Location { get; set; }
    }
}
