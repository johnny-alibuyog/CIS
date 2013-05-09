﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CIS.Core.Entities.Memberships;
using CIS.Data.Commons.Extentions;
using CIS.Data.Mappings.Commons;
using FluentNHibernate.Mapping;

namespace CIS.Data.Mappings.Memberships
{
    public class UserMapping : ClassMap<User>
    {
        public UserMapping()
        {
            OptimisticLock.Version();

            Id(x => x.Id);

            Version(x => x.Version);

            Map(x => x.Username);

            Map(x => x.Password);

            Map(x => x.Email);

            Component(x => x.Person);

            HasManyToMany(x => x.Roles)
                .Access.CamelCaseField(Prefix.Underscore)
                .Schema(GetType().ParseSchema())
                .Table("UsersRoles")
                .AsSet();
        }
    }
}
