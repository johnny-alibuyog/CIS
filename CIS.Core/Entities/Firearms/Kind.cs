using System;

namespace CIS.Core.Entities.Firearms
{
    public class Kind : Entity<Guid>
    {
        private string _name;

        public virtual string Name
        {
            get => _name;
            set => _name = value;
        }
    }
}
