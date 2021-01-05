using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using NUnit.Framework;
using TouchlessLib;
using System.Threading;

namespace TouchlessUnitTests
{
    [TestFixture]
    public class MarkerTests
    {
        [SetUp]
        public void Init()
        {
            _touch = new TouchlessMgr();

            // Get the first camera and start it
            Assert.IsNull(_touch.CurrentCamera);
            IEnumerator<Camera> cams = _touch.Cameras.GetEnumerator();
            cams.MoveNext();
            Camera c = cams.Current;
            Assert.IsNotNull(c);
            _touch.CurrentCamera = c;
            Assert.IsNotNull(_touch.CurrentCamera);

            // Attach a marker based on my cam and green marker
            Bitmap b = new Bitmap(@"cam.bmp");
            Assert.AreEqual(0, _touch.MarkerCount);
            _m = _touch.AddMarker("foo", b, new Point(114, 80), 5.0f);
            Assert.IsNotNull(_m);
            Assert.AreEqual(1, _touch.MarkerCount);
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
        public void TestMarkerProperties()
        {
            Assert.AreEqual("foo", _m.Name);
            Assert.IsNotNull(_m.PreviousData);
            ValidateEventData(_m.CurrentData, false);

            Assert.AreEqual(true, _m.Highlight);
            _m.Highlight = false;
            Assert.AreEqual(false, _m.Highlight);

            Assert.AreEqual(false, _m.ProvideCalculatedProperties);
            _m.ProvideCalculatedProperties = true;
            Assert.AreEqual(true, _m.ProvideCalculatedProperties);

            Assert.AreEqual(true, _m.SmoothingEnabled);
            _m.SmoothingEnabled = false;
            Assert.AreEqual(false, _m.SmoothingEnabled);

            _m.Threshold = 49;
            Assert.AreEqual(49, _m.Threshold);
        }


        void ValidateEventData(MarkerEventData ed, bool shouldBePresent)
        {
            Assert.IsNotNull(ed);

            if (shouldBePresent)
            {
                Assert.IsTrue(ed.Present);
            }

            // TODO: See if we can add more validation here
        }

        private TouchlessMgr _touch = null;
        private Marker _m = null;
        private ManualResetEvent _waitEvent = new ManualResetEvent(false);
    }
}
