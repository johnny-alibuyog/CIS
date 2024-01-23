using System;
using System.Security.Principal;
using CIS.Core.Entities.Commons;
using CIS.Data.Configurations;

namespace CIS.UI.Utilities;

public class UserAuditResolver : AuditResolver
{
    private string GetCurrentUserName()
    {
        return App.Data.User != null
            ? App.Data.User.Username
            : WindowsIdentity.GetCurrent().Name;
    }

    public override Audit CreateNew()
    {
        var createdOn = DateTime.Now;
        var createdBy = this.GetCurrentUserName();

        return Audit.Create(createdBy, createdOn);
    }

    public override Audit CreateUpdate()
    {
        var updatedOn = DateTime.Now;
        var updatedBy = this.GetCurrentUserName();

        return Audit.Create(updatedBy, updatedOn, this.CurrentAudit);
    }
}
