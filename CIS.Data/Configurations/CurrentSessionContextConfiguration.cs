﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NHibernate.Cfg;
using NHibernate.Context;

namespace CIS.Data.Configurations
{
    public static class CurrentSessionContextConfiguration
    {
        public static void Configure(this Configuration config)
        {
            //config.SetProperty(NHibernate.Cfg.Environment.CurrentSessionContextClass, "thread_static");
            var context = typeof(ThreadStaticSessionContext).AssemblyQualifiedName;
            config.SetProperty(NHibernate.Cfg.Environment.CurrentSessionContextClass, context);
        }
    }
}
