using NHibernate.Validator.Constraints;
using System.Windows.Media.Imaging;

namespace CIS.UI.Features.Commons.Signatures;

public class SignatureViewModel : ViewModelBase
{
    [NotNull]
    public virtual BitmapSource SignatureImage { get; set; }
}
