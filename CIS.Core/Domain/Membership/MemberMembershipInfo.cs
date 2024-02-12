using System;
using System.Collections.Generic;

namespace CIS.Core.Domain.Membership;

public class MemberMembershipInfo : Entity<Guid>
{
    private Member _member;
    private MembershipStatus _status;
    private Position _position;
    private readonly ICollection<MembershipStatus> _developments = [];

    public virtual Member Member
    {
        get => _member;
        set => _member = value;
    }

    public virtual MembershipStatus Status
    {
        get => _status;
        set => _status = value;
    }

    public virtual Position Position
    {
        get => _position;
        set => _position = value;
    }

    public virtual IEnumerable<MembershipStatus> Developments
    {
        get => _developments;
    }

    public virtual void WithMember(Member member)
    {
        this.Member = member;
    }
}
