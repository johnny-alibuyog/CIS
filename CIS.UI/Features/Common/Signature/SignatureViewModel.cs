using NHibernate.Validator.Constraints;
using System.Windows.Media.Imaging;

namespace CIS.UI.Features.Common.Signature;

public class SignatureViewModel : ViewModelBase
{
    [NotNull]
    public virtual BitmapSource SignatureImage { get; set; }
}
