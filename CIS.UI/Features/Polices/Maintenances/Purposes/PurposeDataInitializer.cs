﻿using CIS.Core.Entities.Commons;
using CIS.Core.Entities.Polices;
using CIS.Core.Utilities.Extentions;
using NHibernate;
using NHibernate.Linq;
using System.Collections.Generic;
using System.Linq;

namespace CIS.UI.Features.Polices.Maintenances.Purposes;

public class PurposeDataInitializer(ISessionFactory sessionFactory) : IDataInitializer
{
    private readonly ISessionFactory _sessionFactory = sessionFactory;

    public void Execute()
    {
        var data = new string[]
        {
            "Local Employment",
            "Travel Abroad"
        };

        using var session = _sessionFactory.OpenSession();
        using var transaction = session.BeginTransaction();

        var purposes = session.QueryOver<Purpose>().Cacheable().List();
        var properCasing = session.QueryOver<ProperCasingConfiguration>().Cacheable().SingleOrDefault();

        if (!properCasing.IsProperCasingInitialized)
            purposes.ForEach(x => x.Name = x.Name.ToProperCase());

        foreach (var item in data)
        {
            if (purposes.Any(x => x.Name == item))
                continue;

            var purpose = new Purpose();
            purpose.Name = item;

            session.Save(purpose);
        }

        transaction.Commit();
    }
}
