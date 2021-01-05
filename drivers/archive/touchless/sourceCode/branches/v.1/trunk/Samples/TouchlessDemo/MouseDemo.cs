//*****************************************************************************************
//  File:       MouseDemo.cs
//  Project:    TouchlessDemo
//  Author(s):  Michael Wasserman (Michael.Wasserman@microsoft.com)
//              Predrag Tomasevic (pele@beotel.net)
//              eFloh
//
//  Demo to control mouse movement and button state using markers
//  TODO: Have a buffer time on the mouse up, to ignore noise
//*****************************************************************************************

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using TouchlessLib;

namespace TouchlessDemo
{
    class MouseDemo : IDemoInterface
    {
        public override string ToString() { return "Mouse Demo"; }

        #region IDemoInterface

        string IDemoInterface.GetDemoDescription()
        {
            return
@"Mouse Demo Instructions:

Use one-three markers to control your mouse:
>  First marker will control position of the mouse.
>  Touch the second marker to the first to left-click.
>  Touch the third marker to the first to right-click.

Can you stabilize the cursor for poorly tracked markers?
Can you implement a better way to click?
Can you improve the mouse control experience in another way?

Give feedback, submit code, join the community, and more:
http://www.codeplex.com/touchless";
        }

        bool IDemoInterface.StartDemo(TouchlessMgr tlmgr, Rectangle displayBounds)
        {
            if (tlmgr.MarkerCount < 1)
            {
                MessageBox.Show("Mouse Demo requires one-three markers.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            _tlmgr = tlmgr;

            // Setup the translation from the capture area to the screen area
            _xRatio = (double)Screen.PrimaryScreen.Bounds.Width / (_captureAreaPercent * _tlmgr.CurrentCamera.CaptureWidth);
            _yRatio = (double)Screen.PrimaryScreen.Bounds.Height / (_captureAreaPercent * _tlmgr.CurrentCamera.CaptureHeight);
            _xBuffer = 0.5 * (1 - _captureAreaPercent) * _tlmgr.CurrentCamera.CaptureWidth;
            _yBuffer = 0.5 * (1 - _captureAreaPercent) * _tlmgr.CurrentCamera.CaptureHeight;

            // Setup the mouse down information arrays (MouseLeft=0, MouseRight=1)
            _markerMouseButton = new Marker[] {null, null};
            _mouseLastEventTime = new DateTime[] {DateTime.MinValue, DateTime.MinValue};
            _mouseStartTime = new DateTime[] {DateTime.MinValue, DateTime.MinValue};
            _fMouseStartDown = new bool[] {false, false};
            _fMouseActuallyDown = new bool[] {false, false};
            _MouseDownFlag = new MouseEventDWFlags[] { MouseEventDWFlags.MOUSEEVENTF_LEFTDOWN, MouseEventDWFlags.MOUSEEVENTF_RIGHTDOWN};
            _MouseUpFlag = new MouseEventDWFlags[] { MouseEventDWFlags.MOUSEEVENTF_LEFTUP, MouseEventDWFlags.MOUSEEVENTF_RIGHTUP};

            // Setup the markers for movement, left-clicking, and right-clicking
            _markerMouseCursor = _tlmgr.Markers[0];
            _markerMouseCursor.OnChange += new EventHandler<MarkerEventArgs>(UpdateMarker);
            if (_tlmgr.MarkerCount >= 2)
            {
                _markerMouseButton[0] = _tlmgr.Markers[1];
                _markerMouseButton[0].OnChange += new EventHandler<MarkerEventArgs>(UpdateMarker);
            }
            if (_tlmgr.MarkerCount >= 3)
            {
                _markerMouseButton[1] = _tlmgr.Markers[2];
                _markerMouseButton[1].OnChange += new EventHandler<MarkerEventArgs>(UpdateMarker);
            }

            return true;
        }

        void IDemoInterface.StopDemo()
        {
            if (_markerMouseCursor != null)
                _markerMouseCursor.OnChange -= new EventHandler<MarkerEventArgs>(UpdateMarker);
            if (_markerMouseButton != null)
                foreach (Marker marker in _markerMouseButton)
                    if (marker != null)
                        marker.OnChange -= new EventHandler<MarkerEventArgs>(UpdateMarker);

            _tlmgr = null;
        }

        void IDemoInterface.DrawDemoCanvas(Graphics gfx)
        {
            // Do nothing
        }

        #endregion IDemoInterface

        private void UpdateMarker(object sender, MarkerEventArgs args)
        {
            // Move the cursor, or emulate left or right mouse button
            if (args.EventMarker == _markerMouseCursor)
                SimulateMouseCursor();
            else if (args.EventMarker == _markerMouseButton[0])
                SimulateMouseButton(0 /*mouseButtonIndex*/);
            else if (args.EventMarker == _markerMouseButton[1])
                SimulateMouseButton(1 /*mouseButtonIndex*/);
        }

        private void SimulateMouseCursor()
        {
            if (!_markerMouseCursor.CurrentData.Present)
                return;

            // Translate the capture space marker point to screen space
            double x = (_markerMouseCursor.CurrentData.X - _xBuffer) / _captureAreaPercent;
            double y = (_markerMouseCursor.CurrentData.Y - _yBuffer) / _captureAreaPercent;
            x = (x < 0) ? 0 : Math.Min(x * _xRatio, Screen.PrimaryScreen.Bounds.Width);
            y = (y < 0) ? 0 : Math.Min(y * _yRatio, Screen.PrimaryScreen.Bounds.Height);
            // Set the cursor position
            Cursor.Position = new System.Drawing.Point((int)x, (int)y);
        }

        private void SimulateMouseButton(int mouseButtonIndex)
        {
            Marker markerMouseButton = _markerMouseButton[mouseButtonIndex];

            // Mouse is not down if we have an inactive or not-present marker
            bool fMouseDown = _markerMouseCursor.Active && _markerMouseCursor.CurrentData.Present &&
                markerMouseButton.Active && markerMouseButton.CurrentData.Present;

            if (fMouseDown)
            {
                // Get the button marker and cursor marker bounds (with small pixel buffer)
                int rectBuffer = 10;
                Rectangle cursorRect = new Rectangle((int)_markerMouseCursor.CurrentData.Left - rectBuffer,
                                                     (int)_markerMouseCursor.CurrentData.Top - rectBuffer,
                                                     (int)_markerMouseCursor.CurrentData.Width + 2 * rectBuffer,
                                                     (int)_markerMouseCursor.CurrentData.Height + 2 * rectBuffer);
                Rectangle buttonRect = new Rectangle((int)markerMouseButton.CurrentData.Left - rectBuffer,
                                                     (int)markerMouseButton.CurrentData.Top - rectBuffer,
                                                     (int)markerMouseButton.CurrentData.Width + 2 * rectBuffer,
                                                     (int)markerMouseButton.CurrentData.Height + 2 * rectBuffer);
                // Mouse down if the cursor and button marker bounds intersect
                fMouseDown = cursorRect.IntersectsWith(buttonRect);
            }

            // If the marker is in mouse down state
            if (fMouseDown)
            {
                // If this is the first time we see the markers in a mouse down state
                if (!_fMouseStartDown[mouseButtonIndex])
                {
                    // If enough time has passed since the last actual mouse event
                    if (_mouseLastEventTime[mouseButtonIndex] < DateTime.Now.AddMilliseconds(-_mouseBufferTime))
                    {
                        _fMouseStartDown[mouseButtonIndex] = true;
                        _mouseStartTime[mouseButtonIndex] = DateTime.Now;
                    }
                }
                // If the markers have been in mouse down state long enough to actually mouse down
                else if (!_fMouseActuallyDown[mouseButtonIndex] && _mouseStartTime[mouseButtonIndex] < DateTime.Now.AddMilliseconds(-_mouseDownTime))
                {
                    _fMouseActuallyDown[mouseButtonIndex] = true;
                    MouseEvent(mouseButtonIndex, _MouseDownFlag[mouseButtonIndex]);
                }
            }
            // The markers are in a mouse up state and have started mouse down
            else if (_fMouseStartDown[mouseButtonIndex])
            {
                // If the mouse down time has been exceeded
                if (_mouseStartTime[mouseButtonIndex] < DateTime.Now.AddMilliseconds(-_mouseDownTime))
                {
                    // If the mouse down event was sent, send the mouse up event, otherwise click
                    if (_fMouseActuallyDown[mouseButtonIndex])
                        MouseEvent(mouseButtonIndex, _MouseUpFlag[mouseButtonIndex]);
                    else
                        MouseEvent(mouseButtonIndex, _MouseDownFlag[mouseButtonIndex] | _MouseUpFlag[mouseButtonIndex]);
                }
                // Else, If the buffer time has been exceeded, but the down time has not, click
                else if (_mouseStartTime[mouseButtonIndex] < DateTime.Now.AddMilliseconds(-_mouseDownTime))
                    MouseEvent(mouseButtonIndex, _MouseDownFlag[mouseButtonIndex] | _MouseUpFlag[mouseButtonIndex]);

                // Clear the mouse down info
                _fMouseStartDown[mouseButtonIndex] = false;
                _fMouseActuallyDown[mouseButtonIndex] = false;
            }
        }

        #region Mouse Event API

        private enum MouseEventDWFlags
        {
            MOUSEEVENTF_MOVE = 0x01,
            MOUSEEVENTF_LEFTDOWN = 0x02,
            MOUSEEVENTF_LEFTUP = 0x04,
            MOUSEEVENTF_RIGHTDOWN = 0x08,
            MOUSEEVENTF_RIGHTUP = 0x10,
            MOUSEEVENTF_MIDDLEDOWN = 0x20,
            MOUSEEVENTF_MIDDLEUP = 0x40
        }

        // Interface to send mouse events
        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern void mouse_event(
            UInt32 dwFlags, // motion and click options 
            UInt32 dx, // horizontal position or change 
            UInt32 dy, // vertical position or change 
            UInt32 dwData, // wheel movement 
            IntPtr dwExtraInfo // application-defined information 
        );

        private void MouseEvent(int mouseButtonIndex, MouseEventDWFlags dwFlags)
        {
            _mouseLastEventTime[mouseButtonIndex] = DateTime.Now;
            mouse_event((UInt32)dwFlags, 0, 0, 0, new System.IntPtr());
        }

        #endregion Mouse Event API

        private TouchlessMgr _tlmgr;

        // Down then up within buffer time is ignored
        private const int _mouseBufferTime = 40;
        // Down then up within the down time is a click, otherwise mouse down
        private const int _mouseDownTime = 80;

        // Values used in capture-space to screen-space translation
        private const double _captureAreaPercent = 0.9;
        private double _xRatio, _yRatio;
        private double _xBuffer, _yBuffer;

        // Mouse state information and arrays (MouseLeft=0, MouseRight=1)
        private Marker _markerMouseCursor;
        private Marker[] _markerMouseButton;
        private DateTime[] _mouseStartTime;
        private DateTime[] _mouseLastEventTime;
        private bool[] _fMouseStartDown;
        private bool[] _fMouseActuallyDown;
        private MouseEventDWFlags[] _MouseDownFlag;
        private MouseEventDWFlags[] _MouseUpFlag;
    }
}