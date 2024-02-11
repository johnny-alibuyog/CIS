using System;

namespace CIS.Core.Domain.Membership;

public partial class Member : Entity<Guid>, IAggregateRoot
{
    private MemberPersonalInformation _personalInfo = new();
    private MemberProfessionalInfomation _professionalInfo = new();
    private MemberDependentInformation _dependentInfo = new();
    private MemberEducationalAttainment _educationalAttainment = new();
    private MemberMembershipInformation _membershipInfo = new();

    public MemberPersonalInformation PersonalInfo
    {
        get => _personalInfo;
        protected internal set => _personalInfo = value;
    }

    public MemberProfessionalInfomation ProfessionalInfo
    {
        get => _professionalInfo;
        protected set => _professionalInfo = value;
    }

    public MemberDependentInformation DependentInfo
    {
        get => _dependentInfo;
        protected set => _dependentInfo = value;
    }

    public MemberEducationalAttainment EducationalAttainment
    {
        get => _educationalAttainment;
        protected set => _educationalAttainment = value;
    }

    public MemberMembershipInformation MembershipInfo
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

    public void Execute(ICommand<IAggregateRoot> command)
    {
        command.Apply(this);
    }

    public TResult Execute<TResult>(ICommand<IAggregateRoot, TResult> command)
    {
        return command.Apply(this);
    }
}
