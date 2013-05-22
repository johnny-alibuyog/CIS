using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using CIS.Core.Entities.Commons;
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

        private FingerViewModel(string id, string name, string imageUri)
        {
            this.Id = id;
            this.Name = name;
            this.ImageUri = imageUri;
        }

        #region Static Members

        public static readonly FingerViewModel RightThumb = new FingerViewModel(
            id: Finger.RightThumb.Id,
            name: Finger.RightThumb.Name,
            imageUri: Finger.RightThumb.ImageUri
        );
        public static readonly FingerViewModel RightIndex = new FingerViewModel(
            id: Finger.RightIndex.Id,
            name: Finger.RightIndex.Name,
            imageUri: Finger.RightIndex.ImageUri
        );
        public static readonly FingerViewModel RightMiddle = new FingerViewModel(
            id: Finger.RightMiddle.Id,
            name: Finger.RightMiddle.Name,
            imageUri: Finger.RightMiddle.ImageUri
        );
        public static readonly FingerViewModel RightRing = new FingerViewModel(
            id: Finger.RightRing.Id,
            name: Finger.RightRing.Name,
            imageUri: Finger.RightRing.ImageUri
        );
        public static readonly FingerViewModel RightPinky = new FingerViewModel(
            id: Finger.RightPinky.Id,
            name: Finger.RightPinky.Name,
            imageUri: Finger.RightPinky.ImageUri
        );
        public static readonly FingerViewModel LeftThumb = new FingerViewModel(
            id: Finger.LeftThumb.Id,
            name: Finger.LeftThumb.Name,
            imageUri: Finger.LeftThumb.ImageUri
        );
        public static readonly FingerViewModel LeftIndex = new FingerViewModel(
            id: Finger.LeftIndex.Id,
            name: Finger.LeftIndex.Name,
            imageUri: Finger.LeftIndex.ImageUri
        );
        public static readonly FingerViewModel LeftMiddle = new FingerViewModel(
            id: Finger.LeftMiddle.Id,
            name: Finger.LeftMiddle.Name,
            imageUri: Finger.LeftMiddle.ImageUri
        );
        public static readonly FingerViewModel LeftRing = new FingerViewModel(
            id: Finger.LeftRing.Id,
            name: Finger.LeftRing.Name,
            imageUri: Finger.LeftRing.ImageUri
        );
        public static readonly FingerViewModel LeftPinky = new FingerViewModel(
            id: Finger.LeftPinky.Id,
            name: Finger.LeftPinky.Name,
            imageUri: Finger.LeftPinky.ImageUri
        );

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

        public static readonly IList<FingerViewModel> FingersToScan = GetFingersToScan();

        public static FingerViewModel GetNext(FingerViewModel current)
        {
            if (FingerViewModel.FingersToScan.LastOrDefault() == current)
                return FingerViewModel.FingersToScan.FirstOrDefault();
            else
                return FingerViewModel.FingersToScan[FingerViewModel.FingersToScan.IndexOf(current) + 1];
        }

        private static IList<FingerViewModel> GetFingersToScan()
        {
            var fingersToScan = (IList<FingerViewModel>)null;
            var sessionFactory = IoC.Container.Resolve<ISessionFactory>();
            using (var session = sessionFactory.OpenSession())
            using (var transaction = session.BeginTransaction())
            {
                var fingerIds = session.Query<Terminal>()
                    .Where(x => x.MachineName == Environment.MachineName)
                    .SelectMany(x => x.FingersToScan)
                    .Select(x => x.Id)
                    .ToList();

                if (fingerIds == null || fingerIds.Count() == 0)
                    fingerIds = Properties.Settings.Default.FingersToScan.Cast<string>().ToList();

                fingersToScan = FingerViewModel.All.Where(x => fingerIds.Contains(x.Id)).ToList();

                transaction.Commit();
            }

            return fingersToScan;
        }

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
