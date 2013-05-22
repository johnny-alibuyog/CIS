using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace CIS.Core.Entities.Commons
{
    public class Terminal
    {
        private Guid _id;
        private string _machineName;
        private string _ipAddress;
        private string _macAddress;
        private bool _withDefaultLogin;
        private bool _withFingerPrintDevice;
        private ICollection<Finger> _fingersToScan;

        public virtual Guid Id
        {
            get { return _id; }
            protected set { _id = value; }
        }

        public virtual string MachineName
        {
            get { return _machineName; }
            set { _machineName = value; }
        }

        public virtual string IpAddress
        {
            get { return _ipAddress; }
            set { _ipAddress = value; }
        }

        public virtual string MacAddress
        {
            get { return _macAddress; }
            set { _macAddress = value; }
        }

        public virtual bool WithDefaultLogin
        {
            get { return _withDefaultLogin; }
            set { _withDefaultLogin = value; }
        }

        public virtual bool WithFingerPrintDevice
        {
            get { return _withFingerPrintDevice; }
            set { _withFingerPrintDevice = value; }
        }

        public virtual IEnumerable<Finger> FingersToScan
        {
            get { return _fingersToScan; }
            set { SyncFingersToScan(value); }
        }

        #region Methods

        private void SyncFingersToScan(IEnumerable<Finger> items)
        {
            var itemsToInsert = items.Except(_fingersToScan).ToList();
            var itemsToUpdate = _fingersToScan.Where(x => items.Contains(x)).ToList();
            var itemsToRemove = _fingersToScan.Except(items).ToList();

            // insert
            foreach (var item in itemsToInsert)
            {
                _fingersToScan.Add(item);
            }

            //// update
            //foreach (var item in itemsToUpdate)
            //{
            //    var value = items.Single(x => x == item);
            //    item.SerializeWith(value);
            //}

            // delete
            foreach (var item in itemsToRemove)
            {
                _fingersToScan.Remove(item);
            }
        }

        #endregion

        #region Constructors

        public Terminal()
        {
            _fingersToScan = new Collection<Finger>();
        }

        #endregion

        #region Static Members

        public static Terminal CreateLocalTerminal()
        {
            return new Terminal()
            {
                MachineName = Environment.MachineName,
                IpAddress = Dns.GetHostEntry(Dns.GetHostName()).AddressList.FirstOrDefault().ToString(),
                MacAddress = NetworkInterface.GetAllNetworkInterfaces()
                    .Where(x =>
                        x.NetworkInterfaceType == NetworkInterfaceType.Ethernet &&
                        x.OperationalStatus == OperationalStatus.Up)
                    .Select(x => x.GetPhysicalAddress().ToString())
                    .FirstOrDefault(),
                FingersToScan = new Collection<Finger>() { Finger.RightThumb, Finger.LeftThumb }
            };
        }

        #endregion

        #region Equality Comparer

        private Nullable<int> _hashCode;

        public override bool Equals(object obj)
        {
            var that = obj as Terminal;

            if (that == null)
                return false;

            if (that.Id == Guid.Empty && this.Id == Guid.Empty)
                return object.ReferenceEquals(that, this);

            return (that.Id == this.Id);
        }

        public override int GetHashCode()
        {
            if (_hashCode == null)
            {
                _hashCode = (this.Id != Guid.Empty)
                    ? this.Id.GetHashCode()
                    : base.GetHashCode();
            }

            return _hashCode.Value;
        }

        public static bool operator ==(Terminal x, Terminal y)
        {
            return Equals(x, y);
        }

        public static bool operator !=(Terminal x, Terminal y)
        {
            return !Equals(x, y);
        }

        #endregion

    }
}