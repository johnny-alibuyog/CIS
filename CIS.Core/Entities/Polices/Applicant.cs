﻿using CIS.Core.Entities.Commons;
using CIS.Core.Utilities.Extentions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace CIS.Core.Entities.Polices;

public class Applicant : Entity<Guid>
{
    private Person _person;
    private PersonBasic _father;
    private PersonBasic _mother;
    private ICollection<PersonBasic> _relatives = new Collection<PersonBasic>();
    private ICollection<Clearance> _clearances = new Collection<Clearance>();
    private Address _address;
    private Address _provincialAddress;
    private ICollection<ImageBlob> _pictures = new Collection<ImageBlob>();
    private ICollection<ImageBlob> _signatures = new Collection<ImageBlob>();
    private FingerPrint _fingerPrint = new();
    private string _height;
    private string _weight;
    private string _build;
    private string _marks;
    private string _alsoKnownAs;
    private string _birthPlace;
    private string _occupation;
    private string _religion;
    private string _citizenship;
    private string _emailAddress;
    private string _telephoneNumber;
    private string _cellphoneNumber;
    private string _passportNumber;
    private string _taxIdentificationNumber;
    private string _socialSecuritySystemNumber;
    private CivilStatus? _civilStatus;

    public virtual Person Person
    {
        get => _person;
        set => _person = value;
    }

    public virtual PersonBasic Father
    {
        get => _father;
        set => _father = value;
    }

    public virtual PersonBasic Mother
    {
        get => _mother;
        set => _mother = value;
    }

    public virtual IEnumerable<Clearance> Clearances => _clearances;

    public virtual IEnumerable<PersonBasic> Relatives
    {
        get => _relatives;
        set => SyncRelatives(value);
    }

    public virtual Address Address
    {
        get => _address;
        set => _address = value;
    }

    public virtual Address ProvincialAddress
    {
        get => _provincialAddress;
        set => _provincialAddress = value;
    }

    public virtual IEnumerable<ImageBlob> Pictures => _pictures;

    public virtual IEnumerable<ImageBlob> Signatures => _signatures;

    public virtual FingerPrint FingerPrint
    {
        get => _fingerPrint;
        set => _fingerPrint = value;
    }

    public virtual string Height
    {
        get => _height;
        set => _height = value;
    }

    public virtual string Weight
    {
        get => _weight;
        set => _weight = value;
    }

    public virtual string Build
    {
        get => _build;
        set => _build = value;
    }

    public virtual string Marks
    {
        get => _marks;
        set => _marks = value;
    }

    public virtual string AlsoKnownAs
    {
        get => _alsoKnownAs;
        set => _alsoKnownAs = value.ToProperCase();
    }

    public virtual string BirthPlace
    {
        get => _birthPlace;
        set => _birthPlace = value.ToProperCase();
    }

    public virtual string Occupation
    {
        get => _occupation;
        set => _occupation = value.ToProperCase();
    }

    public virtual string Religion
    {
        get => _religion;
        set => _religion = value.ToProperCase();
    }

    public virtual string Citizenship
    {
        get => _citizenship;
        set => _citizenship = value.ToProperCase();
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

    public virtual string PassportNumber
    {
        get => _passportNumber;
        set => _passportNumber = value;
    }

    public virtual string TaxIdentificationNumber
    {
        get => _taxIdentificationNumber;
        set => _taxIdentificationNumber = value;
    }

    public virtual string SocialSecuritySystemNumber
    {
        get => _socialSecuritySystemNumber;
        set => _socialSecuritySystemNumber = value;
    }

    public virtual CivilStatus? CivilStatus
    {
        get => _civilStatus;
        set => _civilStatus = value;
    }

    public virtual void AddClearance(Clearance clearance)
    {
        clearance.Applicant = this;
        _clearances.Add(clearance);
    }

    public virtual void AddPicture(ImageBlob picture)
    {
        _pictures.Add(picture);
    }

    public virtual void AddSignature(ImageBlob signature)
    {
        _signatures.Add(signature);
    }

    private void SyncRelatives(IEnumerable<PersonBasic> items)
    {
        var itemsToInsert = items.Except(_relatives).ToList();
        var itemsToUpdate = _relatives.Where(x => items.Contains(x)).ToList();
        var itemsToRemove = _relatives.Except(items).ToList();

        // insert
        foreach (var item in itemsToInsert)
        {
            _relatives.Add(item);
        }

        // update
        foreach (var item in itemsToUpdate)
        {
            var value = items.Single(x => x == item);
            item.SerializeWith(value);
        }

        // delete
        foreach (var item in itemsToRemove)
        {
            _relatives.Remove(item);
        }
    }
}
