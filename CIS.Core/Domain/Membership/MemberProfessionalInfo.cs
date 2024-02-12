using System;
using CIS.Core.Domain.Common;

namespace CIS.Core.Domain.Membership;

public class MemberProfessionalInfo : Entity<Guid>
{
    private Member _member;
    private string _company;
    private string _lineOfBusiness;
    private string _titleAndPosition;
    private Address _address;
    private string _telephoneNumber;
    private string _faxNumber;
    private string _gsis;
    private string _pagibig;
    private string _philHealth;
    private string _tin;

    public virtual Member Member
    {
        get => _member;
        protected set => _member = value;
    }

    public virtual string Company
    {
        get => _company;
        set => _company = value;
    }

    public virtual string LineOfBusiness
    {
        get => _lineOfBusiness;
        set => _lineOfBusiness = value;
    }

    public virtual string TitleAndPosition
    {
        get => _titleAndPosition;
        set => _titleAndPosition = value;
    }

    public virtual Address CompanyAddress
    {
        get => _address;
        set => _address = value;
    }

    public virtual string TelephoneNumber
    {
        get => _telephoneNumber;
        set => _telephoneNumber = value;
    }

    public virtual string FaxNumber
    {
        get => _faxNumber;
        set => _faxNumber = value;
    }

    public virtual string GSIS
    {
        get => _gsis;
        set => _gsis = value;
    }

    public virtual string Pagibig
    {
        get => _pagibig;
        set => _pagibig = value;
    }

    public virtual string PhilHealth
    {
        get => _philHealth;
        set => _philHealth = value;
    }

    public virtual string TIN
    {
        get => _tin;
        set => _tin = value;
    }

    public virtual void WithMember(Member member)
    {
        this.Member = member;
    }
}
