﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using CIS.Core.Entities.Commons;
using CIS.Core.Entities.Memberships;
using CIS.Data.Configurations;

namespace CIS.UI.Utilities
{
    public class UserAuditResolver : AuditResolver
    {
        public static User CurrentUser { get; set; }

        private string GetCurrentUserName()
        {
            return UserAuditResolver.CurrentUser != null
                ? UserAuditResolver.CurrentUser.Username
                : WindowsIdentity.GetCurrent().Name;
        }

        public override Audit CreateNew()
        {
            var createdOn = DateTime.Now;
            var createdBy = this.GetCurrentUserName();

            return new Audit(createdBy, createdOn);
        }

        public override Audit CreateUpdate()
        {
            var updatedOn = DateTime.Now;
            var updatedBy = this.GetCurrentUserName();

            return new Audit(this.CurrentAudit, updatedBy, updatedOn);
        }
    }
}
