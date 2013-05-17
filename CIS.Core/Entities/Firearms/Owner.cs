using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CIS.Core.Entities.Commons;

namespace CIS.Core.Entities.Firearms
{
    public class Owner
    {
        private Guid _id;
        private Person _person;
        private Address _address;
        private ICollection<Gun> _guns;

        public virtual Guid Id
        {
            get { return _id; }
            protected set { _id = value; }
        }

        public virtual Person Person
        {
            get { return _person; }
            set { _person = value; }
        }

        public virtual Address Address
        {
            get { return _address; }
            set { _address = value; }
        }

        public virtual IEnumerable<Gun> Guns
        {
            get { return _guns; }

        }

        #region Constructors

        public Owner()
        {
            _guns = new Collection<Gun>();
        }

        #endregion

        #region Methods

        private void SyncGuns(IEnumerable<Gun> items)
        {
            foreach (var item in items)
                item.Owner = this;

            var itemsToInsert = items.Except(_guns).ToList();
            var itemsToUpdate = _guns.Where(x => items.Contains(x)).ToList();
            var itemsToRemove = _guns.Except(items).ToList();

            // insert
            foreach (var item in itemsToInsert)
            {
                item.Owner = this;
                _guns.Add(item);
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
                item.Owner = null;
                _guns.Remove(item);
            }
        }

        #endregion
    }
}
