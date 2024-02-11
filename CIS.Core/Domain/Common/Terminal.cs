using System;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;

namespace CIS.Core.Domain.Common;

public class Terminal : Entity<Guid>
{
    private int _version;
    private Audit _audit;
    private string _machineName;
    private string _ipAddress;
    private string _macAddress;
    private bool _withDefaultLogin;

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

    public virtual string MachineName
    {
        get => _machineName;
        set => _machineName = value;
    }

    public virtual string IpAddress
    {
        get => _ipAddress;
        set => _ipAddress = value;
    }

    public virtual string MacAddress
    {
        get => _macAddress;
        set => _macAddress = value;
    }

    public virtual bool WithDefaultLogin
    {
        get => _withDefaultLogin;
        set => _withDefaultLogin = value;
    }

    public static Terminal GetLocalTerminal()
    {
        return new()
        {
            MachineName = Environment.MachineName,
            IpAddress = Dns.GetHostEntry(Dns.GetHostName())
                .AddressList.FirstOrDefault()?
                .ToString(),
            MacAddress = NetworkInterface.GetAllNetworkInterfaces()
                .Where(x =>
                    x.NetworkInterfaceType == NetworkInterfaceType.Ethernet &&
                    x.OperationalStatus == OperationalStatus.Up
                )
                .Select(x => x.GetPhysicalAddress().ToString())
                .FirstOrDefault(),
        };
    }
}
