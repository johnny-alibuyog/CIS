//*****************************************************************************************
//  File:       ImageDemo.cs
//  Project:    TouchlessDemo
//  Author(s):  Michael Wasserman (Michael.Wasserman@microsoft.com)
//
//  Demo to transform an image with the motion of one or two markers.
//*****************************************************************************************

using System;
using System.Drawing;
using System.Windows.Forms;
using TouchlessLib;

namespace TouchlessDemo
{
    public class ImageDemo : IDemoInterface
    {
        public override string ToString() { return "Image Demo"; }

        #region IDemoInterface

        string IDemoInterface.GetDemoDescription()
        {
            return
@"Image Demo Instructions:

Use two markers to control an image:
>  The first marker is the lower left corner of the image.
>  The second marker is the upper right corner of the image.
>  Move the markers around together or independently.

Use one marker to control an image:
>  Control zoom in the center area with the marker size.
>  Move the marker outside the center area to pan.

Can you make a more intuitive way to control an image?
Can you implment arbitrary two-point transformations?
Can you extend the controls to work with multiple images?

Give feedback, submit code, join the community, and more:
http://www.codeplex.com/touchless";
        }

        bool IDemoInterface.StartDemo(TouchlessMgr tlmgr, Rectangle displayBounds)
        {
            if (tlmgr.MarkerCount < 1)
            {
                MessageBox.Show("Image Demo requires one to two markers.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            _tlmgr = tlmgr;

            // Initialize the image (read from file)
            _image = new Bitmap("image.gif");
            _imageWidth = _image.Width;
            _imageHeight = _image.Height;

            // Initialize the bounds
            _captureWidth = tlmgr.CurrentCamera.CaptureWidth;
            _captureHeight = tlmgr.CurrentCamera.CaptureHeight;
            _displayWidth = displayBounds.Width;
            _displayHeight = displayBounds.Height;
            _displayScale = _displayWidth / _tlmgr.CurrentCamera.CaptureWidth;

            // Add marker update handling (one or two markers?)
            if (tlmgr.MarkerCount >= 2)
            {
                _oneMarker = false;

                tlmgr.Markers[0].OnChange += new EventHandler<MarkerEventArgs>(UpdatePrimaryMarker);
                tlmgr.Markers[1].OnChange += new EventHandler<MarkerEventArgs>(UpdateSecondaryMarker);

                // Initialize the points used for placing the image
                _destPoints = new Point[3];
                _destPoints[0] = new Point();
                _destPoints[1] = new Point();
                _destPoints[2] = new Point();

                // Calculate the image's diagonal length
                _imageDiagonal = (float)Math.Sqrt(_imageWidth * _imageWidth + _imageHeight * _imageHeight);
                // The angle from the lower-left corner to the upper right corner (from North)
                _imageDiagonalAngle = (float)Math.Atan2(_imageWidth, _imageHeight);
            }
            else // For a single marker
            {
                _oneMarker = true;

                tlmgr.Markers[0].OnChange += new EventHandler<MarkerEventArgs>(UpdateSingleMarker);
                _markerPos = new Point(0, 0);
                _currentCenter = new Point(_displayWidth / 2, _displayHeight / 2);
                _velocityCenter = new PointF(0, 0);
                _baselineArea = 0;
                _sampleFrameCount = 0;
                _currentScale = 1.0F;
            }

            return true;
        }

        void IDemoInterface.StopDemo()
        {
            if (_tlmgr == null || _tlmgr.Markers == null)
                return;

            // Remove marker update handling
            if (_tlmgr.MarkerCount >= 2)
            {
                _tlmgr.Markers[0].OnChange -= new EventHandler<MarkerEventArgs>(UpdatePrimaryMarker);
                _tlmgr.Markers[1].OnChange -= new EventHandler<MarkerEventArgs>(UpdateSecondaryMarker);
            }
            else if (_tlmgr.MarkerCount >= 1)
                _tlmgr.Markers[0].OnChange -= new EventHandler<MarkerEventArgs>(UpdateSingleMarker);

            _tlmgr = null;
        }

        void IDemoInterface.DrawDemoCanvas(Graphics gfx)
        {
            if (_tlmgr == null)
                return;

            if (_oneMarker)
            {
                // Draw the image for a single marker
                gfx.DrawImage(_image,
                    _currentCenter.X - _imageWidth * _currentScale / 2,
                    _currentCenter.Y - _imageHeight * _currentScale / 2,
                    _imageWidth * _currentScale, _imageHeight * _currentScale);
                // Draw the marker location
                gfx.DrawEllipse(new Pen(Brushes.Red, 4), _markerPos.X - 10, _markerPos.Y - 10, 20, 20);
                // Draw the null region
                int scaledNullRadius = (int)(_NullRadius * _displayScale);
                gfx.DrawRectangle(new Pen(Brushes.Red, 4), _displayWidth / 2 - scaledNullRadius, _displayHeight / 2 - scaledNullRadius, scaledNullRadius * 2, scaledNullRadius * 2);
            }
            else
            {
                // Draw our canvas with all the segments
                gfx.DrawImage(_image, _destPoints);
            }
        }

        #endregion IDemoInterface

        private void UpdateSingleMarker(object sender, MarkerEventArgs args)
        {
            if (!args.EventData.Present)
                return;

            // Calculate baseline area (running avg) for the first _MaxSampleFrameCount frames
            if (_sampleFrameCount < _MaxSampleFrameCount)
                _baselineArea = (_sampleFrameCount++ == 0) ? args.EventData.Area : (_baselineArea + args.EventData.Area) / 2;

            _markerPos.X = (int)(args.EventData.X * _displayScale);
            _markerPos.Y = (int)(args.EventData.Y * _displayScale);

            // Translate x & y coords to have an origin of the image center (opposite directions)
            int x = (int)(_captureWidth / 2 - args.EventData.X);
            int y = (int)(_captureHeight / 2 - args.EventData.Y);

            // In the null area, we can change the zoom
            // TODO: keep the displayed image center steady while zooming
            if ((Math.Abs(x) < _NullRadius) && (Math.Abs(y) < _NullRadius))
            {
                // Set the velocity center to zero
                _velocityCenter.X = 0;
                _velocityCenter.Y = 0;

                // Find new velocity scale by adding new acceleration value; then cap at min/max values
                _velocityScale += (float)((args.EventData.Area - _baselineArea) / (100F * _baselineArea));
                _velocityScale = (float)(Math.Max(Math.Min(_velocityScale, _MaxVelocityScale), -_MaxVelocityScale));

                // Cap the scale to reasonable values
                if ((_currentScale >= _MaxScale && _velocityScale > 0) ||
                    (_currentScale <= _MinScale && _velocityScale < 0))
                    _velocityScale = 0;

                // Apply the scale velocity
                _currentScale += _velocityScale;
            }
            else // Outside the null area, pan around
            {
                // Find the new velocities by adding new acceleration values or slowing (in per-axis null area)
                _velocityCenter.X = (Math.Abs(x) < _NullRadius) ? _velocityCenter.X * .9F : _velocityCenter.X + x / (float)_captureWidth;
                _velocityCenter.Y = (Math.Abs(y) < _NullRadius) ? _velocityCenter.Y * .9F : _velocityCenter.Y + y / (float)_captureHeight;

                // Find the new center
                _currentCenter.X += (int)_velocityCenter.X;
                _currentCenter.Y += (int)_velocityCenter.Y;

                // Make sure the image doesn't easily go off screen
                if (_currentCenter.X > (_imageWidth * _currentScale) / 2 && _velocityCenter.X > 0)
                    _velocityCenter.X *= .9F;
                if (_currentCenter.X < _displayWidth - (_imageWidth * _currentScale) / 2 && _velocityCenter.X < 0)
                    _velocityCenter.X *= .9F;
                if (_currentCenter.Y > (_imageHeight * _currentScale) / 2 && _velocityCenter.Y > 0)
                    _velocityCenter.Y *= .9F;
                if (_currentCenter.Y < _displayHeight - (_imageHeight * _currentScale) / 2 && _velocityCenter.Y < 0)
                    _velocityCenter.Y *= .9F;
            }
        }

        /// <summary>
        /// Find the image transformation given new marker info
        /// </summary>
        private void RecalculateTransformation()
        {
            // Make sure the other two points are valid
            if (_destPoints[2].IsEmpty || _destPoints[1].IsEmpty)
                return;

            // Make local copies of the other two points
            Point upperRight = _destPoints[1];
            Point lowerLeft = _destPoints[2];

            // Determine the image scale based on the distance between the points and the base diagonal
            int dx = upperRight.X - lowerLeft.X;
            int dy = lowerLeft.Y - upperRight.Y;
            float scaledDiagonal = (float)Math.Sqrt(dx * dx + dy * dy);
            _imageScale = scaledDiagonal / _imageDiagonal;

            // Find the scaled height
            float scaledHeight = _imageHeight * _imageScale;

            // Find the current diagonal angle (from East)
            float currDiagAngle = (float)Math.Atan2(dy, dx);

            // Find the current left edge angle (from West)
            float currLeftEdgeAngle = (float)Math.PI - (currDiagAngle + _imageDiagonalAngle);

            // Find the x difference from the lower-left to the upper-left
            float diffX = (float)(Math.Cos(currLeftEdgeAngle) * scaledHeight);
            float diffY = (float)(Math.Sin(currLeftEdgeAngle) * scaledHeight);

            // Find the upper-left point
            _destPoints[0].X = (int)(lowerLeft.X - diffX);
            _destPoints[0].Y = (int)(lowerLeft.Y - diffY);
        }

        private void UpdatePrimaryMarker(object sender, MarkerEventArgs args)
        {
            if (!args.EventData.Present)
                return;

            // Set the lower-left point
            _destPoints[2].X = (int)(args.EventData.X * _displayScale);
            _destPoints[2].Y = (int)(args.EventData.Y * _displayScale);

            // Recalculate the upper-left point
            RecalculateTransformation();
        }

        private void UpdateSecondaryMarker(object sender, MarkerEventArgs args)
        {
            if (!args.EventData.Present)
                return;

            // Set the upper-right point
            _destPoints[1].X = (int)(args.EventData.X * _displayScale);
            _destPoints[1].Y = (int)(args.EventData.Y * _displayScale);

            // Recalculate the upper-left point
            RecalculateTransformation();
        }

        private TouchlessMgr _tlmgr;
        private Bitmap _image;
        private int _captureWidth, _captureHeight;
        private int _displayWidth, _displayHeight;
        private float _displayScale;
        private bool _oneMarker;

        // For multiple markers (upper-left, upper-right, and lower-left corners)
        private Point[] _destPoints;
        private int _imageWidth, _imageHeight;
        private float _imageDiagonal, _imageScale;
        // The angle from the lower-left corner to the upper right corner (from North)
        private float _imageDiagonalAngle;

        // For a single marker
        private Point _markerPos;
        private Point _currentCenter;
        private PointF _velocityCenter;
        private const float _NullRadius = 50;
        private float _currentScale, _velocityScale;
        private const float _MinScale = .1F;
        private const float _MaxScale = 10F;
        private const float _MaxVelocityScale = .002F;
        private double _baselineArea;
        private int _sampleFrameCount;
        private const int _MaxSampleFrameCount = 30;
    }
}
