using CIS.Core.Domain.Common;
using NHibernate.Event;
using NHibernate.Persister.Entity;
using System;
using System.Linq;

namespace CIS.Data.Configurations;

internal class AuditEventListener : IPreInsertEventListener, IPreUpdateEventListener, IPreDeleteEventListener
{
    #region Routine Helpers

    private void Set(IEntityPersister persister, object[] state, string propertyName, object value)
    {
        var index = Array.IndexOf(persister.PropertyNames, propertyName);
        if (index == -1)
            return;

        state[index] = value;
    }

    private AuditResolver GetAuditResolver(AbstractPreDatabaseOperationEvent @event)
    {
        var auditInfo = @event.Entity
            .GetType()
            .GetProperties()
            .Where(x => x.PropertyType == typeof(Audit))
            .Select(x => new
            {
                PropertyInfo = x,
                Value = x.GetValue(@event.Entity, null) as Audit
            })
            .FirstOrDefault();

        if (auditInfo == null)
            return null;

        var auditResolver = SessionProvider.AuditResolver;
        auditResolver ??= new AuditResolver();
        auditResolver.PropertyInfo = auditInfo.PropertyInfo;
        auditResolver.CurrentAudit = auditInfo.Value;

        return auditResolver;
    }

    #endregion

    public bool OnPreInsert(PreInsertEvent @event)
    {
        var auditResolver = GetAuditResolver(@event);
        if (auditResolver == null)
            return false;

        var newAudit = auditResolver.CreateNew();

        Set(@event.Persister, @event.State, auditResolver.PropertyName, newAudit);

        auditResolver.PropertyInfo.SetValue(@event.Entity, newAudit, null);

        return false;
    }

    public bool OnPreUpdate(PreUpdateEvent @event)
    {
        var auditResolver = GetAuditResolver(@event);
        if (auditResolver == null)
            return false;

        var updatedAudit = auditResolver.CreateUpdate();

        Set(@event.Persister, @event.State, auditResolver.PropertyName, updatedAudit);

        auditResolver.PropertyInfo.SetValue(@event.Entity, updatedAudit, null);

        return false;
    }

    public bool OnPreDelete(PreDeleteEvent eventObject)
    {
        throw new NotImplementedException();
    }
}
