//*****************************************************************************************
//  File:       Marker.cs
//  Project:    TouchlessLib
//  Author(s):  Michael Wasserman (Michael.Wasserman@microsoft.com)
//              Gary Caldwell (gacald@microsoft.com)
//
//  Classes to represent markers and their associated data.
//  NOTE: Scanline shape rasterization proposed by Natan Zohar.
//
//  TODO: Get higher degree moments of inertia (primary/secondary axes, etc.)
//  TODO: Extend alpha smoothing with exponential decay...
//  TODO: Optimize threshold, replace threshhold concept with partial matching?
//  TODO: Expose smoothing factor?
//*****************************************************************************************

using System;
using System.Collections.Generic;
using System.Drawing;

namespace TouchlessLib
{
    /// <summary>
    /// Represents a marker being tracked
    /// </summary>
    public class Marker
    {
        #region Public Interface

        /// <summary>
        /// Name of the marker provided when it was created
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Representative color value of the marker
        /// </summary>
        public Color RepresentativeColor { get; set; }

        /// <summary>
        /// Color frequency threshold used for detection
        /// </summary>
        public int Threshold { get; set; }

        /// <summary>
        /// Enable marker motion smoothing
        /// </summary>
        public bool SmoothingEnabled { get; set; }

        /// <summary>
        /// Determines if the marker will be highlighted in images
        /// </summary>
        public bool Highlight { get; set; }

        /// <summary>
        /// Determines if the marker should be tracked and raise events
        /// </summary>
        public bool Active { get; set; }

        /// <summary>
        /// Returns the current status of the marker, useful for polling operations
        /// </summary>
        /// <returns>An instance of the MarkerEventData structure that represents the marker's current state</returns>
        public MarkerEventData CurrentData
        {
            get { return currData; }
            internal set { currData = value; }
        }

        /// <summary>
        /// Returns the previous status of the marker
        /// </summary>
        /// <returns>An instance of the MarkerEventData structure that represents the marker's previous state</returns>
        public MarkerEventData PreviousData
        {
            get { return prevData; }
            internal set { prevData = value; }
        }

        /// <summary>
        /// Returns the last known status of the marker
        /// </summary>
        /// <returns>An instance of the MarkerEventData structure that represents the marker's last state wherein it was present</returns>
        public MarkerEventData LastGoodData
        {
            get { return lastGoodData; }
            internal set { lastGoodData = value; }
        }

        /// <summary>
        /// Event fired when a marker's MarkerEventData is updated
        /// </summary>
        /// <example>
        /// The following is a code snippet that shows how to setup the <c>OnChange</c> event handler
        /// <code>
        ///     TouchlessMgr _touch = new TouchlessMgr();
        ///     ...
        ///
        ///     // Create a new marker using the current image at a fixed location and size
        ///     Marker m = _touch.AddMarker("marker", _touch.CurrentCamera.GetCurrentImage(), new Point(100, 100), 10);
        ///     m.OnChange += new EventHandler&lt;MarkerEventArgs&gt;(Marker_OnChange);
        ///
        ///     ...
        ///     void Marker_OnChange(object sender, MarkerEventArgs args)
        ///     {
        ///         if (args.EventData.IsPresent)
        ///         {
        ///             // Do something with args.EventData
        ///         }
        ///     }
        ///
        /// </code>
        /// </example>
        public event EventHandler<MarkerEventArgs> OnChange;

        /// <summary>
        /// ToString override that returns the name of the marker
        /// </summary>
        /// <returns>The name of the marker</returns>
        public override string ToString() { return Name; }

        #endregion Public Interface

        #region Internal Implementation

        // Color frequency dictionary (ColorKey key -> uint freq)
        internal SortedDictionary<ColorKey, uint> hsvFreq;
        internal MarkerEventData currData;
        internal MarkerEventData prevData;
        internal MarkerEventData lastGoodData;
        internal float smoothingFactor;
        internal byte framesToSkip;

        // Number of bins used for color partitioning
        internal HSV bins;

        // Scanline shape rasterization commands used to limit the marker search area in the image scan loop
        internal MarkerScanCommand searchMinX, searchMaxX, searchMinY, searchMaxY;

        // Intermediate values used for average color calculation
        internal long avgR, avgG, avgB;
        internal int representativeRGB;

