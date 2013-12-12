using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using NHibernate.Validator.Constraints;

namespace CIS.UI.Features.Polices.Clearances
{
    public class SummaryViewModel : ViewModelBase
    {
        public virtual BitmapSource Picture { get; set; }

        public virtual BitmapSource RightThumb { get; set; }

        public virtual string FullName { get; set; }

        public virtual string Address { get; set; }

        public virtual string BirthPlace { get; set; }

        public virtual Nullable<DateTime> BirthDate { get; set; }

        public virtual string FinalFindings { get; set; }

        public virtual string Validity { get; set; }

        [NotNullNotEmpty(Message = "Offical Receipt Number is mandatory.")]
        public virtual string OfficialReceiptNumber { get; set; }

        [NotNullNotEmpty(Message = "Tax Certificate Number is mandatory.")]
        public virtual string TaxCertificateNumber { get; set; }

        //[IsNumeric(Message = "Fee should be numeric.")]
        public virtual Nullable<decimal> ClearanceFee { get; set; }

        [IsNumeric(Message = "Years of Residency should be numeric.")]
        public virtual Nullable<int> YearsOfResidency { get; set; }

        public virtual Nullable<DateTime> ApplicationDate { get; set; }

        public virtual Nullable<DateTime> IssuedDate { get; set; }

        public SummaryViewModel()
        {
            ApplicationDate = DateTime.Today;
            IssuedDate = DateTime.Today;
        }
    }
}
