using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using NHibernate.Validator.Constraints;
using ReactiveUI;
using System.Reactive.Linq;

namespace CIS.UI.Features.Polices.Clearances.Applications
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

        //[NotNullNotEmpty(Message = "Control Number is mandatory.")]
        public virtual string ControlNumber { get; set; }

        [NotNullNotEmpty(Message = "Offical Receipt Number is mandatory.")]
        public virtual string OfficialReceiptNumber { get; set; }

        [NotNullNotEmpty(Message = "Tax Certificate Number is mandatory.")]
        public virtual string TaxCertificateNumber { get; set; }

        //[IsNumeric(Message = "Fee should be numeric.")]
        public virtual Nullable<decimal> ClearanceFee { get; set; }

        public virtual Nullable<int> ClearanceValidityInDays { get; set; }

        [IsNumeric(Message = "Years of Residency should be numeric.")]
        public virtual Nullable<int> YearsOfResidency { get; set; }

        public virtual Nullable<DateTime> ApplicationDate { get; set; }

        public virtual Nullable<DateTime> IssuedDate { get; set; }

        public SummaryViewModel()
        {
            this.ApplicationDate = DateTime.Today;
            this.IssuedDate = DateTime.Today;

            this.WhenAnyValue(x => x.ClearanceValidityInDays)
                .Where(x => x != null)
                .Subscribe(x =>
                {
                    var displayFormat = "Clearance is valid until {0}.";
                    var validUntil = this.IssuedDate.Value.AddDays(x ?? 60).ToString("MMM-dd-yyyy");
                    this.Validity = string.Format(displayFormat, validUntil);
                });
        }
    }
}
