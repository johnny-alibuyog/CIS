using System;
using CIS.Data.Configurations;
using NHibernate;
using NHibernate.Validator.Engine;

namespace CIS.Data
{
    public interface ISessionProvider
    {
        AuditResolver AuditResolver { get; set; }
        ISession GetSharedSession();
        ISession ReleaseSharedSession();
        ISessionFactory SessionFactory { get; }
        ValidatorEngine ValidatorEngine { get; }
    }
}
