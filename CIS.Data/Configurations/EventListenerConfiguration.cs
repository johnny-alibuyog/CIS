using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate.Cfg;
using NHibernate.Event;

namespace CIS.Data.Configurations
{
    internal static class EventListenerConfiguration
    {
        public static void Configure(this Configuration configuration)
        {
            configuration.AppendListeners(ListenerType.PreInsert, new IPreInsertEventListener[] 
            {
                new AuditEventListener(), 
                new ValidationEventListener(),
            });

            configuration.AppendListeners(ListenerType.PreUpdate, new IPreUpdateEventListener[] 
            {
                new AuditEventListener(), 
                new ValidationEventListener(),
            });
        }
    }
}
