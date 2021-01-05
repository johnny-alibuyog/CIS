using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Diagnostics;
using System.Timers;
using System.Windows;
using Multitouch.Contracts;
using Touchless.Multitouch.Configuration;
using Touchless.Multitouch.Devices;
using Touchless.Shared.Extensions;
using Touchless.Vision.Contracts;
using Touchless.Vision.Service;

namespace Touchless.Multitouch
{
    [AddIn("Touchless", Publisher = "Kevin Rohling", Description = "Provides input from the Touchless SDK", Version = Version)]
    [Export(typeof(IProvider))]
    public class TouchlessInputProvider : IProvider
    {
        private const string Version = "0.1.0.0";
        private readonly Queue<Contact> _contactsQueue;
        private readonly Action<IObjectDetector, DetectedObject, Frame> _newObjectAction;
        private readonly Action<IObjectDetector, DetectedObject, Frame> _objectMovedAction;
        private readonly Action<IObjectDetector, DetectedObject, Frame> _objectRemovedAction;
        private readonly Timer _timer;
        private readonly ComposableTouchlessService _touchlessService;
        private readonly PointerManager _deviceManager;

        public TouchlessInputProvider(ComposableTouchlessService touchlessService)
        {
            _touchlessService = touchlessService;
            _newObjectAction = NewObject;
            _objectMovedAction = ObjectMoved;
            _objectRemovedAction = ObjectRemoved;

            _deviceManager = new PointerManager();
            _contactsQueue = new Queue<Contact>();
            _timer = new Timer(1000 / 60d);
            _timer.Elapsed += TimerElapsed;
        }

        public TouchlessInputProvider()
            : this(ComposableTouchlessService.GetComposedInstance())
        {
        }

        #region IProvider Members

        public event EventHandler<NewFrameEventArgs> NewFrame;

        public UIElement GetConfiguration()
        {
            var configurationElement = new TouchlessServiceConfiguration(_touchlessService);
            configurationElement.chkBlockMouse.IsChecked = _deviceManager.BlockMouse;
            configurationElement.chkBlockMouse.Checked += (s, e) => _deviceManager.BlockMouse = true;
            configurationElement.chkBlockMouse.Unchecked += (s, e) => _deviceManager.BlockMouse = false;
            configurationElement.Closed += (s, e) => _deviceManager.BlockMouse = false;
            return configurationElement;
        }

        public bool HasConfiguration
        {
            get { return true; }
        }

        public bool IsRunning { get; private set; }
        public bool SendEmptyFrames { get; set; }

        public bool SendImageType(ImageType imageType, bool value)
        {
            return false;
        }

        public void Start()
        {
            Initialize();
        }

        public void Stop()
        {
            Uninitialize();
        }

        #endregion

        private void Initialize()
        {
            if (!IsRunning)
            {
                _touchlessService.ObjectMoved += _objectMovedAction;
                _touchlessService.NewObject += _newObjectAction;
                _touchlessService.ObjectRemoved += _objectRemovedAction;
                _touchlessService.StartFrameProcessing();
                _timer.Start();
                IsRunning = true;
            }
        }

        private void Uninitialize()
        {
            if (IsRunning)
            {
                _touchlessService.ObjectMoved -= _objectMovedAction;
                _touchlessService.NewObject -= _newObjectAction;
                _touchlessService.ObjectRemoved -= _objectRemovedAction;
                _touchlessService.StopFrameProcessing();
                _timer.Stop();
                IsRunning = false;
            }
        }

        private void ObjectRemoved(IObjectDetector objectDetector, DetectedObject detectedObject, Frame frame)
        {
            var contact = EnqueueContact(detectedObject, frame, ContactState.Removed);
            _deviceManager.RemovePointer(contact.Id);
        }

        private void NewObject(IObjectDetector objectDetector, DetectedObject detectedObject, Frame frame)
        {
            var contact = EnqueueContact(detectedObject, frame, ContactState.New);
            _deviceManager.UpdatePointerPosition(contact.Id, contact.Position.ToDrawingPoint());
        }

        private void ObjectMoved(IObjectDetector objectDetector, DetectedObject detectedObject, Frame frame)
        {
            var contact = EnqueueContact(detectedObject, frame, ContactState.Moved);
            _deviceManager.UpdatePointerPosition(contact.Id, contact.Position.ToDrawingPoint());
        }

        private void TimerElapsed(object sender, ElapsedEventArgs e)
        {
            lock (_contactsQueue)
            {
                if (SendEmptyFrames || _contactsQueue.Count > 0)
                {
                    if (NewFrame != null)
                        NewFrame(this, new NewFrameEventArgs(Stopwatch.GetTimestamp(), _contactsQueue, null));
                    _contactsQueue.Clear();
                }
            }
        }

        private Contact EnqueueContact(DetectedObject detectedObject, Frame frame, ContactState contactState)
        {
            // TODO: InvalidOperationException - Object is currently in use elsewhere. (frame, I think)
            var frameSize = new Size(frame.OriginalImage.Width, frame.OriginalImage.Height);
            var contact = new TouchlessContact(detectedObject.Id, contactState, detectedObject.Position.ToWindowsPoint(), frameSize, 10, 10);
            EnqueueContact(contact);

            return contact;
        }

        private void EnqueueContact(TouchlessContact contact)
        {
            lock (_contactsQueue)
            {
                _contactsQueue.Enqueue((TouchlessContact)contact.Clone());
            }
        }
    }
}