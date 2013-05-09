using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CIS.Core.Entities.Polices
{
    public enum ArrestStatus
    {
        Acquitted,
        Arrested,
        Bailed,
        Fugitive,
        Served
    }

    //public class ArrestStatus
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

    //    public ArrestStatus() { }

    //    public ArrestStatus(string id, string name)
    //    {
    //        _id = id;
    //        _name = name;
    //    }

    //    #endregion

    //    #region Static Members

    //    public static readonly ArrestStatus Acquitted = new ArrestStatus("AQ", "Acquitted");
    //    public static readonly ArrestStatus Arrested = new ArrestStatus("AR", "Arrested");
    //    public static readonly ArrestStatus Bailed = new ArrestStatus("BL", "Bailed");
    //    public static readonly ArrestStatus Fugitive = new ArrestStatus("FG", "Fugitive");
    //    public static readonly ArrestStatus Served = new ArrestStatus("SD", "Served");
    //    public static readonly IEnumerable<ArrestStatus> All = new ArrestStatus[] 
    //    { 
    //        ArrestStatus.Acquitted, 
    //        ArrestStatus.Bailed,
    //        ArrestStatus.Arrested, 
    //        ArrestStatus.Fugitive, 
    //        ArrestStatus.Served
    //    };

    //    #endregion

    //    #region Equality Comparer

    //    private Nullable<int> _hashCode;

    //    public override bool Equals(object obj)
    //    {
    //        var that = obj as ArrestStatus;

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

    //    public static bool operator ==(ArrestStatus x, ArrestStatus y)
    //    {
    //        return Equals(x, y);
    //    }

    //    public static bool operator !=(ArrestStatus x, ArrestStatus y)
    //    {
    //        return !Equals(x, y);
    //    }

    //    #endregion
    //}
}