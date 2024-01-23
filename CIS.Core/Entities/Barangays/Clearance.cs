using CIS.Core.Entities.Commons;
using CIS.Core.Utilities.Extentions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace CIS.Core.Entities.Barangays;

public class Clearance : Entity<Guid>
{
    private int _version;
    private Audit _audit;
    private Citizen _applicant;
    private ImageBlob _applicantPicture;
    private ImageBlob _applicantSignature;
    private string _applicantAddress;
    private Office _office;
    private ICollection<Official> _officials;
    private DateTime? _applicationDate;
    private DateTime? _issueDate;
    private decimal? _fee;
    private string _controlNumber;
    private string _officialReceiptNumber;
    private string _taxCertificateNumber;
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

    public virtual Citizen Applicant
    {
        get => _applicant;
        set => _applicant = value;
    }

    public virtual ImageBlob ApplicantPicture
    {
        get => _applicantPicture;
        set => _applicantPicture = value;
    }

    public virtual ImageBlob ApplicantSignature
    {
        get => _applicantSignature;
        set => _applicantSignature = value;
    }

    public virtual string ApplicantAddress
    {
        get => _applicantAddress;
        set => _applicantAddress = value.ToProperCase();
    }

    public virtual Office Office
    {
        get => _office;
        set => _office = value;
    }

    public virtual IEnumerable<Official> Officials
    {
        get => _officials;
        set => SyncOfficials(value);
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

    public virtual decimal? Fee
    {
        get => _fee;
        set => _fee = value;
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
}
