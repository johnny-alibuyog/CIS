//*****************************************************************************************
//  File:       Marker.cs
//  Project:    TouchlessDemo
//  Author(s):  Michael Wasserman (Michael.Wasserman@microsoft.com)
//              Gary Caldwell (gacald@microsoft.com)
//
//  Demo application to show the rudimentary functionality of the Touchless library.
//*****************************************************************************************

using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using TouchlessLib;

namespace TouchlessDemo
{
    public partial class TouchlessDemo : Form
    {
        #region Touchless Demo Management

        public TouchlessDemo()
        {
            InitializeComponent();
        }

        private void TouchlessDemo_Load(object sender, EventArgs e)
        {
            // Make a new TouchlessMgr for library interaction
            _touchlessMgr = new TouchlessMgr();

            // Initialize some members
            _uiUpdateTimer = new Stopwatch();
            _uiUpdateTimer.Start();
            _fAddingMarker = false;
            _fChangingMode = false;
            _markerSelected = null;
            _addedMarkerCount = 0;

            // Put the app in camera mode and select the first camera by default
            radioButtonCamera.Checked = true;
            foreach (Camera cam in _touchlessMgr.Cameras)
                comboBoxCameras.Items.Add(cam);

            if (comboBoxCameras.Items.Count > 0)
            {
                comboBoxCameras.SelectedIndex = 0;
            }
            else
            {
                MessageBox.Show("Oops, this requires a Webcam. Please connect a Webcam and try again.");
                Environment.Exit(0);
            }

            // Try going directly to the markers tab
            if (radioButtonMarkers.Enabled)
                radioButtonMarkers.Checked = true;

            // Add the demos
            comboBoxDemos.Items.Add(new DrawDemo());
            comboBoxDemos.Items.Add(new ImageDemo());
            comboBoxDemos.Items.Add(new SnakeDemo());
            comboBoxDemos.Items.Add(new DefendDemo());
            comboBoxDemos.Items.Add(new MouseDemo());
        }

        private void TouchlessDemo_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Dispose of the TouchlessMgr object
            if (_touchlessMgr != null)
            {
                _touchlessMgr.Dispose();
                _touchlessMgr = null;
            }
        }

        private void radioButtonMode_CheckedChanged(object sender, EventArgs e)
        {
            if (_fChangingMode)
                return;

            _fChangingMode = true;
            groupBoxCamera.Visible = radioButtonCamera.Checked;
            groupBoxMarkers.Visible = radioButtonMarkers.Checked;
            groupBoxDemo.Visible = radioButtonDemo.Checked;
            _fChangingMode = false;

            // Set the back and next buttons enabled state
            buttonBack.Enabled = !radioButtonCamera.Checked;
            buttonNext.Enabled = !radioButtonDemo.Checked
                && ((radioButtonCamera.Checked && radioButtonMarkers.Enabled)
                || (radioButtonMarkers.Checked && radioButtonDemo.Enabled));

            // Set or unset the picturebox mouse interaction handlers
            if (radioButtonMarkers.Checked)
            {
                pictureBoxDisplay.MouseDown += new MouseEventHandler(pictureBoxDisplay_MouseDown);
                pictureBoxDisplay.MouseMove += new MouseEventHandler(pictureBoxDisplay_MouseMove);
                pictureBoxDisplay.MouseUp += new MouseEventHandler(pictureBoxDisplay_MouseUp);
            }
            else
            {
                pictureBoxDisplay.MouseDown -= new MouseEventHandler(pictureBoxDisplay_MouseDown);
                pictureBoxDisplay.MouseMove -= new MouseEventHandler(pictureBoxDisplay_MouseMove);
                pictureBoxDisplay.MouseUp -= new MouseEventHandler(pictureBoxDisplay_MouseUp);
            }

            // Disable any demos running if we aren't on the demo tab (any more)
            if (!radioButtonDemo.Checked)
            {
                comboBoxDemos.Text = "Select A Demo";
                foreach (IDemoInterface demo in comboBoxDemos.Items)
                    demo.StopDemo();
                buttonDemoStartStop.Text = "Start Demo";
                labelDemoInstructions.Enabled = false;
                labelDemoInstructions.Text = "";
            }
        }

        private void buttonBack_Click(object sender, EventArgs e)
        {
            if (radioButtonMarkers.Checked)
                radioButtonCamera.Checked = true;
            else if (radioButtonDemo.Checked)
                radioButtonMarkers.Checked = true;
        }

        private void buttonNext_Click(object sender, EventArgs e)
        {
            if (radioButtonCamera.Checked)
                radioButtonMarkers.Checked = true;
            else if (radioButtonMarkers.Checked)
                radioButtonDemo.Checked = true;
        }

