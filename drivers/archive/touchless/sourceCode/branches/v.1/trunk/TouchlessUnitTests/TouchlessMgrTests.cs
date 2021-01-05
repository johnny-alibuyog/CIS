using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using TouchlessLib;
using System.Drawing;

namespace TouchlessUnitTests
{
    [TestFixture]
    public class TouchlessMgrTests
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
        public void TestCameraPresent()
        {
            int cnt = 0;

            foreach (Camera c in _touch.Cameras)
            {
                cnt++;
            }

            Assert.Greater(cnt, 0);
        }

        [Test]
        public void TestRefreshCameraList()
        {
            int cnt = 0;
            int cnt2 = 0;

            foreach (Camera c in _touch.Cameras)
            {
                cnt++;
            }

            _touch.RefreshCameraList();

            foreach (Camera c in _touch.Cameras)
            {
                cnt2++;
            }

            Assert.AreEqual(cnt, cnt2);
        }

        [Test]
        public void TestNoCameraSet()
        {
            Assert.IsNull(_touch.CurrentCamera);
        }


        [Test]
        public void TestIfCameraSet()
        {
            Assert.IsNull(_touch.CurrentCamera);
            IEnumerator<Camera> cams = _touch.Cameras.GetEnumerator();
            cams.MoveNext();
            Camera c = cams.Current;
            Assert.IsNotNull(c);
            _touch.CurrentCamera = c;
            Assert.IsNotNull(_touch.CurrentCamera);
        }

        [Test]
        public void TestMarkerManagement()
        {
            Bitmap b = new Bitmap(@"cam.bmp");
            Assert.AreEqual(0, _touch.MarkerCount);
            Marker m = _touch.AddMarker("foo", b, new Point(114, 80), 5.0f);
            Assert.IsNotNull(m);
            Assert.AreEqual(1, _touch.MarkerCount);
            Marker m2 = _touch.AddMarker("bar", b, new Point(189, 77), 5.0f);
            Assert.IsNotNull(m2);
            _touch.RemoveMarker(0);
            Assert.AreEqual(1, _touch.MarkerCount);
            Marker m3 = _touch.Markers[0];
            Assert.IsNotNull(m3);
            Assert.AreSame(m2, m3);
        }

        [Test]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void CheckGetMarkerArguments()
        {
            Assert.AreEqual(0, _touch.MarkerCount);
            Marker m = _touch.Markers[1];
        }

        [Test]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void CheckRemoveMarkerArguments()
        {
            Assert.AreEqual(0, _touch.MarkerCount);
            _touch.RemoveMarker(0);
        }

        private TouchlessMgr _touch = null;
    }
}
