using System;
using CIS.Core.Domain.Common;

namespace CIS.Core.Domain.Membership;

public class MemberPersonalInfo : Entity<Guid>
{
    private Member _member;
    private Person _person = new();
    private Address _homeAddress = new();
    private string _telephoneNumber;
    private string _mobileNumber;
    private string _emailAddress;
    private string _height;
    private string _weight;
    private BloodType? _bloodType;
    private Religion _religion;
    private Citizenship _citizenship;
    private string _emergencyContactPerson;
    private string _emergencyContactNumber;

    public virtual Member Member
    {
        get => _member;
        protected set => _member = value;
    }

    public virtual Person Person
    {
        get => _person;
        set => _person = value;
    }

    public virtual Address HomeAddress
    {
        get => _homeAddress;
        set => _homeAddress = value;
    }

    public virtual string TelephoneNumber
    {
        get => _telephoneNumber;
        set => _telephoneNumber = value;
    }

    public virtual string MobileNumber
    {
        get => _mobileNumber;
        set => _mobileNumber = value;
    }

    public virtual string EmailAddress
    {
        get => _emailAddress;
        set => _emailAddress = value;
    }

    public virtual string Height
    {
        get => _height;
        set => _height = value;
    }

    public virtual string Weight
    {
        get => _weight;
        set => _weight = value;
    }

    public virtual BloodType? BloodType
    {
        get => _bloodType;
        set => _bloodType = value;
    }

    public virtual Religion Religion
    {
        get => _religion;
        set => _religion = value;
    }

    public virtual Citizenship Citizenship
    {
        get => _citizenship;
        set => _citizenship = value;
    }

    public virtual string EmergencyContactPerson
    {
        get => _emergencyContactPerson;
        set => _emergencyContactPerson = value;
    }

    public virtual string EmergencyContactNumber
    {
        get => _emergencyContactNumber;
        set => _emergencyContactNumber = value;
    }

    public virtual void WithMember(Member member)
    {
        this.Member = member;
    }
}