        private void buttonHelp_Click(object sender, EventArgs e)
        {
            if (radioButtonCamera.Checked)
            {
                string cameraHelp = "This demo only works as well as your camera setup, so:\n"
                    + "   Remember to adjust the camera focus for a clear image.\n"
                    + "   A well-lit indoor scene with no visual clutter is ideal.\n"
                    + "   Avoid having strong shadows or washed-out lighting.\n"
                    + "   Place the camera in a way that faces your gesturing.\n\n"
                    + "Camera placement suggestions:\n"
                    + "1. Sitting raised on your desk, facing your hands.\n"
                    + "      Good for mouse-like behavior (supports multiple markers!).\n"
                    + "2. Sitting on your desk, facing a larger area.\n"
                    + "      Good for waving your arms or using large markers.";
                MessageBox.Show(cameraHelp, "Camera Help", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else if (radioButtonMarkers.Checked)
            {
                string markerHelp = "This demo only works as well as your markers are setup, so:\n"
                    + "   An ideal marker is a brightly and solidly colored sphere.\n"
                    + "   An ideal marker is colored different from the background.\n"
                    + "   Think of something to hold, or stick on your hands or fingers.\n"
                    + "   Try using sticky tack, tape, or rings to stick your markers.\n\n"
                    + "Some marker suggestions (remember that bright colors are important!):\n"
                    + "   Tennis balls, racquetballs, handballs, bouncey balls.\n"
                    + "   Beads, candy pieces, pellets, thumbtacks.\n"
                    + "   Pens, pencils, highlighters, actual markers.\n"
                    + "   Rolled up colored paper, sticky notes, craft supplies, colored toys.\n\n"
                    + "To add a new marker:\n"
                    + "   Ensure the camera is set up for your gesturing area.\n"
                    + "   Ensure your marker is visible and located where you'll gesture.\n"
                    + "   Click 'Add A New Marker' and the camera image will pause.\n"
                    + "   Click and drag a circle in the image that fits the marker closely.\n"
                    + "   Drag from the center of the marker to its edge and then release.\n\n"
                    + "Newly added markers will be highlighted in the image by default.\n"
                    + "If you have trouble, edit or remove the marker and try again.\n"
                    + "Check out our camera and demo help too.";
                MessageBox.Show(markerHelp, "Marker Help", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else if (radioButtonDemo.Checked)
            {
                string demoHelp = "Play with the drawing, snake, image, and defend demos.\n\n"
                    + "Our project is open-source SDK with an active community.\n"
                    + "Contribute your own demos or applications to our community.\n"
                    + "Make a game, you're not limited to Windows Forms Apps.\n"
                    + "Figure out a neat way to click with a marker.\n"
                    + "Make a mouse or joypad emulator for existing games and apps.\n"
                    + "Small demos can take less than one hour to write.\n\n"
                    + "Visit our project homepage at http://www.codeplex.com/touchless\n"
                    + "You can also contact: Michael.Wasserman@microsoft.com";
                MessageBox.Show(demoHelp, "Demo Help", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void linkLabelHomepage_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            // Open the linkLabelHomepage link
            Process.Start(linkLabelHomepage.Text);
        }

        private TouchlessMgr _touchlessMgr;
        private static Stopwatch _uiUpdateTimer;
        private static Point _markerCenter;
        private static float _markerRadius;
        private static Marker _markerSelected;
        private static bool _fAddingMarker;
        private static int _addedMarkerCount;
        private static bool _fChangingMode;
        private static bool _fUpdatingMarkerUI;
        private static Image _latestFrame;
        private static bool _drawSelectionAdornment;
        private static IDemoInterface _demo;

        #endregion Touchless Demo Management

        #region Event Handling

        private void drawLatestImage(object sender, PaintEventArgs e)
        {
            if (_latestFrame != null)
            {
                // Draw the latest image from the active camera
                e.Graphics.DrawImage(_latestFrame, 0, 0, pictureBoxDisplay.Width, pictureBoxDisplay.Height);

                // Draw the selection adornment
                if (_drawSelectionAdornment)
                    e.Graphics.DrawEllipse(new Pen(Brushes.Red, 1), _markerCenter.X - _markerRadius,
                        _markerCenter.Y - _markerRadius, 2 * _markerRadius, 2 * _markerRadius);

                // Draw the demo graphics
                if (radioButtonDemo.Checked && _demo != null)
                    _demo.DrawDemoCanvas(e.Graphics);
            }
        }

        /// <summary>
        /// Event handler from the active camera
        /// </summary>
        public void OnImageCaptured(object sender, CameraEventArgs args)
        {
            // Update the FPS display once every second
            if ( _uiUpdateTimer.ElapsedMilliseconds >= 1000)
            {
                this.BeginInvoke(new Action<double>(UpdateFPSInUI), new object[] { args.CameraFps });
                _uiUpdateTimer.Reset();
                _uiUpdateTimer.Start();
            }

            // Save the latest image for drawing
            if (!_fAddingMarker)
            {
                // Cause display Update
                _latestFrame = args.Image;
                pictureBoxDisplay.Invalidate();
            }
        }

        // Thread safe FPS label Update for UI
        private void UpdateFPSInUI(double fps)
        {
            labelCameraFPSValue.Text = "" + Math.Round(fps, 2);
        }

        /// <summary>
        /// Event Handler from the selected marker in the Marker Mode
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        public void OnSelectedMarkerUpdate(object sender, MarkerEventArgs args)
        {
            this.BeginInvoke(new Action<MarkerEventData>(UpdateMarkerDataInUI), new object[] { args.EventData });
        }

        private void UpdateMarkerDataInUI(MarkerEventData data)
        {
            labelMarkerData.Text =
                  "Center X:  " + (int)data.X + "\n"
                + "Center Y:  " + (int)data.Y + "\n"
                + "DX:        " + (int)data.DX + "\n"
                + "DY:        " + (int)data.DY + "\n"
                + "Area:      " + (int)data.Area + "\n"
                + "Left:      " + (int)data.Left + "\n"
                + "Right:     " + (int)data.Right + "\n"
                + "Top:       " + (int)data.Top + "\n"
                + "Bottom:    " + (int)data.Bottom + "\n"
                + "Width:     " + (int)data.Width + "\n"
                + "Height:    " + (int)data.Height + "\n";
        }

        #endregion Event Handling

        #region Demo Mode

        private void comboBoxDemos_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_demo != null)
            {
                _demo.StopDemo();
                _demo = null;
                buttonDemoStartStop.Text = "Start Demo";
                labelDemoInstructions.Enabled = false;
                labelDemoInstructions.Text = "";
            }

            if (comboBoxDemos.SelectedIndex < 0)
            {
                buttonDemoStartStop.Enabled = false;
                return;
            }

            IDemoInterface demo = (IDemoInterface)comboBoxDemos.SelectedItem;
            buttonDemoStartStop.Enabled = demo != null;
            labelDemoInstructions.Enabled = demo != null;
            labelDemoInstructions.Text = demo.GetDemoDescription();
        }


        private void buttonDemoStartStop_Click(object sender, EventArgs e)
        {
            if (_demo == null)
            {
                _demo = (IDemoInterface)comboBoxDemos.SelectedItem;
                if (_demo != null && _demo.StartDemo(_touchlessMgr, pictureBoxDisplay.Bounds))
                    buttonDemoStartStop.Text = "Stop Demo";
                else
                    _demo = null;
            }
            else
            {
                _demo.StopDemo();
                _demo = null;
                buttonDemoStartStop.Text = "Start Demo";
            }
        }

        #endregion Demo Mode

        #region Marker Mode

        private void buttonMarkerAdd_Click(object sender, EventArgs e)
        {
            _fAddingMarker = !_fAddingMarker;
            buttonMarkerAdd.Text = _fAddingMarker ? "Cancel Adding Marker" : "Add A New Marker";
        }

        private void comboBoxMarkers_DropDown(object sender, EventArgs e)
        {
            // Refresh the marker dropdown list.
            comboBoxMarkers.Items.Clear();
            foreach (Marker marker in _touchlessMgr.Markers)
                comboBoxMarkers.Items.Add(marker);
        }

        private void comboBoxMarkers_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_markerSelected != null)
                _markerSelected.OnChange -= new EventHandler<MarkerEventArgs>(OnSelectedMarkerUpdate);

            if (comboBoxMarkers.SelectedIndex < 0)
            {
                comboBoxMarkers.Text = "Edit An Existing Marker";
                groupBoxMarkerControl.Enabled = false;
                groupBoxMarkerControl.Text = "No Marker Selected";
                return;
            }

            _markerSelected = (Marker)comboBoxMarkers.SelectedItem;
            _markerSelected.OnChange += new EventHandler<MarkerEventArgs>(OnSelectedMarkerUpdate);

            groupBoxMarkerControl.Text = _markerSelected.Name;
            groupBoxMarkerControl.Enabled = true;
            _fUpdatingMarkerUI = true;
            checkBoxMarkerHighlight.Checked = _markerSelected.Highlight;
            checkBoxMarkerSmoothing.Checked = _markerSelected.SmoothingEnabled;
            numericUpDownMarkerThresh.Value = _markerSelected.Threshold;
            _fUpdatingMarkerUI = false;
            buttonMarkerActive.Text = _markerSelected.Active ? "Deactivate" : "Activate";
        }

        #region UI Marker Editing

        private void checkBoxMarkerHighlight_CheckedChanged(object sender, EventArgs e)
        {
            if (_fUpdatingMarkerUI)
                return;

            ((Marker)comboBoxMarkers.SelectedItem).Highlight = checkBoxMarkerHighlight.Checked;
        }

        private void checkBoxMarkerSmoothing_CheckedChanged(object sender, EventArgs e)
        {
            if (_fUpdatingMarkerUI)
                return;

            ((Marker)comboBoxMarkers.SelectedItem).SmoothingEnabled = checkBoxMarkerSmoothing.Checked;
        }

        private void numericUpDownMarkerThresh_ValueChanged(object sender, EventArgs e)
        {
            ((Marker)comboBoxMarkers.SelectedItem).Threshold = (int)numericUpDownMarkerThresh.Value;
        }

        private void buttonMarkerActive_Click(object sender, EventArgs e)
        {
            if (comboBoxMarkers.SelectedIndex < 0)
                return;

            Marker marker = _touchlessMgr.Markers[comboBoxMarkers.SelectedIndex];
            marker.Active = !marker.Active;
            buttonMarkerActive.Text = marker.Active ? "Deactivate" : "Activate";
        }

        private void buttonMarkerRemove_Click(object sender, EventArgs e)
        {
            if (comboBoxMarkers.SelectedIndex < 0)
                return;

            _touchlessMgr.RemoveMarker(comboBoxMarkers.SelectedIndex);
            comboBoxMarkers.Items.RemoveAt(comboBoxMarkers.SelectedIndex);
            comboBoxMarkers.SelectedIndex = -1;
            comboBoxMarkers.Text = "Edit An Existing Marker";
            groupBoxMarkerControl.Enabled = false;
            groupBoxMarkerControl.Text = "No Marker Selected";
            if (comboBoxMarkers.Items.Count == 0)
            {
                radioButtonDemo.Enabled = false;
                comboBoxMarkers.Enabled = false;
            }
        }

        #endregion UI Marker Editing

        #region Display Interaction

        private void pictureBoxDisplay_MouseDown(object sender, MouseEventArgs e)
        {
            // If we are adding a marker - get the marker center on mouse down
            if (_fAddingMarker)
            {
                _markerCenter = e.Location;
                _markerRadius = 0;

                // Begin drawing the selection adornment
                _drawSelectionAdornment = true;
            }
        }

        private void pictureBoxDisplay_MouseUp(object sender, MouseEventArgs e)
        {
            // If we are adding a marker - get the marker radius on mouse up, add the marker
            if (_fAddingMarker)
            {
                int dx = e.X - _markerCenter.X;
                int dy = e.Y - _markerCenter.Y;
                _markerRadius = (float)Math.Sqrt(dx * dx + dy * dy);
                // Adjust for the image/display scaling (assumes proportional scaling)
                _markerCenter.X = (_markerCenter.X * _latestFrame.Width) / pictureBoxDisplay.Width;
                _markerCenter.Y = (_markerCenter.Y * _latestFrame.Height) / pictureBoxDisplay.Height;
                _markerRadius = (_markerRadius * _latestFrame.Height) / pictureBoxDisplay.Height;
                // Add the marker
                Marker newMarker = _touchlessMgr.AddMarker("Marker #" + ++_addedMarkerCount, (Bitmap)_latestFrame, _markerCenter, _markerRadius);
                comboBoxMarkers.Items.Add(newMarker);

                // Restore the app to its normal state and clear the selection area adorment
                _fAddingMarker = false;
                buttonMarkerAdd.Text = "Add A New Marker";
                _markerCenter = new Point();
                _drawSelectionAdornment = false;
                pictureBoxDisplay.Image = new Bitmap(pictureBoxDisplay.Width, pictureBoxDisplay.Height);

                // Enable the demo and marker editing
                radioButtonDemo.Enabled = true;
                buttonNext.Enabled = true;
                comboBoxMarkers.Enabled = true;
                comboBoxMarkers.SelectedIndex = comboBoxMarkers.Items.Count - 1;
            }
        }

        private void pictureBoxDisplay_MouseMove(object sender, MouseEventArgs e)
        {
            // If the user is selecting a marker, draw a circle of their selection as a selection adornment
            if (_fAddingMarker && !_markerCenter.IsEmpty)
            {
                // Get the current radius
                int dx = e.X - _markerCenter.X;
                int dy = e.Y - _markerCenter.Y;
                _markerRadius = (float)Math.Sqrt(dx * dx + dy * dy);

                // Cause display Update
                pictureBoxDisplay.Invalidate();
            }
        }

        #endregion Display Interaction

        #endregion Marker Mode

        #region Camera Mode

        private void comboBoxCameras_DropDown(object sender, EventArgs e)
        {
            // Refresh the list of available cameras
            comboBoxCameras.Items.Clear();
            foreach (Camera cam in _touchlessMgr.Cameras)
                comboBoxCameras.Items.Add(cam);
        }

        private void comboBoxCameras_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Early return if we've selected the current camera
            if (_touchlessMgr.CurrentCamera == (Camera)comboBoxCameras.SelectedItem)
                return;

            // Trash the old camera
            if (_touchlessMgr.CurrentCamera != null)
            {
                _touchlessMgr.CurrentCamera.OnImageCaptured -= new EventHandler<CameraEventArgs>(OnImageCaptured);
                _touchlessMgr.CurrentCamera.Dispose();
                _touchlessMgr.CurrentCamera = null;
                comboBoxCameras.Text = "Select A Camera";
                groupBoxCameraInfo.Enabled = false;
                groupBoxCameraInfo.Text = "No Camera Selected";
                labelCameraFPSValue.Text = "0.00";
                radioButtonMarkers.Enabled = false;
                radioButtonDemo.Enabled = false;
                pictureBoxDisplay.Paint -= new PaintEventHandler(drawLatestImage);
            }

            if (comboBoxCameras.SelectedIndex < 0)
            {
                pictureBoxDisplay.Paint -= new PaintEventHandler(drawLatestImage);
                comboBoxCameras.Text = "Select A Camera";
                return;
            }

            try
            {
                Camera c = (Camera)comboBoxCameras.SelectedItem;
                c.OnImageCaptured += new EventHandler<CameraEventArgs>(OnImageCaptured);
                _touchlessMgr.CurrentCamera = c;

                groupBoxCameraInfo.Enabled = true;
                groupBoxCameraInfo.Text = c.ToString();

                // Allow access to the marker mode once a camera has been activated
                radioButtonMarkers.Enabled = true;

                pictureBoxDisplay.Paint += new PaintEventHandler(drawLatestImage);
            }
            catch (Exception ex)
            {
                comboBoxCameras.Text = "Select A Camera";
                MessageBox.Show(ex.Message);
            }
        }

        private void buttonCameraProperties_Click(object sender, EventArgs e)
        {
            if (comboBoxCameras.SelectedIndex < 0)
                return;

            Camera c = (Camera)comboBoxCameras.SelectedItem;
            c.ShowPropertiesDialog(this.Handle);
        }

        private void checkBoxCameraFPSLimit_CheckedChanged(object sender, EventArgs e)
        {
            numericUpDownCameraFPSLimit.Visible = numericUpDownCameraFPSLimit.Enabled = checkBoxCameraFPSLimit.Checked;
            Camera c = (Camera)comboBoxCameras.SelectedItem;
            c.FpsLimit = checkBoxCameraFPSLimit.Checked ? (int)numericUpDownCameraFPSLimit.Value : -1;
        }

        private void numericUpDownCameraFPSLimit_ValueChanged(object sender, EventArgs e)
        {
            if (comboBoxCameras.SelectedIndex < 0)
                return;

            Camera c = (Camera)comboBoxCameras.SelectedItem;
            c.FpsLimit = (int)numericUpDownCameraFPSLimit.Value;
        }

        private void checkBoxCameraFlip_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxCameraFlipX.Checked)
            {
                if (checkBoxCameraFlipY.Checked)
                    _touchlessMgr.CurrentCamera.RotateFlip = RotateFlipType.RotateNoneFlipXY;
                else
                    _touchlessMgr.CurrentCamera.RotateFlip = RotateFlipType.RotateNoneFlipX;
            }
            else
            {
                if (checkBoxCameraFlipY.Checked)
                    _touchlessMgr.CurrentCamera.RotateFlip = RotateFlipType.RotateNoneFlipY;
                else
                    _touchlessMgr.CurrentCamera.RotateFlip = RotateFlipType.RotateNoneFlipNone;
            }
        }

        #endregion Camera Mode

    }

    public interface IDemoInterface
    {
        string GetDemoDescription();
        bool StartDemo(TouchlessMgr tlmgr, Rectangle displayBounds);
        void StopDemo();
        void DrawDemoCanvas(Graphics gfx);
    }
}