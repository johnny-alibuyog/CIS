using System;

namespace CIS.Core.Entities
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
            this.Id = id;
        }

        public override bool Equals(object obj)
        {
            if (obj is Entity<T> other)
            {
                if (ReferenceEquals(this, other))
                    return true;

                if (ReferenceEquals(null, other))
                    return false;

                return Id.Equals(other.Id);
            }

            return false;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        public static bool operator ==(Entity<T> x, Entity<T> y)
        {
            if (ReferenceEquals(x, y))
                return true;

            if (ReferenceEquals(null, x) || ReferenceEquals(null, y))
                return false;

            return x.Id.Equals(y.Id);
        }

        public static bool operator !=(Entity<T> x, Entity<T> y)
        {
            return !(x == y);
        }
    }
}
