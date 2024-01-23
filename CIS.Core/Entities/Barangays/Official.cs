using CIS.Core.Entities.Commons;
using System;

namespace CIS.Core.Entities.Barangays;

public class Official : Entity<Guid>
{
    private int _version;
    private Audit _audit;
    private Person _person = new();
    private Position _position;
    private Committee _committee;
    private Incumbent _incumbent;
    private ImageBlob _picture = new();
    private ImageBlob _signature = new();
    private bool _isActive;

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

    public virtual Position Position
    {
        get => _position;
        set => _position = value;
    }

    public virtual Committee Committee
    {
        get => _committee;
        set => _committee = value;
    }

    public virtual Incumbent Incumbent
    {
        get => _incumbent;
        set => _incumbent = value;
    }

    public virtual ImageBlob Picture
    {
        get => _picture;
        set => _picture = value;
    }

    public virtual ImageBlob Signature
    {
        get => _signature;
        set => _signature = value;
    }

    public virtual bool IsActive
    {
        get => _isActive;
        set => _isActive = value;
    }

    public virtual void SerializeWith(Official value)
    {
        this.Person = value.Person;
        this.Position = value.Position;
        this.Committee = value.Committee;
        this.Incumbent = value.Incumbent;
        this.Picture.Image = value.Picture.Image;
        this.Signature.Image = value.Signature.Image;
        this.IsActive = value.IsActive;
    }
}
