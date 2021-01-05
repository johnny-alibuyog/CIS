using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Multitouch.Contracts;
using System.Windows;

namespace Touchless.Multitouch
{
    public class TouchlessContact : Contact, ICloneable
    {
        public Point FramePosition { get; private set; }
        public Size FrameSize { get; private set; }


        public TouchlessContact(TouchlessContact touchlessContact)
            : this(touchlessContact.Id, touchlessContact.State, touchlessContact.FramePosition, touchlessContact.FrameSize, touchlessContact.Position, touchlessContact.MajorAxis, touchlessContact.MinorAxis)
        {
            
        }

        public TouchlessContact(int id, ContactState contactState, Point framePosition, Size frameSize, double majorAxis, double minorAxis)
            : this(id, contactState, framePosition, frameSize, TransformImagePositionToScreenPosition(framePosition, frameSize), majorAxis, minorAxis)
        {
            
        }

        private TouchlessContact(int id, ContactState contactState, Point framePosition, Size frameSize, Point screenPosition, double majorAxis, double minorAxis)
            : base(id, contactState, screenPosition, majorAxis, minorAxis)
        {
            FramePosition = framePosition;
            FrameSize = frameSize;
        }

        

        public object Clone()
        {
            return new TouchlessContact(this);
        }

        // Transforms the detected object position in image space to a point in screen space.
        // ***Changed to capture X before Y because the screen height is dependent upon which
        // ***screen this point is on.  Use the X value to figure out the appropriate screen.
        private static Point TransformImagePositionToScreenPosition(Point point, Size imageSize)
        {
            // Use a center sub-rectangle of the captured area (the edges are hard to reach)
            double captureSubRegion = 0.9;

            // Get the primary screen width and height
            int screenWidth = System.Windows.Forms.Screen.AllScreens.Sum(i => i.Bounds.Width);
            double xRatio = screenWidth / (captureSubRegion * imageSize.Width);
            double xBuffer = 0.5 * (1 - captureSubRegion) * imageSize.Width;
            double x = (point.X - xBuffer) / captureSubRegion;
            x = (x < 0) ? 0 : Math.Min(x * xRatio, screenWidth);


            int screenHeight = System.Windows.Forms.Screen.FromPoint(new System.Drawing.Point((int)x, 0)).Bounds.Height;

            // Setup the translation from the capture area to the screen area
            double yRatio = screenHeight / (captureSubRegion * imageSize.Height);
            double yBuffer = 0.5 * (1 - captureSubRegion) * imageSize.Height;
            double y = (point.Y - yBuffer) / captureSubRegion;
            y = (y < 0) ? 0 : Math.Min(y * yRatio, screenHeight);

            // Return the screen point
            return new Point(x, y);
        }
    }
}
