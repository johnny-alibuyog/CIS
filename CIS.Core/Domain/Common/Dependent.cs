using System;

namespace CIS.Core.Domain.Common;

public class Dependent : Entity<Guid>
{
    private Relationship _relationship;
    private string _name;
    private DateTime? _birthDate;
    
    public virtual Relationship Relationship
    {
        get => _relationship;
        set => _relationship = value;
    }

    public virtual string Name
    {
        get => _name;
        set => _name = value;
    }

    public virtual DateTime? BirthDate
    {
        get => _birthDate;
        set => _birthDate = value;
    }

    public virtual void SerializeWith(Dependent value)
    {
        this.Relationship = value.Relationship;
        this.Name = value.Name;
        this.BirthDate = value.BirthDate;
    }
}
