using System;
using System.Collections.Generic;

namespace CIS.Core.Domain.Membership;

public abstract class MembershipStatus : Entity<Guid>
{
    private string _remarks;

    public virtual string Remarks
    {
        get => _remarks;
        set => _remarks = value;
    }
}

public class ApplyingStatus : MembershipStatus
{
    private readonly ICollection<Signatory> _signatories = [];

    public virtual ICollection<Signatory> Signatories
    {
        get => _signatories;
    }

    public ApplyingStatus() { }

    public ApplyingStatus(IEnumerable<Signatory> signatories)
    {
        this._signatories = [.. signatories];
    }
}

public class ApprovedStatus : MembershipStatus
{
    private DateTime _approvedDate;
    private readonly ICollection<Signatory> _signatories = [];

    public virtual DateTime ApprovedDate
    {
        get => _approvedDate;
        set => _approvedDate = value;
    }

    public virtual ICollection<Signatory> Signatories
    {
        get => _signatories;
    }

    public ApprovedStatus() { }

    public ApprovedStatus(Signatory signatory)
    {
        this._approvedDate = DateTime.Now;
        this._signatories.Add(signatory);
    }
}

public class RegisteredStatus : MembershipStatus // technically, this is also active status
{
    private int _year;
    private DateTime _registeredDate;
    private Member _incumbentSecretary; // FIXME: this should be a officer
    private Member _incumbentPersident; // FIXME: this should be a officer

    public virtual int Year
    {
        get => _year;
        protected set => _year = value;
    }

    public virtual DateTime RegisteredDate
    {
        get => _registeredDate;
        protected set => _registeredDate = value;
    }

    public virtual Member IncumbentSecretary
    {
        get => _incumbentSecretary;
        set => _incumbentSecretary = value;
    }

    public virtual Member IncumbentPersident
    {
        get => _incumbentPersident;
        set => _incumbentPersident = value;
    }


    public RegisteredStatus() { }

    public RegisteredStatus(Member incumbentSecretary, Member incumbentPresident)
    {
        this._year = DateTime.Now.Year;
        this._registeredDate = DateTime.Now;
        this._incumbentSecretary = incumbentSecretary;
        this._incumbentPersident = incumbentPresident;
    }
}

public class RejectedStatus : MembershipStatus
{
    private DateTime _rejectedDate;
    private Signatory _rejectedBy;

    public virtual DateTime RejectedDate
    {
        get => _rejectedDate;
        protected set => _rejectedDate = value;
    }

    public virtual Signatory RejectedBy
    {
        get => _rejectedBy;
        protected set => _rejectedBy = value;
    }

    public RejectedStatus() { }

    public RejectedStatus(Signatory signatory)
    {
        this._rejectedDate = DateTime.Now;
        this._rejectedBy = signatory;
    }
}

