using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Ink;
using System.Windows.Media.Imaging;
using NHibernate.Validator.Constraints;
using ReactiveUI.Xaml;

namespace CIS.UI.Features.Commons.Signatures
{
    public class SignatureViewModel : ViewModelBase
    {
        [NotNull]
        public virtual BitmapSource SignatureImage { get; set; }
    }
}
