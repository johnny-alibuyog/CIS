using System;

namespace CIS.Core.Entities.Commons
{
    public class FingerPrint : Entity<Guid>
    {
        private ImageBlob _rightThumb = new();
        private ImageBlob _rightIndex = new();
        private ImageBlob _rightMiddle = new();
        private ImageBlob _rightRing = new();
        private ImageBlob _rightPinky = new();
        private ImageBlob _leftThumb = new();
        private ImageBlob _leftIndex = new();
        private ImageBlob _leftMiddle = new();
        private ImageBlob _leftRing = new();
        private ImageBlob _leftPinky = new();

        public virtual ImageBlob RightThumb
        {
            get => _rightThumb;
            set => _rightThumb = value;
        }

        public virtual ImageBlob RightIndex
        {
            get => _rightIndex;
            set => _rightIndex = value;
        }

        public virtual ImageBlob RightMiddle
        {
            get => _rightMiddle;
            set => _rightMiddle = value;
        }

        public virtual ImageBlob RightRing
        {
            get => _rightRing;
            set => _rightRing = value;
        }

        public virtual ImageBlob RightPinky
        {
            get => _rightPinky;
            set => _rightPinky = value;
        }

        public virtual ImageBlob LeftThumb
        {
            get => _leftThumb;
            set => _leftThumb = value;
        }

        public virtual ImageBlob LeftIndex
        {
            get => _leftIndex;
            set => _leftIndex = value;
        }

        public virtual ImageBlob LeftMiddle
        {
            get => _leftMiddle;
            set => _leftMiddle = value;
        }

        public virtual ImageBlob LeftRing
        {
            get => _leftRing;
            set => _leftRing = value;
        }

        public virtual ImageBlob LeftPinky
        {
            get => _leftPinky;
            set => _leftPinky = value;
        }

        public virtual void SerializeWith(FingerPrint value)
        {
            this.RightThumb.Image = value.RightThumb.Image;
            this.RightIndex.Image = value.RightIndex.Image;
            this.RightMiddle.Image = value.RightMiddle.Image;
            this.RightRing.Image = value.RightRing.Image;
            this.RightPinky.Image = value.RightPinky.Image;
            this.LeftThumb.Image = value.LeftThumb.Image;
            this.LeftIndex.Image = value.LeftIndex.Image;
            this.LeftMiddle.Image = value.LeftMiddle.Image;
            this.LeftRing.Image = value.LeftRing.Image;
            this.LeftPinky.Image = value.LeftPinky.Image;
        }
    }
}
