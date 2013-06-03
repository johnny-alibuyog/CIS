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
        private int _version;
        private Audit _audit;
        private string _machineName;
        private string _ipAddress;
        private string _macAddress;
        private bool _withDefaultLogin;

        public virtual Guid Id
        {
            get { return _id; }
            protected set { _id = value; }
        }

        public virtual int Version
        {
            get { return _version; }
            protected set { _version = value; }
        }

        public virtual Audit Audit
        {
            get { return _audit; }
            set {_audit = value;}
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