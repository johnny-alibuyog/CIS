using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace CIS.Core.Entities.Barangays
{
    public class Position
    {
        private string _id;
        private string _name;
        private ICollection<JobDescription> _jobDescriptions;

        public virtual string Id
        {
            get { return _id; }
            protected set { _id = value; }
        }

        public virtual string Name
        {
            get { return _name; }
            protected set { _name = value; }
        }

        public virtual IEnumerable<JobDescription> JobDescriptions
        {
            get { return _jobDescriptions; }
            set { SyncJobDescriptions(value); }
        }

        #region Constructors

        public Position() 
        {
            _jobDescriptions = new Collection<JobDescription>();
        }

        public Position(string id, string name)
            : this()
        {
            _id = id;
            _name = name;
        }

        #endregion

        #region Methods

        private void SyncJobDescriptions(IEnumerable<JobDescription> items)
        {
            foreach (var item in items)
                item.Position = this;

            var itemsToInsert = items.Except(_jobDescriptions).ToList();
            var itemsToUpdate = _jobDescriptions.Where(x => items.Contains(x)).ToList();
            var itemsToRemove = _jobDescriptions.Except(items).ToList();

            // insert
            foreach (var item in itemsToInsert)
            {
                item.Position = this;
                _jobDescriptions.Add(item);
            }

            // update
            foreach (var item in itemsToUpdate)
            {
                var value = items.Single(x => x == item);
                item.SerializeWith(value);
            }

            // delete
            foreach (var item in itemsToRemove)
            {
                item.Position = null;
                _jobDescriptions.Remove(item);
            }
        }

        #endregion

        #region Static Members

        public static readonly Position BarangayCaptain = new Position("BC", "Barangay Captain")
        {
            JobDescriptions = new Collection<JobDescription>()
            {
                new JobDescription("Punong Barangay")
            }
        };

        public static readonly Position BarangayCouncilor = new Position("BL", "Barangay Councilor")
        {
            JobDescriptions = new Collection<JobDescription>()
            {
                new JobDescription("Committee on Education and Information"),
                new JobDescription("Committee on Finance and Appropriation"),
                new JobDescription("Committee on Health and Sanitation"),
                new JobDescription("Committee on Infrastractures"),
                new JobDescription("Committee on Laws, Rules and Regulations"),
                new JobDescription("Committee on Livelihod and Agriculture"),
                new JobDescription("Committee on Peace and Order and Public Safety"),
            }
        };

        public static readonly Position BarangaySecretary = new Position("BS", "Barangay Secretary")
        {
            JobDescriptions = new Collection<JobDescription>()
            {
                new JobDescription("Barangay Secretary")
            }
        };

        public static readonly Position BarangayTreasurer = new Position("BT", "Barangay Treasurer")
        {
            JobDescriptions = new Collection<JobDescription>()
            {
                new JobDescription("Barangay Treasurer")
            }
        };

        public static readonly Position Kagawad = new Position("K", "Kagawad")
        {
            JobDescriptions = new Collection<JobDescription>()
            {
                new JobDescription("Kagawad")
            }
        };

        public static readonly Position SKChairman = new Position("SKC", "SK Chairman")
        {
            JobDescriptions = new Collection<JobDescription>()
            {
                new JobDescription("SK Chairman")
            }
        };

        public static readonly IEnumerable<Position> All = new Position[] 
        { 
            Position.BarangayCaptain, 
            Position.BarangayCouncilor,
            Position.BarangaySecretary,
            Position.BarangayTreasurer, 
            Position.Kagawad, 
            Position.SKChairman
        };

        #endregion

        #region Equality Comparer

        private Nullable<int> _hashCode;

        public override bool Equals(object obj)
        {
            var that = obj as Position;

            if (that == null)
                return false;

            if (string.IsNullOrWhiteSpace(that.Id) && string.IsNullOrWhiteSpace(this.Id))
                return object.ReferenceEquals(that, this);

            return (that.Id == this.Id);
        }

        public override int GetHashCode()
        {
            if (_hashCode == null)
            {
                _hashCode = (!string.IsNullOrWhiteSpace(this.Id))
                    ? this.Id.GetHashCode()
                    : base.GetHashCode();
            }

            return _hashCode.Value;
        }

        public static bool operator ==(Position x, Position y)
        {
            return Equals(x, y);
        }

        public static bool operator !=(Position x, Position y)
        {
            return !Equals(x, y);
        }

        #endregion
    }
}
