using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using CIS.Core.Entities.Commons;
using CIS.Core.Entities.Polices;
using CIS.UI.Bootstraps.DependencyInjection;
using NHibernate;
using NHibernate.Linq;

namespace CIS.UI.Features.Commons.Biometrics
{
    public class FingerViewModel : ViewModelBase
    {
        public virtual string Id { get; private set; }

        public virtual string Name { get; private set; }

        public virtual string ImageUri { get; private set; }

        #region Static Members

        public static readonly FingerViewModel RightThumb = new FingerViewModel()
        {
            Id = Finger.RightThumb.Id,
            Name = Finger.RightThumb.Name,
            ImageUri = Finger.RightThumb.ImageUri
        };
        public static readonly FingerViewModel RightIndex = new FingerViewModel()
        {
            Id = Finger.RightIndex.Id,
            Name = Finger.RightIndex.Name,
            ImageUri = Finger.RightIndex.ImageUri
        };
        public static readonly FingerViewModel RightMiddle = new FingerViewModel()
        {
            Id = Finger.RightMiddle.Id,
            Name = Finger.RightMiddle.Name,
            ImageUri = Finger.RightMiddle.ImageUri
        };
        public static readonly FingerViewModel RightRing = new FingerViewModel()
        {
            Id = Finger.RightRing.Id,
            Name = Finger.RightRing.Name,
            ImageUri = Finger.RightRing.ImageUri
        };
        public static readonly FingerViewModel RightPinky = new FingerViewModel()
        {
            Id = Finger.RightPinky.Id,
            Name = Finger.RightPinky.Name,
            ImageUri = Finger.RightPinky.ImageUri
        };
        public static readonly FingerViewModel LeftThumb = new FingerViewModel()
        {
            Id = Finger.LeftThumb.Id,
            Name = Finger.LeftThumb.Name,
            ImageUri = Finger.LeftThumb.ImageUri
        };
        public static readonly FingerViewModel LeftIndex = new FingerViewModel()
        {
            Id = Finger.LeftIndex.Id,
            Name = Finger.LeftIndex.Name,
            ImageUri = Finger.LeftIndex.ImageUri
        };
        public static readonly FingerViewModel LeftMiddle = new FingerViewModel()
        {
            Id = Finger.LeftMiddle.Id,
            Name = Finger.LeftMiddle.Name,
            ImageUri = Finger.LeftMiddle.ImageUri
        };
        public static readonly FingerViewModel LeftRing = new FingerViewModel()
        {
            Id = Finger.LeftRing.Id,
            Name = Finger.LeftRing.Name,
            ImageUri = Finger.LeftRing.ImageUri
        };
        public static readonly FingerViewModel LeftPinky = new FingerViewModel()
        {
            Id = Finger.LeftPinky.Id,
            Name = Finger.LeftPinky.Name,
            ImageUri = Finger.LeftPinky.ImageUri
        };

        public static readonly FingerViewModel[] All = new FingerViewModel[]
        {
            FingerViewModel.RightThumb,
            FingerViewModel.RightIndex,
            FingerViewModel.RightMiddle,
            FingerViewModel.RightRing,
            FingerViewModel.RightPinky,
            FingerViewModel.LeftThumb,
            FingerViewModel.LeftIndex,
            FingerViewModel.LeftMiddle,
            FingerViewModel.LeftRing,
            FingerViewModel.LeftPinky
        };

        #endregion

        #region Equality Comparer

        private Nullable<int> _hashCode;

        public override bool Equals(object obj)
        {
            var that = obj as FingerViewModel;

            if (that == null)
                return false;

            if (string.IsNullOrWhiteSpace(that.Id) && string.IsNullOrWhiteSpace(this.Id))
                return object.ReferenceEquals(that, this);

            return (that.Id == this.Id);
        }

        public override int GetHashCode()
        {
            if (_hashCode == null)
            {
                _hashCode = (!string.IsNullOrWhiteSpace(this.Id))
                    ? this.Id.GetHashCode()
                    : base.GetHashCode();
            }

            return _hashCode.Value;
        }

        public static bool operator ==(FingerViewModel x, FingerViewModel y)
        {
            return Equals(x, y);
        }

        public static bool operator !=(FingerViewModel x, FingerViewModel y)
        {
            return !Equals(x, y);
        }

        #endregion
    }
}