        /// <summary>
        /// Set the appearance of the marker, given its color frequencies
        /// </summary>
        /// <param name="rawHsvFreq">A 3D array of HSV frequencies</param>
        /// <returns>Success</returns>
        internal bool SetMarkerAppearance(int[, ,] rawHsvFreq)
        {
            // Get the dimensions of the cube
            bins.H = (byte)rawHsvFreq.GetLength(0);
            bins.S = (byte)rawHsvFreq.GetLength(1);
            bins.V = (byte)rawHsvFreq.GetLength(2);

            // Reset hash
            hsvFreq = new SortedDictionary<ColorKey, uint>();

            // Initialization
            long freq = 0, thresh = 0, colors = 0;
            ColorKey key = new ColorKey();
            HSV hsv = new HSV();

            // Populate the hash
            for (hsv.H = 0; hsv.H < bins.H; hsv.H++)
                for (hsv.S = 0; hsv.S < bins.S; hsv.S++)
                    for (hsv.V = 0; hsv.V < bins.V; hsv.V++)
                        if ((freq = rawHsvFreq[hsv.H, hsv.S, hsv.V]) > 0)
                        {
                            key.Hsv = hsv;
                            hsvFreq.Add(key, (uint)freq);
                            thresh += freq;
                            colors++;
                        }

            // Return failure if no marker colors were found
            if (colors == 0)
                return false;

            // Set the threshold to 2*(the average color frequency)
            Threshold = (int)(2 * thresh / colors);

            return true;
        }

        /// <summary>
        /// Fire an event to notify handlers that the marker data was updated
        /// </summary>
        internal void FireOnChangeEvent()
        {
            if(OnChange != null)
                OnChange(this, new MarkerEventArgs(currData, this));
        }

        internal Marker(string name)
        {
            Name = name;
            Highlight = true;
            SmoothingEnabled = true;
            smoothingFactor = 0.55F;
            Active = true;
            framesToSkip = 0;

            // Initialize the scanline search bounds
            searchMinX = new MarkerScanCommand(this, ScanCommand.addMarker, 0);
            searchMaxX = new MarkerScanCommand(this, ScanCommand.remMarker, 0);
            searchMinY = new MarkerScanCommand(this, ScanCommand.addMarker, 0);
            searchMaxY = new MarkerScanCommand(this, ScanCommand.remMarker, 0);
        }

        #endregion
    }

    /// <summary>
    /// Defines data associated with a Marker
    /// </summary>
    public struct MarkerEventData
    {
        /// <summary>
        /// If the marker is currently present
        /// </summary>
        public bool Present { get { return (this.Area > 0); } }

        /// <summary>
        /// Current X position of the marker
        /// </summary>
        public double X { get; internal set; }

        /// <summary>
        /// Current Y position of the marker
        /// </summary>
        public double Y { get; internal set; }

        /// <summary>
        /// Relative distance in the X axis from the last processed location
        /// </summary>
        public double DX { get; internal set; }

        /// <summary>
        /// Relative distance in the Y axis from the last processed location
        /// </summary>
        public double DY { get; internal set; }

        /// <summary>
        /// The 2D volume of the marker
        /// </summary>
        public double Area { get; internal set; }

        /// <summary>
        /// The top of the bounding box of the marker area
        /// </summary>
        public double Top { get; internal set; }

        /// <summary>
        /// The bottom of the bounding box of the marker area
        /// </summary>
        public double Bottom { get; internal set; }

        /// <summary>
        /// The left of the bounding box of the marker area
        /// </summary>
        public double Left { get; internal set; }

        /// <summary>
        /// The right of the bounding box of the marker area
        /// </summary>
        public double Right { get; internal set; }

        /// <summary>
        /// The height of the bounding box of the marker area
        /// </summary>
        public double Height { get { return Right - Left; } }

        /// <summary>
        /// The width of the bounding box of the marker area
        /// </summary>
        public double Width { get { return Bottom - Top; } }

        /// <summary>
        /// A DateTime that reflects when this data was collected
        /// </summary>
        public DateTime Timestamp { get; internal set; }
    }

    /// <summary>
    /// Marker specific EventArgs that provides the MarkerEventData
    /// </summary>
    public class MarkerEventArgs : EventArgs
    {
        /// <summary>
        /// Get the Marker's updated state information
        /// </summary>
        public MarkerEventData EventData { get; private set; }

        /// <summary>
        /// Get the updated Marker
        /// </summary>
        public Marker EventMarker { get; private set; }

        /// <summary>
        /// A constructor that takes the event data as an argument
        /// </summary>
        /// <param name="med">The marker event data for this event</param>
        /// <param name="m">The marker updated with this event</param>
        internal MarkerEventArgs(MarkerEventData med, Marker m) { EventData = med; EventMarker = m; }
    }

    #region Scanline Rasterization
    /// <summary>
    /// An enumeration used to distinguish between add/rem commands during the image scan loop
    /// </summary>
    internal enum ScanCommand { addMarker = 0, remMarker = 1 }

    /// <summary>
    /// Represents a command to add or remove a marker from consideration in part of the image scan loop
    /// </summary>
    internal struct MarkerScanCommand
    {
        internal MarkerScanCommand(Marker m, ScanCommand c, int i) { marker = m; command = c; coordinate = i; }

        // The marker associated with this command
        internal Marker marker;
        // The instrcution to add/rem the marker for entering/exiting the search region
        internal ScanCommand command;
        // The coordinate of the command
        internal int coordinate;
    }
    #endregion Scanline Rasterization
}
