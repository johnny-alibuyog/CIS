using System;

namespace CIS.Core.Entities.Firearms;

public class Make : Entity<Guid>
{
    private string _name;

    public virtual string Name
    {
        get => _name;
        set => _name = value;
    }
}
