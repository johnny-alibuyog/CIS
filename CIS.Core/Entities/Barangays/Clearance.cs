using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using CIS.Core.Entities.Commons;
using CIS.Core.Utilities.Extentions;

namespace CIS.Core.Entities.Barangays
{
    public class Clearance
    {
        private Guid _id;
        private int _version;
        private Audit _audit;
        private Citizen _applicant;
        private ImageBlob _applicantPicture;
        private ImageBlob _applicantSignature;
        private string _applicantAddress;
        private Office _office;
        private ICollection<Official> _officials;
        private Nullable<DateTime> _applicationDate;
        private Nullable<DateTime> _issueDate;
        private Nullable<decimal> _fee;
        private string _controlNumber;
        private string _officialReceiptNumber;
        private string _taxCertificateNumber;
        private string _finalFindings;
        private Finding _finding;
        private Purpose _purpose;
        
        public virtual Guid Id
        {
            get { return _id; }
            protected set { _id = value; }
        }

        public virtual int Version
        {
            get { return _version; }
            protected set { _version = value; }
        }

        public virtual Audit Audit
        {
            get { return _audit; }
            set { _audit = value; }
        }

        public virtual Citizen Applicant
        {
            get { return _applicant; }
            set { _applicant = value; }
        }

        public virtual ImageBlob ApplicantPicture
        {
            get { return _applicantPicture; }
            set { _applicantPicture = value; }
        }

        public virtual ImageBlob ApplicantSignature
        {
            get { return _applicantSignature; }
            set { _applicantSignature = value; }
        }

        public virtual string ApplicantAddress
        {
            get { return _applicantAddress; }
            set { _applicantAddress = value.ToProperCase(); }
        }

        public virtual Office Office
        {
            get { return _office; }
            set { _office = value; }
        }

        public virtual IEnumerable<Official> Officials
        {
            get { return _officials; }
            set { SyncOfficials(value); }
        }

        public virtual Nullable<DateTime> ApplicationDate
        {
            get { return _applicationDate; }
            set { _applicationDate = value; }
        }

        public virtual Nullable<DateTime> IssueDate
        {
            get { return _issueDate; }
            set { _issueDate = value; }
        }

        public virtual Nullable<decimal> Fee
        {
            get { return _fee; }
            set { _fee = value; }
        }

        public virtual string ControlNumber
        {
            get { return _controlNumber; }
            set { _controlNumber = value; }
        }

        public virtual string OfficialReceiptNumber
        {
            get { return _officialReceiptNumber; }
            set { _officialReceiptNumber = value; }
        }

        public virtual string TaxCertificateNumber
        {
            get { return _taxCertificateNumber; }
            set { _taxCertificateNumber = value; }
        }

        public virtual string FinalFindings
        {
            get { return _finalFindings; }
            set { _finalFindings = value; }
        }

        public virtual Finding Finding
        {
            get { return _finding; }
            set { _finding = value; }
        }

        public virtual Purpose Purpose
        {
            get { return _purpose; }
            set { _purpose = value; }
        }

        #region Methods

        private void SyncOfficials(IEnumerable<Official> items)
        {
            var itemsToInsert = items.Except(_officials).ToList();
            var itemsToUpdate = _officials.Where(x => items.Contains(x)).ToList();
            var itemsToRemove = _officials.Except(items).ToList();

            // insert
            foreach (var item in itemsToInsert)
            {
                _officials.Add(item);
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
                _officials.Remove(item);
            }
        }

        #endregion

        #region Constructors

        public Clearance()
        {
            _officials = new Collection<Official>();
        }

        #endregion

        #region Equality Comparer

        private Nullable<int> _hashCode;

        public override bool Equals(object obj)
        {
            var that = obj as Clearance;

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

        public static bool operator ==(Clearance x, Clearance y)
        {
            return Equals(x, y);
        }

        public static bool operator !=(Clearance x, Clearance y)
        {
            return !Equals(x, y);
        }

        #endregion
    }
}
