﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using CIS.Core.Entities.Commons;
using CIS.Core.Entities.Firearms;

namespace CIS.Core.Entities.Polices
{
    public class Clearance
    {
        private Guid _id;
        private int _version;
        private Audit _audit;
        private Applicant _applicant;
        private Barcode _barcode;
        private Officer _verifier;
        private string _verifierRank;
        private string _verifierPosition;
        private Officer _certifier;
        private string _certifierRank;
        private string _certifierPosition;
        private Station _station;
        private DateTime _issueDate;
        private string _validity;
        private string _officialReceiptNumber;
        private string _communityTaxCertificateNumber;
        private string _partialMatchFindings;
        private string _perfectMatchFindings;
        private string _finalFindings;
        private ICollection<Suspect> _suspectPartialMatches;
        private ICollection<Suspect> _suspectPerfectMatches;
        private ICollection<License> _expiredLicenseMatches;


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

        public virtual Officer Verifier
        {
            get { return _verifier; }
            protected set { _verifier = value; }
        }

        public virtual string VerifierRank
        {
            get { return _verifierRank; }
            protected set { _verifierRank = value; }
        }

        public virtual string VerifierPosition
        {
            get { return _verifierPosition; }
            protected set { _verifierPosition = value; }
        }

        public virtual Officer Certifier
        {
            get { return _certifier; }
            protected set { _certifier = value; }
        }

        public virtual string CertifierRank
        {
            get { return _certifierRank; }
            protected set { _certifierRank = value; }
        }

        public virtual string CertifierPosition
        {
            get { return _certifierPosition; }
            protected set { _certifierPosition = value; }
        }

        public virtual Station Station
        {
            get { return _station; }
            protected set { _station = value; }
        }

        public virtual DateTime IssueDate
        {
            get { return _issueDate; }
            set { _issueDate = value; }
        }

        public virtual string Validity
        {
            get { return _validity; }
            set { _validity = value; }
        }

        public virtual string OfficialReceiptNumber
        {
            get { return _officialReceiptNumber; }
            set { _officialReceiptNumber = value; }
        }

        public virtual string TaxCertificateNumber
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

        public virtual string FinalFindings
        {
            get { return _finalFindings; }
            set { _finalFindings = value; }
        }

        public virtual IEnumerable<Suspect> SuspectPartialMatches
        {
            get { return _suspectPartialMatches; }
            set { SyncSuspectPartialMatches(value); }
        }

        public virtual IEnumerable<Suspect> SuspectPerfectMatches
        {
            get { return _suspectPerfectMatches; }
            set { SyncSuspectPerfectMatches(value); }
        }

        public virtual IEnumerable<License> ExpiredLicenseMatches
        {
            get { return _expiredLicenseMatches; }
            set { SyncExpiredLicenseMatches(value); }
        }

        #region Methods

        public virtual void SetVerifier(Officer officer)
        {
            this.Verifier = officer;
            this.VerifierRank = officer.Rank.Name;
            this.VerifierPosition = officer.Position;
        }

        public virtual void SetCertifier(Officer officer)
        {
            this.Certifier = officer;
            this.CertifierRank = officer.Rank.Name;
            this.CertifierPosition = officer.Position;
        }

        public virtual void SetStation(Station station)
        {
            this.Station = station;
            this.IssueDate = DateTime.Today;
            this.Validity = station.GetValidity(this.IssueDate);
        }

        private void SyncSuspectPartialMatches(IEnumerable<Suspect> items)
        {
            var itemsToInsert = items.Except(_suspectPartialMatches).ToList();
            var itemsToRemove = _suspectPartialMatches.Except(items).ToList();

            // insert
            foreach (var item in itemsToInsert)
                _suspectPartialMatches.Add(item);

            // delete
            foreach (var item in itemsToRemove)
                _suspectPartialMatches.Remove(item);
        }

        private void SyncSuspectPerfectMatches(IEnumerable<Suspect> items)
        {
            var itemsToInsert = items.Except(_suspectPerfectMatches).ToList();
            var itemsToRemove = _suspectPerfectMatches.Except(items).ToList();

            // insert
            foreach (var item in itemsToInsert)
                _suspectPerfectMatches.Add(item);

            // delete
            foreach (var item in itemsToRemove)
                _suspectPerfectMatches.Remove(item);
        }

        private void SyncExpiredLicenseMatches(IEnumerable<License> items)
        {
            var itemsToInsert = items.Except(_expiredLicenseMatches).ToList();
            var itemsToRemove = _expiredLicenseMatches.Except(items).ToList();

            // insert
            foreach (var item in itemsToInsert)
                _expiredLicenseMatches.Add(item);

            // delete
            foreach (var item in itemsToRemove)
                _expiredLicenseMatches.Remove(item);
        }

        #endregion

        #region Constructors

        public Clearance()
        {
            _applicant = new Applicant();
            _barcode = Barcode.GenerateBarcode();
            _suspectPartialMatches = new Collection<Suspect>();
            _suspectPerfectMatches = new Collection<Suspect>();
            _expiredLicenseMatches = new Collection<License>();
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
