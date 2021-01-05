using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using TouchlessLib;
using System.Drawing;
using System.Threading;

namespace TouchlessUnitTests
{
    [TestFixture]
    public class CameraTests
    {
        [SetUp]
        public void Init()
        {
            _touch = new TouchlessMgr();

            
        }

        [TearDown]
        public void Cleanup()
        {
            if (_touch != null)
            {
                _touch.Dispose();
                _touch = null;
            }
        }

        [Test]
        public void TestGetCurrentImage()
        {
            _touch.CurrentCamera = GetFirstCamera();

            Camera c = _touch.CurrentCamera;
            Assert.IsNotNull(c);

            // Wait a few seconds to ensure the image is captured
            Thread.Sleep(2000);

            Bitmap b = c.GetCurrentImage();
            Assert.IsNotNull(b);

            Assert.AreEqual(b.Height, c.CaptureHeight);
            Assert.AreEqual(b.Width, c.CaptureWidth);
        }

        [Test]
        public void TestGetCurrentImageAndChangeSize()
        {
            _touch.CurrentCamera = GetFirstCamera();

            Camera c = _touch.CurrentCamera;
            Assert.IsNotNull(c);

            c.CaptureHeight = 100;
            c.CaptureWidth = 100;

            // Wait a few seconds to ensure the image is captured
            Thread.Sleep(2000);

            Bitmap b = c.GetCurrentImage();
            Assert.IsNotNull(b);

            Assert.AreEqual(100, c.CaptureHeight);
            Assert.AreEqual(100, c.CaptureWidth);
            Assert.AreEqual(b.Height, c.CaptureHeight);
            Assert.AreEqual(b.Width, c.CaptureWidth);
        }

        [Test]
        public void TestCaptureEvent()
        {
            Camera c = GetFirstCamera();
            Assert.IsNotNull(c);

            _waitEvent.Reset();
            _captureCount = 0;

            c.OnImageCaptured += new EventHandler<CameraEventArgs>(OnImageCaptured);

            // Start it
             _touch.CurrentCamera = c;

            // Wait at most 5 seconds to capture 5 events
            Assert.IsTrue(_waitEvent.WaitOne(5000, false));
        }

        void OnImageCaptured(object sender, CameraEventArgs e)
        {
            Assert.AreSame(sender, _touch.CurrentCamera);
            Assert.IsNotNull(e.Image);

            Assert.AreEqual(e.Image.Height, _touch.CurrentCamera.CaptureHeight);
            Assert.AreEqual(e.Image.Width, _touch.CurrentCamera.CaptureWidth);

            _captureCount++;

            if (_captureCount >= 5)
            {
                _waitEvent.Set();
            }
        }

        [Test]
        public void TestCameraProperties()
        {
            Camera c = GetFirstCamera();
            Assert.IsNotNull(c);

            Assert.AreEqual(-1, c.FpsLimit);
            c.FpsLimit = 15;
            Assert.AreEqual(15, c.FpsLimit);

            Assert.IsNotNull(c.ToString());
        }

        // Grab and start the first camera
        protected Camera GetFirstCamera()
        {
            Assert.IsNull(_touch.CurrentCamera);
            IEnumerator<Camera> cams = _touch.Cameras.GetEnumerator();
            cams.MoveNext();
            Camera c = cams.Current;
            Assert.IsNotNull(c);

            return c;
        }

        private TouchlessMgr _touch = null;
        private ManualResetEvent _waitEvent = new ManualResetEvent(false);
        private int _captureCount = 0;
    }
}
