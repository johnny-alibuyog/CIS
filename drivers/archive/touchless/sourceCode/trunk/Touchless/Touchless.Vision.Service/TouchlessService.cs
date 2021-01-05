using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Touchless.Shared.Extensions;
using Touchless.Vision.Contracts;

namespace Touchless.Vision.Service
{
    public class TouchlessService : IDisposable
    {
        private readonly Action<IFrameSource, Frame, double> _onNewFrameAction;

        private readonly Action<IObjectDetector, DetectedObject, Frame> _newObjectAction,
                                                                        _objectMovedAction,
                                                                        _objectRemovedAction;

        public TouchlessService()
        {
            _newObjectAction = (d, o, f) => NewObject.IfNotNull(i => i(d, o, f));
            _objectMovedAction = (d, o, f) => ObjectMoved.IfNotNull(i => i(d, o, f));
            _objectRemovedAction = (d, o, f) => ObjectRemoved.IfNotNull(i => i(d, o, f));

            IsFrameProcessing = false;
            RegisteredFrameSourcesInternal = new List<IFrameSource>();
            RegisteredObjectDetectorsInternal = new List<IObjectDetector>();

            _onNewFrameAction = (s, f, d) => OnNewFrame(s, f);
        }

        public bool IsFrameProcessing { get; private set; }
        private IList<IFrameSource> RegisteredFrameSourcesInternal { get; set; }
        private IList<IObjectDetector> RegisteredObjectDetectorsInternal { get; set; }

        public ReadOnlyCollection<IFrameSource> RegisteredFrameSources
        {
            get { return RegisteredFrameSourcesInternal.ToList().AsReadOnly(); }
        }

        public ReadOnlyCollection<IObjectDetector> RegisteredObjectDetectors
        {
            get { return RegisteredObjectDetectorsInternal.ToList().AsReadOnly(); }
        }

        #region IDisposable Members

        public void Dispose()
        {
            if (IsFrameProcessing)
            {
                StopFrameProcessing();
            }
        }

        #endregion

        public event Action<IFrameSource, Frame> NewFrame;
        public event Action<Frame, ReadOnlyCollection<DetectedObject>> ObjectsDetected;
        public event Action<IObjectDetector, DetectedObject, Frame> NewObject , ObjectMoved , ObjectRemoved;

        public virtual void StartFrameProcessing()
        {
            RegisteredFrameSourcesInternal.ForEach(i => i.StartFrameCapture());
            IsFrameProcessing = true;
        }

        public virtual void StopFrameProcessing()
        {
            RegisteredFrameSourcesInternal.ForEach(i => i.StopFrameCapture());
            IsFrameProcessing = false;
        }

        protected virtual void OnNewFrame(IFrameSource frameSource, Frame frame)
        {
            var detectedObjects = new List<DetectedObject>();

            foreach (var objectDetector in RegisteredObjectDetectorsInternal)
            {
                var result = objectDetector.DetectObjects(frame);
                result.ForEach(detectedObjects.Add);
            }

            NewFrame.IfNotNull(i => i(frameSource, frame));

            if (detectedObjects.Count > 0 && ObjectsDetected != null)
            {
                ObjectsDetected(frame, detectedObjects.AsReadOnly());
            }
        }

        #region Manage Event Handlers

        protected void RegisterFrameSourceEventHandlers(IFrameSource frameSource)
        {
            frameSource.NewFrame += _onNewFrameAction;
        }

        protected void UnregisterFrameSourceEventHandlers(IFrameSource frameSource)
        {
            frameSource.NewFrame -= _onNewFrameAction;
        }

        protected void RegisterObjectDetectorEventHandlers(IObjectDetector objectTrackingService)
        {
            objectTrackingService.NewObject += _newObjectAction;
            objectTrackingService.ObjectMoved += _objectMovedAction;
            objectTrackingService.ObjectRemoved += _objectRemovedAction;
        }

        protected void UnregisterObjectDetectorEventHandlers(IObjectDetector objectTrackingService)
        {
            objectTrackingService.NewObject -= _newObjectAction;
            objectTrackingService.ObjectMoved -= _objectMovedAction;
            objectTrackingService.ObjectRemoved -= _objectRemovedAction;
        }

        #endregion Manage Event Handlers

        #region Register/Unregister

        public virtual void Register(IFrameSource frameSource)
        {
            RegisteredFrameSourcesInternal.Add(frameSource);
            RegisterFrameSourceEventHandlers(frameSource);
        }

        public virtual void Unregister(IFrameSource frameSource)
        {
            if (RegisteredFrameSourcesInternal.Contains(frameSource))
            {
                UnregisterFrameSourceEventHandlers(frameSource);
                RegisteredFrameSourcesInternal.Remove(frameSource);
            }
        }

        public virtual void Register(IObjectDetector objectDetector)
        {
            RegisteredObjectDetectorsInternal.Add(objectDetector);
            RegisterObjectDetectorEventHandlers(objectDetector);
        }

        public virtual void Unregister(IObjectDetector objectDetector)
        {
            if (RegisteredObjectDetectorsInternal.Contains(objectDetector))
            {
                UnregisterObjectDetectorEventHandlers(objectDetector);
                RegisteredObjectDetectorsInternal.Remove(objectDetector);
            }
        }

        #endregion
    }
}