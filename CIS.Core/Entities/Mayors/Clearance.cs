using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CIS.Core.Entities.Commons;

namespace CIS.Core.Entities.Mayors
{
    public class Clearance
    {
        private Guid _id;
        private int _version;
        private Audit _audit;
        private Person _applicant;
        private Address _applicantAddress;
        private Picture _applicantPicture;
        private string _company;
        private Address _companyAddress;
        private Person _mayor;
        private Person _secretary;
        private DateTime _issueDate;
        private string _permitNumber;
        private decimal _permitFee;
        private decimal _penalty;
        private string _officialReceiptNumber;
        private string _notice;

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

        public virtual Person Applicant
        {
            get { return _applicant; }
            set { _applicant = value; }
        }

        public virtual Address ApplicantAddress
        {
            get { return _applicantAddress; }
            set { _applicantAddress = value; }
        }

        public virtual Picture ApplicantPicture
        {
            get { return _applicantPicture; }
            set { _applicantPicture = value; }
        }

        public virtual string Company
        {
            get { return _company; }
            set { _company = value; }
        }

        public virtual Address CompanyAddress
        {
            get { return _companyAddress; }
            set { _companyAddress = value; }
        }

        public virtual Person Mayor
        {
            get { return _mayor; }
            set { _mayor = value; }
        }

        public virtual Person Secretary
        {
            get { return _secretary; }
            set { _secretary = value; }
        }

        public virtual DateTime IssueDate
        {
            get { return _issueDate; }
            set { _issueDate = value; }
        }

        public virtual string PermitNumber
        {
            get { return _permitNumber; }
            set { _permitNumber = value; }
        }

        public virtual decimal PermitFee
        {
            get { return _permitFee; }
            set { _permitFee = value; }
        }

        public virtual decimal Penalty
        {
            get { return _penalty; }
            set { _penalty = value; }
        }

        public virtual string OfficialReceiptNumber
        {
            get { return _officialReceiptNumber; }
            set { _officialReceiptNumber = value; }
        }

        public virtual string Notice
        {
            get { return _notice; }
            set { _notice = value; }
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
