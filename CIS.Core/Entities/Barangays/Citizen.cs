using CIS.Core.Entities.Commons;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace CIS.Core.Entities.Barangays;

public class Citizen : Entity<Guid>
{
    private int _version;
    private Audit _audit;
    private Person _person;
    private CivilStatus? _civilStatus;
    private string _alsoKnownAs;
    private string _birthPlace;
    private string _occupation;
    private string _religion;
    private string _citizenship;
    private string _emailAddress;
    private string _telephoneNumber;
    private string _cellphoneNumber;
    private Address _currentAddress;
    private Address _provincialAddress;
    private FingerPrint _fingerPrint = new();
    private ICollection<ImageBlob> _pictures = new Collection<ImageBlob>();
    private ICollection<ImageBlob> _signatures = new Collection<ImageBlob>();

    public virtual int Version
    {
        get => _version;
        set => _version = value;
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

    public virtual CivilStatus? CivilStatus
    {
        get => _civilStatus;
        set => _civilStatus = value;
    }

    public virtual string AlsoKnownAs
    {
        get => _alsoKnownAs;
        set => _alsoKnownAs = value;
    }

    public virtual string BirthPlace
    {
        get => _birthPlace;
        set => _birthPlace = value;
    }

    public virtual string Occupation
    {
        get => _occupation;
        set => _occupation = value;
    }

    public virtual string Religion
    {
        get => _religion;
        set => _religion = value;
    }

    public virtual string Citizenship
    {
        get => _citizenship;
        set => _citizenship = value;
    }

    public virtual string EmailAddress
    {
        get => _emailAddress;
        set => _emailAddress = value;
    }

    public virtual string TelephoneNumber
    {
        get => _telephoneNumber;
        set => _telephoneNumber = value;
    }

    public virtual string CellphoneNumber
    {
        get => _cellphoneNumber;
        set => _cellphoneNumber = value;
    }

    public virtual Address CurrentAddress
    {
        get => _currentAddress;
        set => _currentAddress = value;
    }

    public virtual Address ProvincialAddress
    {
        get => _provincialAddress;
        set => _provincialAddress = value;
    }

    public virtual FingerPrint FingerPrint
    {
        get => _fingerPrint;
        set => _fingerPrint = value;
    }

    public virtual IEnumerable<ImageBlob> Pictures => _pictures;

    public virtual IEnumerable<ImageBlob> Signatures => _signatures;

    #region Methods

    public virtual void SerializeWith(Citizen value)
    {
        this.Person = value.Person;
        this.CivilStatus = value.CivilStatus;
        this.AlsoKnownAs = value.AlsoKnownAs;
        this.BirthPlace = value.BirthPlace;
        this.Occupation = value.Occupation;
        this.Religion = value.Religion;
        this.Citizenship = value.Citizenship;
        this.EmailAddress = value.EmailAddress;
        this.TelephoneNumber = value.TelephoneNumber;
        this.CellphoneNumber = value.CellphoneNumber;
        this.CurrentAddress = value.CurrentAddress;
        this.ProvincialAddress = value.ProvincialAddress;
        //this.FingerPrint.SerializeWith(value.FingerPrint);
        //this.Pictures = value.Pictures;
        //this.Signatures = value.Signatures;
    }

    public virtual void AddPicture(ImageBlob picture)
    {
        _pictures.Add(picture);
    }

    public virtual void AddSignature(ImageBlob signature)
    {
        _signatures.Add(signature);
    }

    #endregion
}
