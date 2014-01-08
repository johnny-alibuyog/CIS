using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ServiceStack.DataAnnotations;

namespace CIS.Store.Domain.Entities
{
    public class AuditTrail
    {
        [AutoIncrement()]
        public virtual long Id { get; set; }

        [Required()]
        [StringLength(50)]
        public virtual string User { get; set; }

        [Required()]
        [StringLength(50)]
        public virtual string Origin { get; set; }

        [Required()]
        public virtual Nullable<DateTimeOffset> Date { get; set; }

        [Required()]
        [StringLength(50)]
        public virtual string Service { get; set; }

        [Required()]
        [StringLength(50)]
        public virtual string Operation { get; set; }

        [Required()]
        [StringLength(250)]
        public virtual string Remarks { get; set; }

        public AuditTrail()
        {
            Date = DateTimeOffset.UtcNow;
        }

        public AuditTrail(string user, string origin, string service, string operation, string remarks)
            : this()
        {
            this.User = user;
            this.Origin = origin;
            this.Service = service;
            this.Operation = operation;
            this.Remarks = remarks;
        }
    }
}