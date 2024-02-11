using NHibernate;

namespace CIS.Data.Definition.Membership;

public class ApplicantionSeeder(ISessionFactory sessionFactory) : ISeeder
{
    private readonly ISessionFactory _sessionFactory = sessionFactory;

    public void Seed()
    {
        var sql = @"
                update 
	                Membership.Applications 
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

        session.CreateSQLQuery(sql).ExecuteUpdate();
        transaction.Commit();
    }
}
