using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using CIS.Core.Entities.Commons;

namespace CIS.Core.Entities.Polices
{
    public class Setting
    {
        private Guid _id;
        private int _version;
        private Audit _audit;
        private Terminal _terminal;
        private bool _withCameraDevice;
        private bool _withFingerScannerDevice;
        private bool _withDigitalSignatureDevice;
        private ICollection<Finger> _fingersToScan;
        private Officer _currentVerifier;
        private Officer _currentCertifier;

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
            set { _audit = value; }
        }

        public virtual Terminal Terminal
        {
            get { return _terminal; }
            set { _terminal = value; }
        }

        public virtual bool WithCameraDevice
        {
            get { return _withCameraDevice; }
            set { _withCameraDevice = value; }
        }

        public virtual bool WithFingerScannerDevice
        {
            get { return _withFingerScannerDevice; }
            set { _withFingerScannerDevice = value; }
        }

        public virtual bool WithDigitalSignatureDevice
        {
            get { return _withDigitalSignatureDevice; }
            set { _withDigitalSignatureDevice = value; }
        }

        public virtual IEnumerable<Finger> FingersToScan
        {
            get { return _fingersToScan; }
            set { SyncFingersToScan(value); }
        }

        public virtual Officer CurrentVerifier
        {
            get { return _currentVerifier; }
            set { _currentVerifier = value; }
        }

        public virtual Officer CurrentCertifier
        {
            get { return _currentCertifier; }
            set { _currentCertifier = value; }
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

        public Setting()
        {
            _fingersToScan = new Collection<Finger>();
        }

        #endregion

        #region Static Members

        public static Setting Default
        {
            get
            {
                return new Setting()
                {
                    WithCameraDevice = true,
                    WithFingerScannerDevice = true,
                    WithDigitalSignatureDevice = true,
                    FingersToScan = new Collection<Finger>()
                    {
                        Finger.RightThumb,
                        Finger.LeftThumb
                    }
                };
            }
        }

        #endregion

        #region Equality Comparer

        private Nullable<int> _hashCode;

        public override bool Equals(object obj)
        {
            var that = obj as Setting;

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

        public static bool operator ==(Setting x, Setting y)
        {
            return Equals(x, y);
        }

        public static bool operator !=(Setting x, Setting y)
        {
            return !Equals(x, y);
        }

        #endregion
    }
}
