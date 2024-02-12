using System.Collections.Generic;
using System.Linq;

namespace CIS.Core.Domain;

public abstract class ValueObject
{
    protected abstract IEnumerable<object> GetEqualityValues();

    public static bool operator ==(ValueObject left, ValueObject right)
    {
        if (left is null ^ right is null)
            return false;

        if (ReferenceEquals(left, right))
            return true;

        return left.Equals(right);
    }

    public static bool operator !=(ValueObject one, ValueObject two)
    {
        return !(one == two);
    }

    public override bool Equals(object obj)
    {
        if (obj is null)
            return false;

        if (obj.GetType() != GetType())
            return false;

        var that = (ValueObject)obj;

        return this.GetEqualityValues().SequenceEqual(that.GetEqualityValues());
    }

    public override int GetHashCode()
    {
        return GetEqualityValues()
            .Select(x => x?.GetHashCode() ?? 0)
            .Aggregate((x, y) => x ^ y);
    }
}
