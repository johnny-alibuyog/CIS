using System;
using System.Collections.Generic;
using System.ServiceModel;
using Touchless.Shared.Extensions;
using Touchless.Vision.Service.Client.TouchlessHost;

namespace Touchless.Vision.Service.Client
{
    public class TouchlessClient : ITouchlessHostCallback, IDisposable
    {
        private readonly TouchlessHostClient _touchlessService;

        public TouchlessClient()
        {
            var instanceContext = new InstanceContext(this);
            _touchlessService = new TouchlessHostClient(instanceContext);
            _touchlessService.RegisterClient();
        }

        #region IDisposable Members

        public void Dispose()
        {
            try
            {
                _touchlessService.UnRegisterClient();
            }
            catch (Exception)
            {
            }
        }

        #endregion

        #region ITouchlessHostCallback Members

        public void OnObjectsDetected(Frame frame, DetectedObject[] detectedObjects)
        {
            ObjectsDetected.IfNotNull(i => i(frame, detectedObjects));
        }

        public void OnNewObject(Frame frame, DetectedObject detectedObject)
        {
            NewObject.IfNotNull(i => i(frame, detectedObject));
        }

        public void OnObjectMoved(Frame frame, DetectedObject detectedObject)
        {
            ObjectMoved.IfNotNull(i => i(frame, detectedObject));
        }

        public void OnObjectRemoved(Frame frame, DetectedObject detectedObject)
        {
            ObjectRemoved.IfNotNull(i => i(frame, detectedObject));
        }

        #endregion

        public event Action<Frame, IEnumerable<DetectedObject>> ObjectsDetected;
        public event Action<Frame, DetectedObject> NewObject , ObjectMoved , ObjectRemoved;

        public void StartFrameProcessing()
        {
            _touchlessService.StartFrameProcessing();
        }

        public void StopFrameProcessing()
        {
            _touchlessService.StopFrameProcessing();
        }
    }
}