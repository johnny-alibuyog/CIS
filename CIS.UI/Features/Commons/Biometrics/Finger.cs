using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace CIS.UI.Features.Commons.Biometrics
{
    public class Finger
    {
        public virtual string Name { get; private set; }

        public virtual int Index { get; private set; }

        public virtual string ImageUri { get; private set; }

        private Finger(string name, int index, string imageUri)
        {
            this.Name = name;
            this.Index = index;
            this.ImageUri = imageUri;
        }

        #region Static Members

        public static readonly Finger RightThumb = new Finger(
            name: "RightThumb",
            index: 0,
            imageUri: "/Assets/Images/FF-R1.jpg"
        );
        public static readonly Finger RightIndex = new Finger(
            name: "RightIndex",
            index: 1,
            imageUri: "/Assets/Images/FF-R2.jpg"
        );
        public static readonly Finger RightMiddle = new Finger(
            name: "RightMiddle",
            index: 2,
            imageUri: "/Assets/Images/FF-R3.jpg"
        );
        public static readonly Finger RightRing = new Finger(
            name: "RightRing",
            index: 3,
            imageUri: "/Assets/Images/FF-R4.jpg"
        );
        public static readonly Finger RightPinky = new Finger(
            name: "RightPinky",
            index: 4,
            imageUri: "/Assets/Images/FF-R5.jpg"
        );
        public static readonly Finger LeftThumb = new Finger(
            name: "LeftThumb",
            index: 5,
            imageUri: "/Assets/Images/FF-L1.jpg"
        );
        public static readonly Finger LeftIndex = new Finger(
            name: "LeftIndex",
            index: 6,
            imageUri: "/Assets/Images/FF-L2.jpg"
        );
        public static readonly Finger LeftMiddle = new Finger(
            name: "LeftMiddle",
            index: 7,
            imageUri: "/Assets/Images/FF-L3.jpg"
        );
        public static readonly Finger LeftRing = new Finger(
            name: "LeftRing",
            index: 8,
            imageUri: "/Assets/Images/FF-L4.jpg"
        );
        public static readonly Finger LeftPinky = new Finger(
            name: "LeftPinky",
            index: 9,
            imageUri: "/Assets/Images/FF-L5.jpg"
        );

        public static readonly Finger[] All = new Finger[]
        {
            Finger.RightThumb,
            Finger.RightIndex,
            Finger.RightMiddle,
            Finger.RightRing,
            Finger.RightPinky,
            Finger.LeftThumb,
            Finger.LeftIndex,
            Finger.LeftMiddle,
            Finger.LeftRing,
            Finger.LeftPinky
        };

        public static readonly IList<Finger> FingersToScan = GetFingersToScan();

        public static Finger GetNext(Finger current)
        {
            if (Finger.FingersToScan.LastOrDefault() == current)
                return Finger.FingersToScan.FirstOrDefault();
            else
                return Finger.FingersToScan[Finger.FingersToScan.IndexOf(current) + 1];
        }

        private static IList<Finger> GetFingersToScan()
        {
            var fingerNamesToScan = Properties.Settings.Default.FingersToScan;
            return Finger.All.Where(x => fingerNamesToScan.Contains(x.Name)).ToArray();
        }

        #endregion
    }
}
