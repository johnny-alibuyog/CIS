using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CIS.Core.Entities.Commons;

namespace CIS.Core.Entities.Polices
{
    public class Applicant
    {
        private Guid _id;
        private Person _person;
        private Address _address;
        private ImageBlob _picture;
        private ImageBlob _signature;
        private FingerPrint _fingerPrint;
        private string _height;
        private string _weight;
        private string _alsoKnownAs;
        private string _birthPlace;
        private string _occupation;
        private string _religion;
        private string _citizenship;
        private CivilStatus _civilStatus;
        private Purpose _purpose;

        public virtual Guid Id
        {
            get { return _id; }
            protected set { _id = value; }
        }

        public virtual Person Person
        {
            get { return _person; }
            set { _person = value; }
        }

        public virtual Address Address
        {
            get { return _address; }
            set { _address = value; }
        }

        public virtual ImageBlob Picture
        {
            get { return _picture; }
            set { _picture = value; }
        }

        public virtual ImageBlob Signature
        {
            get { return _signature; }
            set { _signature = value; }
        }

        public virtual FingerPrint FingerPrint
        {
            get { return _fingerPrint; }
            set { _fingerPrint = value; }
        }

        public virtual string Height
        {
            get { return _height; }
            set { _height = value; }
        }

        public virtual string Weight
        {
            get { return _weight; }
            set { _weight = value; }
        }

        public virtual string AlsoKnownAs
        {
            get { return _alsoKnownAs; }
            set { _alsoKnownAs = value; }
        }

        public virtual string BirthPlace
        {
            get { return _birthPlace; }
            set { _birthPlace = value; }
        }

        public virtual string Occupation
        {
            get { return _occupation; }
            set { _occupation = value; }
        }

        public virtual string Religion
        {
            get { return _religion; }
            set { _religion = value; }
        }

        public virtual string Citizenship
        {
            get { return _citizenship; }
            set { _citizenship = value; }
        }

        public virtual CivilStatus CivilStatus
        {
            get { return _civilStatus; }
            set { _civilStatus = value; }
        }

        public virtual Purpose Purpose
        {
            get { return _purpose; }
            set { _purpose = value; }
        }

        #region Constructors

        public Applicant()
        {
            Picture = new ImageBlob();
            Signature = new ImageBlob();
            FingerPrint = new FingerPrint();
        }

        #endregion

        #region Equality Comparer

        private Nullable<int> _hashCode;

        public override bool Equals(object obj)
        {
            var that = obj as Applicant;

            if (that == null)
                return false;

            if (that.Id == Guid.Empty && this.Id == Guid.Empty)
                return object.ReferenceEquals(that, this);

            return (that.Id == this.Id);
        }

        public override int GetHashCode()
        {
            if (_hashCode == null)
            {
                _hashCode = (this.Id != Guid.Empty)
                    ? this.Id.GetHashCode()
                    : base.GetHashCode();
            }

            return _hashCode.Value;
        }

        public static bool operator ==(Applicant x, Applicant y)
        {
            return Equals(x, y);
        }

        public static bool operator !=(Applicant x, Applicant y)
        {
            return !Equals(x, y);
        }

        #endregion
    }
}
