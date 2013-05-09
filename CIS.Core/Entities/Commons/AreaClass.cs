using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CIS.Core.Entities.Commons
{
    public enum AreaClass
    {
        Exurban,
        Suburban,
        Rural,
        Urban,
    }

    //public class BarangayClass
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

    //    public BarangayClass() { }

    //    public BarangayClass(string id, string name)
    //    {
    //        _id = id;
    //        _name = name;
    //    }

    //    #endregion

    //    #region Static Members

    //    public static readonly BarangayClass Urban = new BarangayClass("U", "Urban");
    //    public static readonly BarangayClass Rural = new BarangayClass("R", "Rural");
    //    public static readonly IEnumerable<BarangayClass> All = new BarangayClass[] { BarangayClass.Urban,  BarangayClass.Rural, };

    //    #endregion

    //    #region Equality Comparer

    //    private Nullable<int> _hashCode;

    //    public override bool Equals(object obj)
    //    {
    //        var that = obj as BarangayClass;

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

    //    public static bool operator ==(BarangayClass x, BarangayClass y)
    //    {
    //        return Equals(x, y);
    //    }

    //    public static bool operator !=(BarangayClass x, BarangayClass y)
    //    {
    //        return !Equals(x, y);
    //    }

    //    #endregion

    //    #region Method Overrides

    //    public override string ToString()
    //    {
    //        return this.Name;
    //    }

    //    #endregion
    //}
}
