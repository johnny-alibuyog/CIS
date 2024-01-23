using CIS.Core.Entities.Commons;
using System;

namespace CIS.Core.Entities.Mayors;

public class Office : Entity<Guid>
{
    private int _version;
    private Audit _audit;
    private string _name;
    private string _location;
    private Address _address;
    private Incumbent _incumbent;

    public virtual int Version
    {
        get => _version;
        protected set => _version = value;
    }

    public virtual Audit Audit
    {
        get => _audit;
        set => _audit = value;
    }

    public virtual string Name
    {
        get => _name;
        set => _name = value;
    }

    public virtual string Location
    {
        get => _location;
        set => _location = value;
    }

    public virtual Address Address
    {
        get => _address;
        set => _address = value;
    }

    public virtual Incumbent Incumbent
    {
        get => _incumbent;
        set => _incumbent = value;
    }
}
