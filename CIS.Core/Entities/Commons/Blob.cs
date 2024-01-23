using System;

namespace CIS.Core.Entities.Commons;

public abstract class Blob : Entity<Guid>
{
    private byte[] _bytes;

    public virtual byte[] Bytes 
    { 
        get => _bytes; 
        protected set => _bytes = value; 
    }
}
