using CIS.Core.Entities.Commons;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace CIS.Core.Entities.Polices;

public class Setting : Entity<Guid>
{
    private int _version;
    private Audit _audit;
    private Terminal _terminal;
    private bool _withCameraDevice;
    private bool _withFingerScannerDevice;
    private bool _withDigitalSignatureDevice;
    private ICollection<Finger> _fingersToScan = new Collection<Finger>();
    private Officer _currentVerifier;
    private Officer _currentCertifier;

    public virtual int Version
    {
        get => _version;
        protected set => _version = value;
    }

    public virtual Audit Audit
    {
        get => _audit;
        set => _audit = value;
    }

    public virtual Terminal Terminal
    {
        get => _terminal;
        set => _terminal = value;
    }

    public virtual bool WithCameraDevice
    {
        get => _withCameraDevice;
        set => _withCameraDevice = value;
    }

    public virtual bool WithFingerScannerDevice
    {
        get => _withFingerScannerDevice;
        set => _withFingerScannerDevice = value;
    }

    public virtual bool WithDigitalSignatureDevice
    {
        get => _withDigitalSignatureDevice;
        set => _withDigitalSignatureDevice = value;
    }

    public virtual IEnumerable<Finger> FingersToScan
    {
        get => _fingersToScan;
        set => SyncFingersToScan(value);
    }

    public virtual Officer CurrentVerifier
    {
        get => _currentVerifier;
        set => _currentVerifier = value;
    }

    public virtual Officer CurrentCertifier
    {
        get => _currentCertifier;
        set => _currentCertifier = value;
    }

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

    public static Setting Default => new()
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
