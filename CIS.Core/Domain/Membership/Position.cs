using System;

namespace CIS.Core.Domain.Membership;

public class Position : Entity<Guid>
{
    private string _name;

    public virtual string Name
    {
        get => _name;
        set => _name = value;
    }

    public virtual void SerializeWith(Position value)
    {
        this.Name = value.Name;
    }
}
