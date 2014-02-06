using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CIS.Core.Entities.Commons;

namespace CIS.Core.Entities.Barangays
{
    public class Blotter
    {
        private Guid _id;
        private int _version;
        private Audit _audit;
        private ICollection<Citizen> _complainants;
        private ICollection<Complaint> _complaints;
        private ICollection<Citizen> _defendants;

        public virtual Guid Id
        {
            get { return _id; }
            protected set { _id = value; }
        }

        public virtual int Version
        {
            get { return _version; }
            set { _version = value; }
        }

        public virtual Audit Audit
        {
            get { return _audit; }
            set { _audit = value; }
        }

        public virtual IEnumerable<Citizen> Complainants
        {
            get { return _complainants; }
        }

        public virtual IEnumerable<Complaint> Complaints
        {
            get { return _complaints; }
        }

        public virtual IEnumerable<Citizen> Defendants
        {
            get { return _defendants; }
        }

        public Blotter()
        {
            _complainants = new Collection<Citizen>();
            _complaints = new Collection<Complaint>();
            _defendants = new Collection<Citizen>();
        }
    }
}
