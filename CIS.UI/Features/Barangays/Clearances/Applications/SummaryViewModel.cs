using NHibernate.Validator.Constraints;
using System;
using System.Windows.Media.Imaging;

namespace CIS.UI.Features.Barangays.Clearances.Applications;

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

    public virtual decimal? ClearanceFee { get; set; }

    public virtual DateTime? ApplicationDate { get; set; }

    public virtual DateTime? IssuedDate { get; set; }

    public SummaryViewModel()
    {
        this.ApplicationDate = DateTime.Today;
        this.IssuedDate = DateTime.Today;
    }
}
