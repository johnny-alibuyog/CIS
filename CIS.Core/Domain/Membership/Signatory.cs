using System;

namespace CIS.Core.Domain.Membership;

public class Signatory : Entity<Guid>
{
    private Member _member;
    private Position _position;
    private bool _isSinged;
    private DateTime? _dateSigned;
    private SignatoryRole _role;

    public virtual Member Member 
    { 
        get => _member; 
        set => _member = value; 
    }

    public virtual Position Position 
    { 
        get => _position; 
        set => _position = value; 
    }

    public virtual bool IsSigned 
    {
        get => _isSinged; 
        internal set => _isSinged = value; 
    }

    public virtual DateTime? DateSigned 
    { 
        get => _dateSigned;
        internal set => _dateSigned = value;
    }

    public virtual SignatoryRole Role
    {
        get => _role;
        set => _role = value;
    }
}
