using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIS.UI.Features.Polices.Warrants
{
    public class LitusImportWarrant 
    {
        private string _warrantCode;
        private string _caseNumber;
        private string _description;
        private decimal _bailAmount;
        private DateTime _issuedOn;
        private string _issuedBy;
        private string _address1;
        private string _address2;
        private string _barangay;
        private string _city;
        private string _province;
        private IEnumerable<LitusImportSuspect> _suspects;

        public virtual string WarrantCode
        {
            get { return _warrantCode; }
            set { _warrantCode = (value ?? string.Empty).Trim(); }
        }

        public virtual string CaseNumber
        {
            get { return _caseNumber; }
            set { _caseNumber = (value ?? string.Empty).Trim(); }
        }

        public virtual string Description
        {
            get { return _description; }
            set { _description = (value ?? string.Empty).Trim(); }
        }

        public virtual decimal BailAmount
        {
            get { return _bailAmount; }
            set { _bailAmount = value; }
        }

        public virtual DateTime IssuedOn
        {
            get { return _issuedOn; }
            set { _issuedOn = value; }
        }

        public virtual string IssuedBy
        {
            get { return _issuedBy; }
            set { _issuedBy = (value ?? string.Empty).Trim(); }
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

        public virtual IEnumerable<LitusImportSuspect> Suspects
        {
            get { return _suspects; }
            set { _suspects = value; }
        }
    }
}
