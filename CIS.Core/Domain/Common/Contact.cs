using System;

namespace CIS.Core.Domain.Common
{
    public abstract class Contact : Entity<Guid>
    {
        private string _value;

        public virtual string Value
        {
            get => _value;
            set => _value = value;
        }
    }
}
