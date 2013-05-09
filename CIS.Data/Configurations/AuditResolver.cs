using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Principal;
using System.Text;
using CIS.Core.Entities.Commons;

namespace CIS.Data.Configurations
{
    public class AuditResolver
    {
        private PropertyInfo _propertyInfo;
        private Audit _value;

        public virtual string PropertyName
        {
            get { return _propertyInfo != null ? _propertyInfo.Name : string.Empty; }
        }

        public virtual PropertyInfo PropertyInfo
        {
            get { return _propertyInfo; }
            set { _propertyInfo = value; }
        }

        public virtual Audit CurrentAudit
        {
            get { return _value; }
            set { _value = value; }
        }

        public virtual Audit CreateNew()
        {
            var createdOn = DateTime.Now;
            var createdBy = WindowsIdentity.GetCurrent().Name;
            return new Audit(createdBy: createdBy, createdOn: createdOn);
        }

        public virtual Audit CreateUpdate()
        {
            var updatedOn = DateTime.Now;
            var updatedBy = WindowsIdentity.GetCurrent().Name;
            return new Audit(currentAudit: CurrentAudit, updatedBy: updatedBy, updatedOn: updatedOn);
        }
    }
}
