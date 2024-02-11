using System;

namespace CIS.Core.Domain.Membership;

public class Religion : Entity<Guid>
{
    private string _name;

    public virtual string Name
    {
        get => _name;
        set => _name = value;
    }

    public override string ToString()
    {
        return this.Name;
    }
}
