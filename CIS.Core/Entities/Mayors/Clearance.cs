using CIS.Core.Entities.Commons;
using System;

namespace CIS.Core.Entities.Mayors;

public class Clearance : Entity<Guid>
{
    private int _version;
    private Audit _audit;
    private Person _applicant;
    private Address _applicantAddress;
    private ImageBlob _applicantPicture;
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

    public virtual Person Applicant
    {
        get => _applicant;
        set => _applicant = value;
    }

    public virtual Address ApplicantAddress
    {
        get => _applicantAddress;
        set => _applicantAddress = value;
    }

    public virtual ImageBlob ApplicantPicture
    {
        get => _applicantPicture;
        set => _applicantPicture = value;
    }

    public virtual string Company
    {
        get => _company;
        set => _company = value;
    }

    public virtual Address CompanyAddress
    {
        get => _companyAddress;
        set => _companyAddress = value;
    }

    public virtual Person Mayor
    {
        get => _mayor;
        set => _mayor = value;
    }

    public virtual Person Secretary
    {
        get => _secretary;
        set => _secretary = value;
    }

    public virtual DateTime IssueDate
    {
        get => _issueDate;
        set => _issueDate = value;
    }

    public virtual string PermitNumber
    {
        get => _permitNumber;
        set => _permitNumber = value;
    }

    public virtual decimal PermitFee
    {
        get => _permitFee;
        set => _permitFee = value;
    }

    public virtual decimal Penalty
    {
        get => _penalty;
        set => _penalty = value;
    }

    public virtual string OfficialReceiptNumber
    {
        get => _officialReceiptNumber;
        set => _officialReceiptNumber = value;
    }

    public virtual string Notice
    {
        get => _notice;
        set => _notice = value;
    }

}
