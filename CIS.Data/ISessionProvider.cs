using NHibernate;

namespace CIS.Data;

public interface ISessionProvider
{
    ISession GetSharedSession();
    ISession ReleaseSharedSession();
    ISessionFactory SessionFactory { get; }
}
