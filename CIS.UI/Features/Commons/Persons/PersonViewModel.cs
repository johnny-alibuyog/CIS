using CIS.Core.Entities.Commons;
using CIS.Core.Utilities.Extentions;
using NHibernate.Validator.Constraints;
using System;

namespace CIS.UI.Features.Commons.Persons;

public class PersonViewModel : ViewModelBase
{
    private string _prefix;
    private string _firstName;
    private string _middleName;
    private string _lastName;
    private string _suffix;

    public virtual string Prefix
    {
        get { return _prefix; }
        set { _prefix = value; }
    }

    [NotNullNotEmpty(Message = "First name is mandatory.")]
    public virtual string FirstName
    {
        get { return _firstName; }
        set { _firstName = value; }
    }

    //[NotNullNotEmpty(Message = "Middle name is mandatory.")]
    public virtual string MiddleName
    {
        get { return _middleName; }
        set { _middleName = value; }
    }

    [NotNullNotEmpty(Message = "Last name is mandatory.")]
    public virtual string LastName 
    {
        get { return _lastName; }
        set { _lastName = value; }
    }

    public virtual string Suffix 
    {
        get { return _suffix; }
        set { _suffix = value; }
    }

    public virtual string FullName
    {
        get { return GetFullName(); }
    }

    //[NotNull(Message = "Gender is mandatory.")]
    public virtual Gender? Gender { get; set; }

    //[NotNull(Message = "BirthDate is mandatory.")]
    public virtual DateTime? BirthDate { get; set; }


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
            return target;
        }
        else if (instance is Person)
        {
            var source = instance as Person;
            var target = this;

            target.Prefix = source.Prefix;
            target.FirstName = source.FirstName;
            target.MiddleName = source.MiddleName;
            target.LastName = source.LastName;
            target.Suffix = source.Suffix;
            target.Gender = source.Gender;
            target.BirthDate = source.BirthDate;
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
        else if (instance is Person)
        {
            var source = this;
            var target = instance as Person;

            target.Prefix = source.Prefix;
            target.FirstName = source.FirstName;
            target.MiddleName = source.MiddleName;
            target.LastName = source.LastName;
            target.Suffix = source.Suffix;
            target.Gender = source.Gender;
            target.BirthDate = source.BirthDate;

            return target;
        }

        return null;
    }
}
