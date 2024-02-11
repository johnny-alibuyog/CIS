using System.Collections.Generic;

namespace CIS.Core.Domain.Membership;

public class MemberMembershipInformation
{
    private Member _member;
    private MembershipStatus _status;
    private Position _position;
    private readonly ICollection<MembershipStatus> _developments = [];

    public Member Member
    {
        get => _member;
        set => _member = value;
    }

    public MembershipStatus Status
    {
        get => _status;
        set => _status = value;
    }

    public Position Position
    {
        get => _position;
        set => _position = value;
    }

    public IEnumerable<MembershipStatus> Developments
    {
        get => _developments;
    }

    internal virtual void WithMember(Member member)
    {
        this.Member = member;
    }
}
