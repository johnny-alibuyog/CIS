//*****************************************************************************************
//  File:       DrawDemo.cs
//  Project:    TouchlessDemo
//  Author(s):  Michael Wasserman (Michael.Wasserman@microsoft.com)
//
//  Demo to draw using n > 0 markers, using the size for the brush size.
//*****************************************************************************************

using System;
using System.Drawing;
using System.Windows.Forms;
using TouchlessLib;

namespace TouchlessDemo
{
    public class DrawDemo : IDemoInterface
    {
        public override string ToString() { return "Draw Demo"; }

        #region IDemoInterface

        string IDemoInterface.GetDemoDescription()
        {
            return
@"Drawing Demo Instructions:

Use one or more markers to draw on a canvas.
Change a marker's visible size to change its drawing width:
>  Bring a marker closer to or farther from the camera.
>  Hide or expose parts of a marker.
>  Hide the entire marker to prevent it from drawing:
>  With a marker on your finger, 'click' to hide.
>  Use a marker on your thumb and hide it with your fingers.

Can you extend this demo to make a small version of paint?
Can you think of better ways to 'click'?

Give feedback, submit code, join the community, and more:
http://www.codeplex.com/touchless";
        }

        bool IDemoInterface.StartDemo(TouchlessMgr tlmgr, Rectangle displayBounds)
        {
            if (tlmgr.MarkerCount < 1)
            {
                MessageBox.Show("Draw Demo requires a marker.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            _tlmgr = tlmgr;

            // Initialize the display bounds
            _displayWidth = displayBounds.Width;
            _displayHeight = displayBounds.Height;

            // Initialize the canvas for drawing and its graphics object
            _canvas = new Bitmap(_tlmgr.CurrentCamera.CaptureWidth, _tlmgr.CurrentCamera.CaptureHeight);
            lock (_canvas)
            {
                _canvasGFX = Graphics.FromImage(_canvas);
                _canvasGFX.FillRectangle(new SolidBrush(Color.FromArgb(64, 255, 255, 255)), 0, 0, _canvas.Width, _canvas.Height);
            }

            // Initialize the points and pen used for drawing segments
            _pen = new Pen(Color.Black);
            _p1 = new Point();
            _p2 = new Point();

            // Add marker update handling
            foreach (Marker marker in tlmgr.Markers)
                marker.OnChange += new EventHandler<MarkerEventArgs>(UpdateMarker);

            return true;
        }

        void IDemoInterface.StopDemo()
        {
            if (_tlmgr == null || _tlmgr.Markers == null)
                return;

            // Remove marker update handling
            foreach (Marker marker in _tlmgr.Markers)
                marker.OnChange -= new EventHandler<MarkerEventArgs>(UpdateMarker);

            _tlmgr = null;
        }

        void IDemoInterface.DrawDemoCanvas(Graphics gfx)
        {
            if (_tlmgr == null)
                return;

            // Draw our canvas with all the segments
            lock (_canvas)
            {
                gfx.DrawImage(_canvas, 0, 0, _displayWidth, _displayHeight);
            }
        }

        #endregion IDemoInterface

        private void UpdateMarker(object sender, MarkerEventArgs args)
        {
            // Do not draw if the marker's wasn't previously found
            if (!args.EventData.Present || !args.EventMarker.PreviousData.Present)
                return;

            // Draw a segment on our canvas between the marker and where it was previously found
            MarkerEventData data = args.EventData;
            Color c = args.EventMarker.RepresentativeColor;
            _pen.Color = Color.FromArgb(128, c.R, c.G, c.B);
            _pen.Width = (int)(data.Area / 60);
            _p1.X = (int)(data.X - data.DX);
            _p1.Y = (int)(data.Y - data.DY);
            _p2.X = (int)data.X;
            _p2.Y = (int)data.Y;

            lock (_canvas)
            {
                _canvasGFX.DrawLine(_pen, _p1, _p2);
            }
        }

        private TouchlessMgr _tlmgr;
        private int _displayWidth, _displayHeight;
        private Bitmap _canvas;
        private Graphics _canvasGFX;
        private Point _p1, _p2;
        private Pen _pen;
    }
}
