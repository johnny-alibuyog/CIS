using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CIS.Core.Entities.Commons
{
    public enum Gender
    {
        Female,
        Male
    }

    //public class Gender
    //{
    //    private string _id;
    //    private string _name;

    //    public virtual string Id 
    //    {
    //        get { return _id; }
    //        protected set { _id = value; }
    //    }

    //    public virtual string Name 
    //    {
    //        get { return _name; }
    //        protected set { _name = value; }
    //    }

    //    #region Constructors

    //    public Gender() { }

    //    public Gender(string id, string name)
    //    {
    //        _id = id;
    //        _name = name;
    //    }

    //    #endregion

    //    #region Static Members

    //    public static readonly Gender Male = new Gender("M", "Male");
    //    public static readonly Gender Female = new Gender("F", "Female");
    //    public static readonly IEnumerable<Gender> All = new Gender[] { Gender.Male, Gender.Female, };

    //    #endregion

    //    #region Equality Comparer

    //    private Nullable<int> _hashCode;

    //    public override bool Equals(object obj)
    //    {
    //        var that = obj as Gender;

    //        if (that == null)
    //            return false;

    //        if (string.IsNullOrWhiteSpace(that.Id) && string.IsNullOrWhiteSpace(this.Id))
    //            return object.ReferenceEquals(that, this);

    //        return (that.Id == this.Id);
    //    }

    //    public override int GetHashCode()
    //    {
    //        if (_hashCode == null)
    //        {
    //            _hashCode = (!string.IsNullOrWhiteSpace(this.Id))
    //                ? this.Id.GetHashCode()
    //                : base.GetHashCode();
    //        }

    //        return _hashCode.Value;
    //    }

    //    public static bool operator ==(Gender x, Gender y)
    //    {
    //        return Equals(x, y);
    //    }

    //    public static bool operator !=(Gender x, Gender y)
    //    {
    //        return !Equals(x, y);
    //    }

    //    #endregion
    //}
}
