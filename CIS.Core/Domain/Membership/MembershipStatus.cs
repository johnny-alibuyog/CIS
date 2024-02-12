using System;
using System.Collections.Generic;

namespace CIS.Core.Domain.Membership;

public abstract class MembershipStatus : Entity<Guid> 
{
    private MemberMembershipInfo _membershipInfo;

    public virtual MemberMembershipInfo MembershipInfo
    {
        get => _membershipInfo;
        protected set => _membershipInfo = value;
    }

    public MembershipStatus() { }

    public MembershipStatus(MemberMembershipInfo member)
    {
        _membershipInfo = member;
    }
}

public class MemberApplied : MembershipStatus
{
    private DateTime _appliedOn;
    private readonly ICollection<Signatory> _signatories = [];

    public virtual DateTime AppliedOn
    {
        get => _appliedOn;
        set => _appliedOn = value;
    }

    public virtual ICollection<Signatory> Signatories
    {
        get => _signatories;
    }

    public MemberApplied() { }

    public MemberApplied(MemberMembershipInfo membershipInfo, IEnumerable<Signatory> signatories) : base(membershipInfo)
    {
        this._signatories = [.. signatories];
    }
}

public class MemberApproved : MembershipStatus
{
    private DateTime _approvedOn;
    private readonly ICollection<Signatory> _signatories = [];

    public virtual DateTime ApprovedOn
    {
        get => _approvedOn;
        set => _approvedOn = value;
    }

    public virtual ICollection<Signatory> Signatories
    {
        get => _signatories;
    }

    public MemberApproved() { }

    public MemberApproved(MemberMembershipInfo membershipInfo, IEnumerable<Signatory> signatories) : base(membershipInfo)
    {
        this._approvedOn = DateTime.Now;
        this._signatories = [.. signatories];
    }
}

public class MemberRegistered : MembershipStatus // technically, this is also active status
{
    private int _year;
    private DateTime _registeredOn;
    private Member _incumbentSecretary; // FIXME: this should be a officer
    private Member _incumbentPersident; // FIXME: this should be a officer

    public virtual int Year
    {
        get => _year;
        protected set => _year = value;
    }

    public virtual DateTime RegisteredOn
    {
        get => _registeredOn;
        protected set => _registeredOn = value;
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

    public MemberRegistered() { }

    public MemberRegistered(MemberMembershipInfo membershipInfo, Member incumbentSecretary, Member incumbentPresident) : base(membershipInfo)
    {
        this._year = DateTime.Now.Year;
        this._registeredOn = DateTime.Now;
        this._incumbentSecretary = incumbentSecretary;
        this._incumbentPersident = incumbentPresident;
    }
}

public class MemberRejected : MembershipStatus
{
    private DateTime _rejectedOn;
    private Signatory _rejectedBy;
    private string _reason;

    public virtual DateTime RejectedOn
    {
        get => _rejectedOn;
        protected set => _rejectedOn = value;
    }

    public virtual Signatory RejectedBy
    {
        get => _rejectedBy;
        protected set => _rejectedBy = value;
    }

    public virtual string Reason
    {
        get => _reason;
        set => _reason = value;
    }   

    public MemberRejected() { }

    public MemberRejected(MemberMembershipInfo membershipInfo, Signatory rejectedBy, string reason = null) : base(membershipInfo)
    {
        this._rejectedOn = DateTime.Now;
        this._rejectedBy = rejectedBy;
        this._reason = reason;
    }
}
