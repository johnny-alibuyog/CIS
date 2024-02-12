using System;

namespace CIS.Core.Domain
{
    public abstract class Entity<T> where T : IEquatable<T>
    {
        private protected T _id;

        public virtual T Id
        {
            get => _id;
            protected set => _id = value;
        }

        public virtual void WithId(T id)
        {
            _id = id;
        }

        public override bool Equals(object obj)
        {
            if (obj is null)
                return false;

            if (obj is not Entity<T> other)
                return false;

            if (ReferenceEquals(this, other))
                return true;

            return Id.Equals(other.Id);
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        public static bool operator ==(Entity<T> x, Entity<T> y)
        {
            if (x is null ^ y is null)
                return false;

            if (ReferenceEquals(x, y))
                return true;

            return x.Id.Equals(y.Id);
        }

        public static bool operator !=(Entity<T> x, Entity<T> y)
        {
            return !(x == y);
        }
    }
}
