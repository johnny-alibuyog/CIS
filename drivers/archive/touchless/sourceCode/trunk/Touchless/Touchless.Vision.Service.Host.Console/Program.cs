using System;
using System.ServiceModel;
using System.ServiceModel.Description;

namespace Touchless.Vision.Service.Host.Console
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            try
            {
                using (var serviceHost = new ServiceHost(typeof (TouchlessHost)))
                {
                    var baseAddress = new Uri("net.pipe://localhost/touchless/host");
                    var binding = new NetNamedPipeBinding(NetNamedPipeSecurityMode.None);
                    binding.MaxBufferSize = 66665536;
                    binding.MaxBufferPoolSize = 66665536;
                    binding.MaxReceivedMessageSize = 66665536;
                    serviceHost.AddServiceEndpoint(typeof (ITouchlessHost), binding, baseAddress);

                    var smb = new ServiceMetadataBehavior();
                    smb.HttpGetEnabled = true;
                    smb.HttpGetUrl = new Uri("http://localhost:8000/touchless/host");
                    serviceHost.Description.Behaviors.Add(smb);

                    serviceHost.Open();
                    System.Console.WriteLine("Service Running.");
                    System.Console.In.ReadLine();
                }
            }
            catch (Exception err)
            {
                System.Console.WriteLine(err.Message);
                System.Console.In.ReadLine();
            }
        }
    }
}