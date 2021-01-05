using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using TouchlessLib;
using System.Drawing.Imaging;

namespace sample
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        TouchlessMgr _touch = new TouchlessMgr();
        Bitmap _overlay;

        private void Form1_Load(object sender, EventArgs e)
        {
            foreach (Camera c in _touch.Cameras)
            {
                if (c != null)
                {
                    _touch.CurrentCamera = c;
                    c.OnImageCaptured += new EventHandler<CameraEventArgs>(c_OnImageCaptured);
                    _overlay = new Bitmap(c.CaptureWidth, c.CaptureHeight, PixelFormat.Format24bppRgb);
                    _overlay.MakeTransparent();
                    break;
                }
            }

            pictureBox1.Paint += new PaintEventHandler(pictureBox1_Paint);

        }

        void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            lock (this)
            {
                if (_b != null)
                {
                    e.Graphics.DrawImageUnscaledAndClipped(_b, pictureBox1.ClientRectangle);
                    e.Graphics.DrawImageUnscaledAndClipped(_overlay, pictureBox1.ClientRectangle);
                }
            }
        }

        Bitmap _b;

        void c_OnImageCaptured(object sender, CameraEventArgs e)
        {
            if (button1.Enabled)
            {
                _b = e.Image;
                pictureBox1.Invalidate();
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Bitmap c = _touch.CurrentCamera.GetCurrentImage();
            pictureBox1.BackgroundImage = c;
            button1.Enabled = false;
        }

        private void pictureBox1_MouseClick(object sender, MouseEventArgs e)
        {
            Marker m = _touch.AddMarker("marker", (Bitmap)pictureBox1.BackgroundImage, new Point(e.X, e.Y), 10);
            m.Highlight = true;
            m.OnChange += new EventHandler<MarkerEventArgs>(m_OnChange);
            button1.Enabled = true;
        }

        void m_OnChange(object sender, MarkerEventArgs e)
        {
            lock (this)
            {
                if (e.EventData.Present)
                {
                    _overlay.SetPixel(e.EventData.X, e.EventData.Y, Color.Black);
                }
            }
        }
    }
}