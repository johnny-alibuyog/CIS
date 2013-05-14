using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIS.Core.Entities.Commons
{
    public class Finger
    {
        private string _id;
        private string _name;
        private string _imageUri;

        public virtual string Id
        {
            get { return _id; }
            protected set { _id = value; }
        }

        public virtual string Name 
        {
            get { return _name; }
            protected set { _name = value; }
        }

        public virtual string ImageUri 
        {
            get { return _imageUri; }
            protected set { _imageUri = value; } 
        }

        #region Constructors

        public Finger() { }

        private Finger(string id, string name, string imageUri)
        {
            this.Id = id;
            this.Name = name;
            this.ImageUri = imageUri;
        }

        #endregion

        #region Equality Comparer

        private Nullable<int> _hashCode;

        public override bool Equals(object obj)
        {
            var that = obj as Finger;

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

        public static bool operator ==(Finger x, Finger y)
        {
            return Equals(x, y);
        }

        public static bool operator !=(Finger x, Finger y)
        {
            return !Equals(x, y);
        }

        #endregion

        #region Static Members

        public static readonly Finger RightThumb = new Finger(
            id: "RT",
            name: "Right Thumb",
            imageUri: "/Assets/Images/FF-R1.jpg"
        );
        public static readonly Finger RightIndex = new Finger(
            id: "RI",
            name: "Right Index",
            imageUri: "/Assets/Images/FF-R2.jpg"
        );
        public static readonly Finger RightMiddle = new Finger(
            id: "RM",
            name: "Right Middle",
            imageUri: "/Assets/Images/FF-R3.jpg"
        );
        public static readonly Finger RightRing = new Finger(
            id: "RR", 
            name: "Right Ring",
            imageUri: "/Assets/Images/FF-R4.jpg"
        );
        public static readonly Finger RightPinky = new Finger(
            id: "RP",
            name: "Right Pinky",
            imageUri: "/Assets/Images/FF-R5.jpg"
        );
        public static readonly Finger LeftThumb = new Finger(
            id: "LT",
            name: "Left Thumb",
            imageUri: "/Assets/Images/FF-L1.jpg"
        );
        public static readonly Finger LeftIndex = new Finger(
            id: "LI",
            name: "Left Index",
            imageUri: "/Assets/Images/FF-L2.jpg"
        );
        public static readonly Finger LeftMiddle = new Finger(
            id: "LM",
            name: "Left Middle",
            imageUri: "/Assets/Images/FF-L3.jpg"
        );
        public static readonly Finger LeftRing = new Finger(
            id: "LR",
            name: "Left Ring",
            imageUri: "/Assets/Images/FF-L4.jpg"
        );
        public static readonly Finger LeftPinky = new Finger(
            id: "LP",
            name: "Left Pinky",
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

        #endregion
    }
}
