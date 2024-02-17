﻿using CIS.Core.Domain.Common;
using CIS.Core.Utility.Extention;
using System;
using System.Linq;

namespace CIS.Core.Domain.Membership;

public class Registration : Entity<Guid>
{
    private int _version;
    private Audit _audit;
    private Application _application;
    private ImageBlob _applicantPicture = new();
    private ImageBlob _applicantSignature = new();
    private CivilStatus? _applicantCivilStatus;
    private string _applicantAddress;
    private string _applicantCitizenship;
    private Barcode _barcode = Barcode.GenerateBarcode();
    private Officer _verifier;
    private string _verifierRank;
    private string _verifierPosition;
    private Officer _certifier;
    private string _certifierRank;
    private string _certifierPosition;
    private Station _station;
    private DateTime? _applicationDate;
    private DateTime? _issueDate;
    private string _validity;
    private string _controlNumber;
    private string _officialReceiptNumber;
    private string _taxCertificateNumber;
    private decimal? _fee;
    private int? _yearsResident;
    private string _finalFindings;
    private Finding _finding;
    private Purpose _purpose;

    public virtual int Version
    {
        get => _version;
        protected set => _version = value;
    }

    public virtual Audit Audit
    {
        get => _audit;
        set => _audit = value;
    }

    public virtual Application Application
    {
        get => _application;
        set => _application = value;
    }

    public virtual ImageBlob ApplicantPicture
    {
        get => _applicantPicture ??= Application.Pictures.FirstOrDefault() ?? new ImageBlob();
        set => _applicantPicture = value;
    }

    public virtual ImageBlob ApplicantSignature
    {
        get => _applicantSignature ??= Application.Signatures.FirstOrDefault() ?? new ImageBlob();
        set => _applicantSignature = value;
    }

    public virtual CivilStatus? ApplicantCivilStatus
    {
        get => _applicantCivilStatus ??= Application.CivilStatus;
        set => _applicantCivilStatus = value;
    }

    public virtual string ApplicantAddress
    {
        get => _applicantAddress ??= Application.Address.ToString();
        set => _applicantAddress = value.ToProperCase();
    }

    public virtual string ApplicantCitizenship
    {
        get => _applicantCitizenship ??= Application.Citizenship;
        set => _applicantCitizenship = value.ToProperCase();
    }

    public virtual Barcode Barcode
    {
        get => _barcode;
        set => _barcode = value;
    }

    public virtual Officer Verifier
    {
        get => _verifier;
        protected set => _verifier = value;
    }

    public virtual string VerifierRank
    {
        get => _verifierRank;
        protected set => _verifierRank = value;
    }

    public virtual string VerifierPosition
    {
        get => _verifierPosition;
        protected set => _verifierPosition = value;
    }

    public virtual Officer Certifier
    {
        get => _certifier;
        protected set => _certifier = value;
    }

    public virtual string CertifierRank
    {
        get => _certifierRank;
        protected set => _certifierRank = value;
    }

    public virtual string CertifierPosition
    {
        get => _certifierPosition;
        protected set => _certifierPosition = value;
    }

    public virtual Station Station
    {
        get => _station;
        protected set => _station = value;
    }

    public virtual DateTime? ApplicationDate
    {
        get => _applicationDate;
        set => _applicationDate = value;
    }

    public virtual DateTime? IssueDate
    {
        get => _issueDate;
        set => _issueDate = value;
    }

    public virtual string Validity
    {
        get => _validity;
        set => _validity = value;
    }

    public virtual string ControlNumber
    {
        get => _controlNumber;
        set => _controlNumber = value;
    }

    public virtual string OfficialReceiptNumber
    {
        get => _officialReceiptNumber;
        set => _officialReceiptNumber = value;
    }

    public virtual string TaxCertificateNumber
    {
        get => _taxCertificateNumber;
        set => _taxCertificateNumber = value;
    }

    public virtual decimal? Fee
    {
        get => _fee;
        set => _fee = value;
    }

    public virtual int? YearsResident
    {
        get => _yearsResident;
        set => _yearsResident = value;
    }

    public virtual string FinalFindings
    {
        get => _finalFindings;
        set => _finalFindings = value;
    }

    public virtual Finding Finding
    {
        get => _finding;
        set => _finding = value;
    }

    public virtual Purpose Purpose
    {
        get => _purpose;
        set => _purpose = value;
    }

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
    }
}