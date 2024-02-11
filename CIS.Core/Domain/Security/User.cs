using System;
using System.Collections.Generic;
using System.Linq;
using CIS.Core.Domain.Common;

namespace CIS.Core.Domain.Security;

public class User : Entity<Guid>
{
    private int _version;
    private string _username;
    private string _password;
    private string _email;
    private Person _person;
    private ICollection<Role> _roles = [];

    public virtual int Version
    {
        get => _version;
        protected set => _version = value;
    }

    public virtual string Username
    {
        get => _username;
        set => _username = value;
    }

    public virtual string Password
    {
        get => _password;
        set => _password = value;
    }

    public virtual string Email
    {
        get => _email;
        set => _email = value;
    }

    public virtual Person Person
    {
        get => _person;
        set => _person = value;
    }

    public virtual IEnumerable<Role> Roles
    {
        get => _roles;
        set => SyncRoles(value);
    }

    private void SyncRoles(IEnumerable<Role> items)
    {
        var itemsToInsert = items.Except(_roles).ToList();
        var itemsToRemove = _roles.Except(items).ToList();

        // insert
        foreach (var item in itemsToInsert)
            _roles.Add(item);

        // delete
        foreach (var item in itemsToRemove)
            _roles.Remove(item);
    }

    public virtual bool IsSystemAdministrator()
    {
        return _roles.Any(x => x == Role.SystemAdministrator);
    }

    public virtual bool IsPoliceStaff()
    {
        return this.Has(Role.PoliceAdministartor, Role.PoliceApprover, Role.PoliceEncoder);
    }

    public virtual bool IsPoliceAdministartor()
    {
        return this.Has(Role.PoliceAdministartor);
    }

    public virtual bool IsPoliceApprover()
    {
        return this.Has(Role.PoliceApprover);
    }

    public virtual bool IsPoliceEncoder()
    {
        return this.Has(Role.PoliceEncoder);
    }

    public virtual bool IsBarangayStaff()
    {
        return this.Has(Role.BarangayAdministartor, Role.BarangayApprover, Role.BarangayEncoder);
    }

    public virtual bool IsBarangayAdministartor()
    {
        return this.Has(Role.BarangayAdministartor);
    }

    public virtual bool IsBarangayApprover()
    {
        return this.Has(Role.BarangayApprover);
    }

    public virtual bool IsBarangayEncoder()
    {
        return this.Has(Role.BarangayEncoder);
    }

    public virtual bool IsMayorStaff()
    {
        return this.Has(Role.MayorAdministrator, Role.MayorApprover, Role.MayorEncoder);
    }

    public virtual bool IsMayorAdministrator()
    {
        return this.Has(Role.MayorAdministrator);
    }

    public virtual bool IsMayorApprover()
    {
        return this.Has(Role.MayorApprover);
    }

    public virtual bool IsMayorEncoder()
    {
        return this.Has(Role.MayorEncoder);
    }

    public virtual bool Has(params Role[] roles)
    {
        if (this.IsSystemAdministrator())
            return true;

        return this.Roles.Any(x => roles.Contains(x));
    }
}
