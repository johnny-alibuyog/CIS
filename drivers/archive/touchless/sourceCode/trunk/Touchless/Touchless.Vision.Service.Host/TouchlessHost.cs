using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.ComponentModel.Composition.Hosting;
using System.Reflection;
using Touchless.Shared.Extensions;
using Touchless.Vision.Service;

namespace Touchless.Vision.Service.Host
{
    public class TouchlessHost : ITouchlessHost
    {
        private TouchlessService _touchlessService;
        private readonly List<ITouchlessClient> _clients;

        private IEnumerable<ITouchlessClient> OpenClients
        {
            get
            {
                foreach (var client in _clients)
                {
                    if (((ICommunicationObject)client).State == CommunicationState.Opened)
                    {
                        yield return client;
                    }
                    else
                    {
                        _clients.Remove(client);
                    }
                }
            }
        }

        public TouchlessHost()
        {
            _clients = new List<ITouchlessClient>();
            InitializeTouchlessService();
        }

        private void InitializeTouchlessService()
        {
            _touchlessService = ComposableTouchlessService.GetComposedInstance();
            _touchlessService.NewObject += (s, o, f) => OpenClients.ForEach(i => i.OnNewObject(f, o));
            _touchlessService.ObjectMoved += (s, o, f) => OpenClients.ForEach(i => i.OnObjectMoved(f, o));
            _touchlessService.ObjectRemoved += (s, o, f) => OpenClients.ForEach(i => i.OnObjectRemoved(f, o));
            _touchlessService.ObjectsDetected += (f, o) => OpenClients.ForEach(i => i.OnObjectsDetected(f, o));
        }

        public bool RegisterClient()
        {
            try
            {
                var touchlessClient = OperationContext.Current.GetCallbackChannel<ITouchlessClient>();
                if (touchlessClient != null && !_clients.Contains(touchlessClient))
                {
                    _clients.Add(touchlessClient);
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool UnRegisterClient()
        {
            try
            {
                var touchlessClient = OperationContext.Current.GetCallbackChannel<ITouchlessClient>();
                if (touchlessClient != null && _clients.Contains(touchlessClient))
                {
                    _clients.Remove(touchlessClient);
                }

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public void StartFrameProcessing()
        {
            lock (_touchlessService)
            {
                if (!_touchlessService.IsFrameProcessing)
                {
                    _touchlessService.StartFrameProcessing();
                }
            }
        }

        public void StopFrameProcessing()
        {
            lock (_touchlessService)
            {
                if (_touchlessService.IsFrameProcessing)
                {
                    _touchlessService.StopFrameProcessing();
                }
            }
        }
    }
}
