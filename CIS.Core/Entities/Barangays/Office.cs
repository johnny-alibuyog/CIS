using CIS.Core.Entities.Commons;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace CIS.Core.Entities.Barangays;

public class Office : Entity<Guid>
{
    private int _version;
    private Audit _audit;
    private ImageBlob _logo = new();
    private string _name;
    private string _location;
    private Address _address;
    private decimal? _clearanceFee;
    private decimal? _certificationFee;
    private decimal? _documentStampTax;

    private ICollection<Incumbent> _incumbents = new Collection<Incumbent>();

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

    public virtual ImageBlob Logo
    {
        get => _logo;
        set => _logo = value;
    }

    public virtual string Name
    {
        get => _name;
        set => _name = value;
    }

    public virtual string Location
    {
        get => _location;
        set => _location = value;
    }

    public virtual Address Address
    {
        get => _address;
        set => _address = value;
    }

    public virtual decimal? ClearanceFee
    {
        get => _clearanceFee;
        set => _clearanceFee = value;
    }

    public virtual decimal? CertificationFee
    {
        get => _certificationFee;
        set => _certificationFee = value;
    }

    public virtual decimal? DocumentStampTax
    {
        get => _documentStampTax;
        set => _documentStampTax = value;
    }

    public virtual IEnumerable<Incumbent> Incumbents => _incumbents;
}
