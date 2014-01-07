using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ServiceStack.DataAnnotations;

namespace CIS.Store.Domain.Entities
{
    public class 
        Warrant
    {
        [AutoIncrement()]
        public virtual long Id { get; set; }

        public virtual Guid ParentKey { get; set; }

        public virtual Guid ChildKey { get; set; }

        [Index()]
        [StringLength(50)]
        public virtual string WarrantCode { get; set; }

        [StringLength(50)]
        public virtual string CaseNumber { get; set; }

        [Index()]
        [StringLength(300)]
        public virtual string Crime { get; set; }

        [StringLength(4001)]
        public virtual string Description { get; set; }

        [StringLength(4001)]
        public virtual string Remarks { get; set; }

        public virtual decimal BailAmount { get; set; }

        public virtual Nullable<DateTime> IssuedOn { get; set; }

        [StringLength(300)]
        public virtual string IssuedBy { get; set; }

        [StringLength(700)]
        public virtual Address IssuedAt { get; set; }

        [StringLength(100)]
        public virtual string ArrestStatus { get; set; }

        public virtual Nullable<DateTime> ArrestDate { get; set; }

        [StringLength(700)]
        public virtual string Disposition { get; set; }

        [Index()]
        [StringLength(150)]
        public virtual string SuspectFirstName { get; set; }

        [Index()]
        [StringLength(150)]
        public virtual string SuspectMiddleName { get; set; }

        [Index()]
        [StringLength(150)]
        public virtual string SuspectLastName { get; set; }

        [StringLength(150)]
        public virtual string SuspectSuffix { get; set; }

        [StringLength(100)]
        public virtual string SuspectGender { get; set; }

        public virtual Nullable<DateTime> SuspectBirthDate { get; set; }

        [StringLength(700)]
        public virtual Address SuspectAddress { get; set; }

        [StringLength(1750)]
        public virtual PhysicalAttributes SuspectPhysicalAttributes { get; set; }

        [StringLength(100)]
        public virtual List<string> SuspectAliases { get; set; }

        [StringLength(150)]
        public virtual List<string> SuspectOccupations { get; set; }
    }
}