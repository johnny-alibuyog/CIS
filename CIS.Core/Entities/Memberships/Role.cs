using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace CIS.Core.Entities.Memberships
{
    public enum Role
    {
        [Description("System Administrator")]
        SystemAdministrator = 0,

        [Description("Police Administartor")]
        PoliceAdministartor = 101,

        [Description("Police Approver")]
        PoliceApprover = 102,

        [Description("Police Encoder")]
        PoliceEncoder = 103,

        [Description("Barangay Administartor")]
        BarangayAdministartor = 201,

        [Description("Barangay Approver")]
        BarangayApprover = 202,

        [Description("Barangay Encoder")]
        BarangayEncoder = 203,

        [Description("Mayor Administrator")]
        MayorAdministrator = 301,

        [Description("Mayor Approver")]
        MayorApprover = 302,

        [Description("Mayor Encoder")]
        MayorEncoder = 303,
    }

    //public class Role
    //{
    //    private string _id;
    //    private string _name;

    //    public virtual string Id
    //    {
    //        get { return _id; }
    //        set { _id = value; }
    //    }

    //    public virtual string Name
    //    {
    //        get { return _name; }
    //        set { _name = value; }
    //    }

    //    #region Constructors

    //    public Role() { }

    //    public Role(string id, string name)
    //        : this()
    //    {
    //        _id = id;
    //        _name = name;
    //    }

    //    #endregion

    //    #region Static Members

    //    public static readonly Role SystemAdministrator = new Role("SA", "System Administrator");
    //    public static readonly Role PoliceAdministartor = new Role("PA", "Police Administrator");
    //    public static readonly Role PoliceApprover = new Role("PAP", "Police Approver");
    //    public static readonly Role PoliceEncoder = new Role("PE", "Police Encoder");
    //    public static readonly Role BarangayAdministartor = new Role("BA", "Barangay Administrator");
    //    public static readonly Role BarangayApprover = new Role("BAP", "Barangay Approver");
    //    public static readonly Role BarangayEncoder = new Role("BAP", "Barangay Encoder");
    //    public static readonly Role MayorAdministrator = new Role("MA", "Mayor Aministrator");
    //    public static readonly Role MayorApprover = new Role("M", "Mayor Approver");
    //    public static readonly Role MayorEncoder = new Role("ME", "Mayor Encoder");
    //    public static readonly IEnumerable<Role> All = new Role[] 
    //    { 
    //        SystemAdministrator, 
    //        PoliceAdministartor, 
    //        PoliceApprover, 
    //        PoliceEncoder, 
    //        BarangayAdministartor, 
    //        BarangayApprover, 
    //        BarangayEncoder, 
    //        MayorAdministrator,
    //        MayorApprover,
    //        MayorEncoder,
    //    };

    //    public static IEnumerable<Role> GetByIds(params string[] ids)
    //    {
    //        return Role.All.Where(x => ids.Contains(x.Id));
    //    }

    //    #endregion

    //    #region Equality Comparer

    //    private Nullable<int> _hashCode;

    //    public override bool Equals(object obj)
    //    {
    //        var that = obj as Role;

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

    //    public static bool operator ==(Role x, Role y)
    //    {
    //        return Equals(x, y);
    //    }

    //    public static bool operator !=(Role x, Role y)
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