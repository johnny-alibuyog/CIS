using System.Collections.Generic;
using System.Linq;
using CIS.Core.Domain.Common;
using CIS.Core.Domain.Membership;
using NHibernate;
using NHibernate.Linq;

namespace CIS.Data.Definition.Membership;

public class OfficerSeeder(ISessionFactory sessionFactory) : ISeeder
{
    private readonly ISessionFactory _sessionFactory = sessionFactory;

    public void Seed()
    {
        using var session = _sessionFactory.OpenSession();
        using var transaction = session.BeginTransaction();

        if (session.Query<Officer>().Any())
            return;

        var ranks = session.Query<Rank>().Cacheable().ToList();
        var stations = session.Query<Station>().Cacheable().ToList();
        var officers = GenerateFakeOfficer(stations, ranks);

        foreach (var officer in officers)
        {
            session.Save(officer);
        }

        transaction.Commit();
    }

    private static List<Officer> GenerateFakeOfficer(List<Station> stations, List<Rank> ranks, int count = 100)
    {
        var personGenerator = new Bogus.Faker<Person>()
            .RuleFor(x => x.Gender, x => x.PickRandom<Gender>())
            .RuleFor(x => x.Prefix, (x, y) => x.Name.Prefix(y.Gender.AsBogus()))
            .RuleFor(x => x.FirstName, (x, y) => x.Name.FirstName(y.Gender.AsBogus()))
            .RuleFor(x => x.MiddleName, (x, y) => x.Name.LastName(y.Gender.AsBogus()))
            .RuleFor(x => x.LastName, (x, y) => x.Name.LastName(y.Gender.AsBogus()))
            .RuleFor(x => x.Suffix, (x, y) => x.Name.Suffix())
            .RuleFor(x => x.BirthDate, x => x.Person.DateOfBirth);

        var officerGenerator = new Bogus.Faker<Officer>()
            .RuleFor(x => x.Person, x => personGenerator.Generate())
            .RuleFor(x => x.Station, x => x.PickRandom(stations))
            .RuleFor(x => x.Rank, x => x.PickRandom(ranks))
            .RuleFor(x => x.Position, x => x.Name.JobDescriptor());

        return officerGenerator.Generate(count);
    }
}

internal static class GenderExtension
{
    public static Bogus.DataSets.Name.Gender? AsBogus(this Gender? gender)
    {
        return gender switch
        {
            Gender.Male   => Bogus.DataSets.Name.Gender.Male,
            Gender.Female => Bogus.DataSets.Name.Gender.Female,
            _             => null
        };
    }
}
