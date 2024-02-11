using CIS.Core.Domain.Common;
using CIS.Core.Utility.Extention;
using NHibernate.Validator.Constraints;
using System;

namespace CIS.UI.Features.Common.Person;

public class PersonViewModel : ViewModelBase
{
    private string _prefix;
    private string _firstName;
    private string _middleName;
    private string _lastName;
    private string _suffix;
    private Gender? _gender;
    private DateTime? _birthDate;
    private string _birthPlace;

    public virtual string Prefix
    {
        get => _prefix;
        set => _prefix = value;
    }

    [NotNullNotEmpty(Message = "First name is mandatory.")]
    public virtual string FirstName
    {
        get => _firstName;
        set => _firstName = value;
    }

    [NotNullNotEmpty(Message = "Middle name is mandatory.")]
    public virtual string MiddleName
    {
        get => _middleName;
        set => _middleName = value;
    }

    [NotNullNotEmpty(Message = "Last name is mandatory.")]
    public virtual string LastName
    {
        get => _lastName;
        set => _lastName = value;
    }

    public virtual string Suffix
    {
        get => _suffix;
        set => _suffix = value;
    }

    public virtual string FullName
    {
        get => GetFullName();
    }

    [NotNull(Message = "Gender is mandatory.")]
    public virtual Gender? Gender
    {
        get => _gender;
        set => _gender = value;
    }

    [NotNull(Message = "BirthDate is mandatory.")]
    public virtual DateTime? BirthDate
    {
        get => _birthDate;
        set => _birthDate = value;
    }

    public virtual string BirthPlace
    {
        get => _birthPlace;
        set => _birthPlace = value;
    }

    private string GetFullName()
    {
        return
        (
            (!string.IsNullOrWhiteSpace(this.Prefix) ? this.Prefix : string.Empty) +
            (!string.IsNullOrWhiteSpace(this.FirstName) ? " " + this.FirstName : string.Empty) +
            (!string.IsNullOrWhiteSpace(this.MiddleName) ? " " + this.MiddleName : string.Empty) +
            (!string.IsNullOrWhiteSpace(this.LastName) ? " " + this.LastName : string.Empty) +
            (!string.IsNullOrWhiteSpace(this.Suffix) ? " " + this.Suffix : string.Empty)
        )
        .ToProperCase()
        .Trim();
    }

    public override string ToString()
    {
        return this.GetFullName();
    }

    public override object SerializeWith(object instance)
    {
        if (instance == null)
            return null;

        if (instance is PersonViewModel)
        {
            var source = instance as PersonViewModel;
            var target = this;

            target.Prefix = source.Prefix;
            target.FirstName = source.FirstName;
            target.MiddleName = source.MiddleName;
            target.LastName = source.LastName;
            target.Suffix = source.Suffix;
            target.Gender = source.Gender;
            target.BirthDate = source.BirthDate;
            target.BirthPlace = source.BirthPlace;
            return target;
        }
        else if (instance is Core.Domain.Common.Person)
        {
            var source = instance as Core.Domain.Common.Person;
            var target = this;

            target.Prefix = source.Prefix;
            target.FirstName = source.FirstName;
            target.MiddleName = source.MiddleName;
            target.LastName = source.LastName;
            target.Suffix = source.Suffix;
            target.Gender = source.Gender;
            target.BirthDate = source.BirthDate;
            target.BirthPlace = source.BirthPlace;
            return target;
        }

        return null;
    }

    public override object DeserializeInto(object instance)
    {
        if (instance == null)
            return null;

        if (instance is PersonViewModel)
        {
            var source = this;
            var target = instance as PersonViewModel;

            target.SerializeWith(source);
            return target;
        }
        else if (instance is Core.Domain.Common.Person)
        {
            var source = this;
            var target = instance as Core.Domain.Common.Person;

            target.Prefix = source.Prefix;
            target.FirstName = source.FirstName;
            target.MiddleName = source.MiddleName;
            target.LastName = source.LastName;
            target.Suffix = source.Suffix;
            target.Gender = source.Gender;
            target.BirthDate = source.BirthDate;
            target.BirthPlace = source.BirthPlace;

            return target;
        }

        return null;
    }
}
