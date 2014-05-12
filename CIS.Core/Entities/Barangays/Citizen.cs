using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CIS.Core.Entities.Commons;

namespace CIS.Core.Entities.Barangays
{
    public class Citizen
    {
        private Guid _id;
        private int _version;
        private Audit _audit;
        private Person _person;
        private Nullable<CivilStatus> _civilStatus;
        private string _alsoKnownAs;
        private string _birthPlace;
        private string _occupation;
        private string _religion;
        private string _citizenship;
        private string _emailAddress;
        private string _telephoneNumber;
        private string _cellphoneNumber;
        private Address _currentAddress;
        private Address _provincialAddress;
        private FingerPrint _fingerPrint;
        private ICollection<ImageBlob> _pictures;
        private ICollection<ImageBlob> _signatures;

        public virtual Guid Id
        {
            get { return _id; }
            set { _id = value; }
        }

        public virtual int Version
        {
            get { return _version; }
            set { _version = value; }
        }

        public virtual Audit Audit
        {
            get { return _audit; }
            set { _audit = value; }
        }

        public virtual Person Person
        {
            get { return _person; }
            set { _person = value; }
        }

        public virtual Nullable<CivilStatus> CivilStatus
        {
            get { return _civilStatus; }
            set { _civilStatus = value; }
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

        public virtual string EmailAddress
        {
            get { return _emailAddress; }
            set { _emailAddress = value; }
        }

        public virtual string TelephoneNumber
        {
            get { return _telephoneNumber; }
            set { _telephoneNumber = value; }
        }

        public virtual string CellphoneNumber
        {
            get { return _cellphoneNumber; }
            set { _cellphoneNumber = value; }
        }

        public virtual Address CurrentAddress
        {
            get { return _currentAddress; }
            set { _currentAddress = value; }
        }

        public virtual Address ProvincialAddress
        {
            get { return _provincialAddress; }
            set { _provincialAddress = value; }
        }

        public virtual FingerPrint FingerPrint
        {
            get { return _fingerPrint; }
            set { _fingerPrint = value; }
        }

        public virtual IEnumerable<ImageBlob> Pictures
        {
            get { return _pictures; }

        }

        public virtual IEnumerable<ImageBlob> Signatures
        {
            get { return _signatures; }
        }

        #region Methods

        public virtual void SerializeWith(Citizen value)
        {
            this.Person = value.Person;
            this.CivilStatus = value.CivilStatus;
            this.AlsoKnownAs = value.AlsoKnownAs;
            this.BirthPlace = value.BirthPlace;
            this.Occupation = value.Occupation;
            this.Religion = value.Religion;
            this.Citizenship = value.Citizenship;
            this.EmailAddress = value.EmailAddress;
            this.TelephoneNumber = value.TelephoneNumber;
            this.CellphoneNumber = value.CellphoneNumber;
            this.CurrentAddress = value.CurrentAddress;
            this.ProvincialAddress = value.ProvincialAddress;
            //this.FingerPrint.SerializeWith(value.FingerPrint);
            //this.Pictures = value.Pictures;
            //this.Signatures = value.Signatures;
        }

        public virtual void AddPicture(ImageBlob picture)
        {
            _pictures.Add(picture);
        }

        public virtual void AddSignature(ImageBlob signature)
        {
            _signatures.Add(signature);
        }

        #endregion

        #region Constructors

        public Citizen()
        {
            _fingerPrint = new FingerPrint();
            _pictures = new Collection<ImageBlob>();
            _signatures = new Collection<ImageBlob>();
        }

        #endregion

        #region Equality Comparer

        private Nullable<int> _hashCode;

        public override bool Equals(object obj)
        {
            var that = obj as Citizen;

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

        public static bool operator ==(Citizen x, Citizen y)
        {
            return Equals(x, y);
        }

        public static bool operator !=(Citizen x, Citizen y)
        {
            return !Equals(x, y);
        }

        #endregion
    }
}
