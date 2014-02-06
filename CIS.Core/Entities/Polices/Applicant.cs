﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CIS.Core.Entities.Commons;
using CIS.Core.Utilities.Extentions;

namespace CIS.Core.Entities.Polices
{
    public class Applicant
    {
        private Guid _id;
        private Person _person;
        private PersonBasic _father;
        private PersonBasic _mother;
        private ICollection<PersonBasic> _relatives;
        private ICollection<Clearance> _clearances;
        private Address _address;
        private Address _provincialAddress;
        private ICollection<ImageBlob> _pictures;
        private ICollection<ImageBlob> _signatures;
        private FingerPrint _fingerPrint;
        private string _height;
        private string _weight;
        private string _build;
        private string _marks;
        private string _alsoKnownAs;
        private string _birthPlace;
        private string _occupation;
        private string _religion;
        private string _citizenship;
        private string _emailAddress;
        private string _telephoneNumber;
        private string _cellphoneNumber;
        private string _passportNumber;
        private string _taxIdentificationNumber;
        private string _socialSecuritySystemNumber;
        private Nullable<CivilStatus> _civilStatus;

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

        public virtual PersonBasic Father
        {
            get { return _father; }
            set { _father = value; }
        }

        public virtual PersonBasic Mother
        {
            get { return _mother; }
            set { _mother = value; }
        }

        public virtual IEnumerable<Clearance> Clearances
        {
            get { return _clearances; }
        }

        public virtual IEnumerable<PersonBasic> Relatives
        {
            get { return _relatives; }
            set { SyncRelatives(value); }
        }

        public virtual Address Address
        {
            get { return _address; }
            set { _address = value; }
        }

        public virtual Address ProvincialAddress
        {
            get { return _provincialAddress; }
            set { _provincialAddress = value; }
        }

        public virtual IEnumerable<ImageBlob> Pictures
        {
            get { return _pictures; }
        }

        public virtual IEnumerable<ImageBlob> Signatures
        {
            get { return _signatures; }
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

        public virtual string Build
        {
            get { return _build; }
            set { _build = value; }
        }

        public virtual string Marks
        {
            get { return _marks; }
            set { _marks = value; }
        }

        public virtual string AlsoKnownAs
        {
            get { return _alsoKnownAs; }
            set { _alsoKnownAs = value.ToProperCase(); }
        }

        public virtual string BirthPlace
        {
            get { return _birthPlace; }
            set { _birthPlace = value.ToProperCase(); }
        }

        public virtual string Occupation
        {
            get { return _occupation; }
            set { _occupation = value.ToProperCase(); }
        }

        public virtual string Religion
        {
            get { return _religion; }
            set { _religion = value.ToProperCase(); }
        }

        public virtual string Citizenship
        {
            get { return _citizenship; }
            set { _citizenship = value.ToProperCase(); }
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

        public virtual string PassportNumber
        {
            get { return _passportNumber; }
            set { _passportNumber = value; }
        }

        public virtual string TaxIdentificationNumber
        {
            get { return _taxIdentificationNumber; }
            set { _taxIdentificationNumber = value; }
        }

        public virtual string SocialSecuritySystemNumber
        {
            get { return _socialSecuritySystemNumber; }
            set { _socialSecuritySystemNumber = value; }
        }

        public virtual Nullable<CivilStatus> CivilStatus
        {
            get { return _civilStatus; }
            set { _civilStatus = value; }
        }

        #region Constructors

        public Applicant()
        {
            //_picture = new ImageBlob();
            //_signature = new ImageBlob();
            _fingerPrint = new FingerPrint();
            _pictures = new Collection<ImageBlob>();
            _signatures = new Collection<ImageBlob>();
            _relatives = new Collection<PersonBasic>();
            _clearances = new Collection<Clearance>();
        }

        #endregion

        #region Methods

        public virtual void AddClearance(Clearance clearance)
        {
            clearance.Applicant = this;
            _clearances.Add(clearance);
        }

        public virtual void AddPicture(ImageBlob picture)
        {
            _pictures.Add(picture);
        }

        public virtual void AddSignature(ImageBlob signature)
        {
            _signatures.Add(signature);
        }

        private void SyncRelatives(IEnumerable<PersonBasic> items)
        {
            var itemsToInsert = items.Except(_relatives).ToList();
            var itemsToUpdate = _relatives.Where(x => items.Contains(x)).ToList();
            var itemsToRemove = _relatives.Except(items).ToList();

            // insert
            foreach (var item in itemsToInsert)
            {
                _relatives.Add(item);
            }

            // update
            foreach (var item in itemsToUpdate)
            {
                var value = items.Single(x => x == item);
                item.SerializeWith(value);
            }

            // delete
            foreach (var item in itemsToRemove)
            {
                _relatives.Remove(item);
            }
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
