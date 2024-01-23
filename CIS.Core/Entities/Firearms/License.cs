using CIS.Core.Entities.Commons;
using System;

namespace CIS.Core.Entities.Firearms;

public class License : Entity<Guid>
{
    private int _version;
    private Audit _audit;
    private Person _person;
    private Address _address;
    private Gun _gun;
    private string _licenseNumber;
    private string _controlNumber;
    private DateTime? _issueDate;
    private DateTime? _expiryDate;

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

    public virtual Person Person
    {
        get => _person;
        set => _person = value;
    }

    public virtual Address Address
    {
        get => _address;
        set => _address = value;
    }

    public virtual Gun Gun
    {
        get => _gun;
        set => _gun = value;
    }

    public virtual string LicenseNumber
    {
        get => _licenseNumber;
        set => _licenseNumber = value;
    }

    public virtual string ControlNumber
    {
        get => _controlNumber;
        set => _controlNumber = value;
    }

    public virtual DateTime? IssueDate
    {
        get => _issueDate;
        set => _issueDate = value;
    }

    public virtual DateTime? ExpiryDate
    {
        get => _expiryDate;
        set => _expiryDate = value;
    }

    #region Methods

    public virtual void SerializeWith(License value)
    {
        this.LicenseNumber = value.LicenseNumber;
        this.ControlNumber = value.ControlNumber;
        this.IssueDate = value.IssueDate;
        this.ExpiryDate = value.ExpiryDate;
        this.Gun = value.Gun;
    }

    #endregion
}
