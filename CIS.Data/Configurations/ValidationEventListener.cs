using CIS.Data.Commons.Exceptions;
using NHibernate.Event;
using System.Linq;

namespace CIS.Data.Configurations;

public class ValidationEventListener : IPreInsertEventListener, IPreUpdateEventListener
{
    private void PerformValidation(object entity)
    {
        var validator = SessionProvider.Validator;
        var invalidValues = validator.Validate(entity);
        if (invalidValues.Count() > 0)
            throw new BusinessExceptionBuilder().Build(invalidValues);
    }

    public bool OnPreInsert(PreInsertEvent @event)
    {
        PerformValidation(@event.Entity);
        return false;
    }

    public bool OnPreUpdate(PreUpdateEvent @event)
    {
        PerformValidation(@event.Entity);
        return false;
    }
}
