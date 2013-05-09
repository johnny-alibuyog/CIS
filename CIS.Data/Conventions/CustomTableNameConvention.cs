﻿using System;
using System.Collections.Generic;
using System.Data.Entity.Design.PluralizationServices;
using System.Globalization;
using System.Linq;
using System.Text;
using CIS.Data.Commons.Extentions;
using FluentNHibernate.Conventions;
using FluentNHibernate.Conventions.AcceptanceCriteria;
using FluentNHibernate.Conventions.Inspections;
using FluentNHibernate.Conventions.Instances;

namespace CIS.Data.Conventions
{
    public class CustomTableNameConvention : IClassConvention, IClassConventionAcceptance
    {
        private readonly PluralizationService _pluralizationService;

        public CustomTableNameConvention()
        {
            _pluralizationService = PluralizationService.CreateService(new CultureInfo("en-US"));
        }

        public void Apply(IClassInstance instance)
        {
            //var schema = instance.EntityType.Namespace.Split('.').Last();
            var schema = instance.EntityType.ParseSchema();
            var tableName = _pluralizationService.Pluralize(instance.EntityType.Name);

            instance.Schema(schema);
            instance.Table(tableName);
        }

        public void Accept(IAcceptanceCriteria<IClassInspector> criteria)
        {
            //criteria.Expect(x => x.TableName, Is.Not.Set);
        }
    }
}
