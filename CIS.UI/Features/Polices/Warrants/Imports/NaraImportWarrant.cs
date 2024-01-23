using System;
using System.Collections.Generic;
using System.Data.SqlTypes;

namespace CIS.UI.Features.Polices.Warrants.Imports
{
    public class NaraImportWarrant
    {
        private string _warrantCode;
        private string _caseNumber;
        private string _crime;
        private string _remarks;
        private decimal _bailAmount;
        private DateTime? _issuedOn;
        private string _issuedBy;
        private string _issuedAtAddress1;
        private string _issuedAtAddress2;
        private string _issuedAtBarangay;
        private string _issuedAtCity;
        private string _issuedAtProvince;
        private IEnumerable<NaraImportSuspect> _suspects;

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

        public virtual string Crime
        {
            get { return _crime; }
            set { _crime = (value ?? string.Empty).Trim(); }
        }

        public virtual string Remarks
        {
            get { return _remarks; }
            set { _remarks = (value ?? string.Empty).Trim(); }
        }

        public virtual decimal BailAmount
        {
            get { return _bailAmount; }
            set { _bailAmount = value; }
        }

        public virtual DateTime? IssuedOn
        {
            get { return _issuedOn; }
            set { _issuedOn = value <= SqlDateTime.MinValue.Value ? null : value; }
        }

        public virtual string IssuedBy
        {
            get { return _issuedBy; }
            set { _issuedBy = (value ?? string.Empty).Trim(); }
        }

        public virtual string IssuedAtAddress1
        {
            get { return _issuedAtAddress1; }
            set { _issuedAtAddress1 = (value ?? string.Empty).Trim(); }
        }

        public virtual string IssuedAtAddress2
        {
            get { return _issuedAtAddress2; }
            set { _issuedAtAddress2 = (value ?? string.Empty).Trim(); }
        }

        public virtual string IssuedAtBarangay
        {
            get { return _issuedAtBarangay; }
            set { _issuedAtBarangay = value; }
        }

        public virtual string IssuedAtCity
        {
            get { return _issuedAtCity; }
            set { _issuedAtCity = (value ?? string.Empty).Trim(); }
        }

        public virtual string IssuedAtProvince
        {
            get { return _issuedAtProvince; }
            set { _issuedAtProvince = (value ?? string.Empty).Trim(); }
        }

        public virtual IEnumerable<NaraImportSuspect> Suspects
        {
            get { return _suspects; }
            set { _suspects = value; }
        }
    }
}
