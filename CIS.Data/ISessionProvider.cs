using System;
using CIS.Data.Configurations;
using NHibernate;
using NHibernate.Validator.Engine;

namespace CIS.Data
{
    public interface ISessionProvider
    {
        ISession GetSharedSession();
        ISession ReleaseSharedSession();
        ISessionFactory SessionFactory { get; }
    }
}
