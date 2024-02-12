using System;

namespace CIS.Core.Domain.Membership;

public partial class Member : Entity<Guid>, IAggregateRoot
{
    private MemberPersonalInfo _personalInfo = new();
    private MemberProfessionalInfo _professionalInfo = new();
    private MemberDependentInfo _dependentInfo = new();
    private MemberEducationalAttainment _educationalAttainment = new();
    private MemberMembershipInfo _membershipInfo = new();

    public virtual MemberPersonalInfo PersonalInfo
    {
        get => _personalInfo;
        protected internal set => _personalInfo = value;
    }

    public virtual MemberProfessionalInfo ProfessionalInfo
    {
        get => _professionalInfo;
        protected set => _professionalInfo = value;
    }

    public virtual MemberDependentInfo DependentInfo
    {
        get => _dependentInfo;
        protected set => _dependentInfo = value;
    }

    public virtual MemberEducationalAttainment EducationalAttainment
    {
        get => _educationalAttainment;
        protected set => _educationalAttainment = value;
    }

    public virtual MemberMembershipInfo MembershipInfo
    {
        get => _membershipInfo;
        protected set => _membershipInfo = value;
    }

    public Member() 
    { 
        this._personalInfo.WithMember(this);
        this._professionalInfo.WithMember(this);
        this._dependentInfo.WithMember(this);
        this._educationalAttainment.WithMember(this);
        this._membershipInfo.WithMember(this);
    }

    public virtual void Execute(ICommand<IAggregateRoot> command)
    {
        command.Apply(this);
    }

    public virtual TResult Execute<TResult>(ICommand<IAggregateRoot, TResult> command)
    {
        return command.Apply(this);
    }
}
