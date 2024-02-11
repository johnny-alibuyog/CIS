using CIS.Core.Domain.Common;
using CIS.Core.Utility.Extention;
using System;

namespace CIS.Core.Domain.Membership;

public class Officer : Entity<Guid>
{
    private string _position;
    private int _version;
    private Audit _audit;
    private Person _person;
    private Station _station;
    private Rank _rank;
    private ImageBlob _signature = new();

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

    public virtual Station Station
    {
        get => _station;
        set => _station = value;
    }

    public virtual Rank Rank
    {
        get => _rank;
        set => _rank = value;
    }

    public virtual string Position
    {
        get => _position;
        set => _position = value.ToProperCase();
    }

    public virtual ImageBlob Signature
    {
        get => _signature;
        set => _signature = value;
    }

    public virtual void SerializeWith(Officer value)
    {
        this.Person = value.Person;
        this.Station = value.Station;
        this.Rank = value.Rank;
        this.Position = value.Position;
        this.Signature = value.Signature;
    }
}
