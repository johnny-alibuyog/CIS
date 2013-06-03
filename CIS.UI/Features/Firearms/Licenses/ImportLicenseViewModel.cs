using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIS.UI.Features.Firearms.Licenses
{
    public class ImportLicenseViewModel
    {
        private string _licenseNumber;
        private string _controlNumber;
        private DateTime _issueDate;
        private DateTime _expiryDate;
        private string _model;
        private string _caliber;
        private string _serialNumber;
        private string _kind;
        private string _make;
        private string _firstName;
        private string _middleName;
        private string _lastName;
        private string _suffix;
        private string _gender;
        private DateTime _birthDate;
        private string _address1;
        private string _address2;
        private string _barangay;
        private string _city;
        private string _province;

        public virtual string LicenseNumber 
        {
            get { return _licenseNumber; }
            set { _licenseNumber = (value ?? string.Empty).Trim(); }
        }

        public virtual string ControlNumber
        {
            get { return _controlNumber; }
            set { _controlNumber = (value ?? string.Empty).Trim(); }
        }

        public virtual DateTime IssueDate
        {
            get { return _issueDate; }
            set { _issueDate = value; }
        }

        public virtual DateTime ExpiryDate
        {
            get { return _expiryDate; }
            set { _expiryDate = value; }
        }

        public virtual string Model
        {
            get { return _model; }
            set { _model = (value ?? string.Empty).Trim(); }
        }

        public virtual string Caliber
        {
            get { return _caliber; }
            set { _caliber = (value ?? string.Empty).Trim(); }
        }

        public virtual string SerialNumber
        {
            get { return _serialNumber; }
            set { _serialNumber = (value ?? string.Empty).Trim(); }
        }

        public virtual string Kind
        {
            get { return _kind; }
            set { _kind = (value ?? string.Empty).Trim(); }
        }

        public virtual string Make
        {
            get { return _make; }
            set { _make = (value ?? string.Empty).Trim(); }
        }

        public virtual string FirstName
        {
            get { return _firstName; }
            set { _firstName = (value ?? string.Empty).Trim(); }
        }

        public virtual string MiddleName
        {
            get { return _middleName; }
            set { _middleName = (value ?? string.Empty).Trim(); }
        }

        public virtual string LastName
        {
            get { return _lastName; }
            set { _lastName = (value ?? string.Empty).Trim(); }
        }

        public virtual string Suffix
        {
            get { return _suffix; }
            set { _suffix = (value ?? string.Empty).Trim(); }
        }

        public virtual string Gender
        {
            get { return _gender; }
            set { _gender = (value ?? string.Empty).Trim(); }
        }

        public virtual DateTime BirthDate
        {
            get { return _birthDate; }
            set { _birthDate = value; }
        }

        public virtual string Address1
        {
            get { return _address1; }
            set { _address1 = (value ?? string.Empty).Trim(); }
        }

        public virtual string Address2
        {
            get { return _address2; }
            set { _address2 = (value ?? string.Empty).Trim(); }
        }

        public virtual string Barangay
        {
            get { return _barangay; }
            set { _barangay = (value ?? string.Empty).Trim(); }
        }

        public virtual string City
        {
            get { return _city; }
            set { _city = (value ?? string.Empty).Trim(); }
        }

        public virtual string Province
        {
            get { return _province; }
            set { _province = (value ?? string.Empty).Trim(); }
        }
    }
}
