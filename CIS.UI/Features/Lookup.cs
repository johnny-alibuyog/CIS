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

    private int? _hashCode;

    private bool IsValueDefault(T value)
    {
        //return EqualityComparer<T>.Default.Equals(value, default(T));
        return EqualityComparer<T>.Default.Equals(value, default(T));
    }

    public override bool Equals(object obj)
    {
        var that = obj as Lookup<T>;

        if (that == null)
            return false;

        if (IsValueDefault(that.Id) && IsValueDefault(this.Id))
            return object.ReferenceEquals(that, this);

        return EqualityComparer<T>.Default.Equals(that.Id, this.Id); //(that.Id == this.Id);
    }

    public override int GetHashCode()
    {
        if (_hashCode == null)
        {
            _hashCode = (!IsValueDefault(this.Id))
                ? this.Id.GetHashCode()
                : base.GetHashCode();
        }

        return _hashCode.Value;
    }

    #endregion
}
