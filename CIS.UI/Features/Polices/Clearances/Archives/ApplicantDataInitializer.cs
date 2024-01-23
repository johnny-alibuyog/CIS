using NHibernate;

namespace CIS.UI.Features.Polices.Clearances.Archives;

public class ApplicantDataInitializer(ISessionFactory sessionFactory) : IDataInitializer
{
    private readonly ISessionFactory _sessionFactory = sessionFactory;

    public void Execute()
    {
        var sql = @"
                update 
	                Polices.Applicants 
                set 
	                CivilStatus = 
		                case Gender
			                when 'Male' then 'Widower'
			                when 'Female'then 'Widow'
			                else CivilStatus
		                end
                where 
	                (CivilStatus = 'Widowed') or
	                (CivilStatus = 'Widower' and Gender = 'Female') or
	                (CivilStatus = 'Widow' and Gender = 'Male');
            ";

        using var session = _sessionFactory.OpenSession();
        using var transaction = session.BeginTransaction();

        var count = session.CreateSQLQuery(sql).ExecuteUpdate();
        transaction.Commit();
    }
}
