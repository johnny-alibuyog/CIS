using System.Collections.Generic;

namespace CIS.UI.Features;

public class Lookup<T> : ViewModelBase
{
    public virtual T Id { get; set; }

    public virtual string Name { get; set; }

    public override string ToString()
    {
        return this.Name;
    }

    public Lookup() { }

    public Lookup(T id, string name)
    {
        this.Id = id;
        this.Name = name;
    }

    #region Equality Comparer


    private bool IsValueDefault(T value)
    {
        return EqualityComparer<T>.Default.Equals(value, default);
    }

    public override bool Equals(object obj)
    {
        if (obj is not Lookup<T> that)
            return false;

        if (IsValueDefault(that.Id) && IsValueDefault(this.Id))
            return object.ReferenceEquals(that, this);

        return EqualityComparer<T>.Default.Equals(that.Id, this.Id);
    }

    public override int GetHashCode()
    {
        var hashCode = (!IsValueDefault(this.Id))
                ? this.Id.GetHashCode()
                : base.GetHashCode();

        return hashCode;
    }

    #endregion
}
