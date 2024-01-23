using CIS.Core.Entities.Commons;
using System;

namespace CIS.Core.Entities.Mayors;

public abstract class Official : Entity<Guid>
{
    private int _version;
    private Audit _audit;
    private Person _person;

    public virtual int Version
    {
        get => _version;
        protected set => _version = value;
    }

    public virtual Audit Audit
    {
        get => _audit;
        set => _audit = value;
    }

    public virtual Person Person
    {
        get => _person;
        set => _person = value;
    }

    #region Methods

    public virtual void SerializeWith(Official value)
    {
        this.Person = value.Person;
    }

    #endregion
}
