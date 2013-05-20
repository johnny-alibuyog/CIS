using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CIS.Core.Entities.Commons;

namespace CIS.Core.Entities.Polices
{
    public class Clearance
    {
        private Guid _id;
        private int _version;
        private Audit _audit;
        private Applicant _applicant;
        private Barcode _barcode;
        private Officer _verifiedBy;
        private Officer _issuedBy;
        private Address _issuedAt;
        private string _office;
        private string _station;
        private string _location;
        private string _officialReceiptNumber;
        private string _communityTaxCertificateNumber;
        private string _partialMatchFindings;
        private string _perfectMatchFindings;

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

        public virtual Applicant Applicant
        {
            get { return _applicant; }
            set { _applicant = value; }
        }

        public virtual Barcode Barcode
        {
            get { return _barcode; }
            set { _barcode = value; }
        }

        public virtual Officer VerifiedBy
        {
            get { return _verifiedBy; }
            set { _verifiedBy = value; }
        }

        public virtual Officer IssuedBy
        {
            get { return _issuedBy; }
            set { _issuedBy = value; }
        }

        public virtual Address IssuedAt
        {
            get { return _issuedAt; }
            set { _issuedAt = value; }
        }

        public virtual string Office
        {
            get { return _office; }
            set { _office = value; }
        }

        public virtual string Station
        {
            get { return _station; }
            set { _station = value; }
        }

        public virtual string Location
        {
            get { return _location; }
            set { _location = value; }
        }

        public virtual string OfficialReceiptNumber
        {
            get { return _officialReceiptNumber; }
            set { _officialReceiptNumber = value; }
        }

        public virtual string CommunityTaxCertificateNumber
        {
            get { return _communityTaxCertificateNumber; }
            set { _communityTaxCertificateNumber = value; }
        }

        public virtual string PartialMatchFindings
        {
            get { return _partialMatchFindings; }
            set { _partialMatchFindings = value; }
        }

        public virtual string PerfectMatchFindings
        {
            get { return _perfectMatchFindings; }
            set { _perfectMatchFindings = value; }
        }

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
