using System;

namespace CIS.Core.Domain.Common;

public class Skill : Entity<Guid>
{
    private string _name;

    public virtual string Name
    {
        get => _name;
        set => _name = value;
    }

    public virtual void SerializeWith(Skill value)
    {
        Name = value.Name;
    }

    public override string ToString()
    {
        return this.Name;
    }

    public override bool Equals(object obj)
    {
        if (obj is null)
            return false;

        if (obj is not Skill other)
            return false;

        if (ReferenceEquals(this, other))
            return true;

        if (Id == other.Id)
            return true;

        if (Name == other.Name)
            return true;

        return false;
    }

    public override int GetHashCode()
    {
        if (Id != default)
            return Id.GetHashCode();

        if (Name != default)
            return Name.GetHashCode();

        return 0;
    }

    public static bool operator ==(Skill x, Skill y)
    {
        if (x is null ^ y is null)
            return false;

        if (ReferenceEquals(x, y))
            return true;

        if (x.Id == y.Id)
            return true;

        if (x.Name == y.Name)
            return true;

        return false;
    }

    public static bool operator !=(Skill x, Skill y)
    {
        return !(x == y);
    }

}
