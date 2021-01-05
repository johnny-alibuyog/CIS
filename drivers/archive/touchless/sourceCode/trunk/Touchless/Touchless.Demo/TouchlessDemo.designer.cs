namespace Touchless.Demo
{
    partial class TouchlessDemo
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.buttonCameraProperties = new System.Windows.Forms.Button();
            this.comboBoxCameras = new System.Windows.Forms.ComboBox();
            this.pictureBoxDisplay = new System.Windows.Forms.PictureBox();
            this.groupBoxCamera = new System.Windows.Forms.GroupBox();
            this.groupBoxCameraInfo = new System.Windows.Forms.GroupBox();
            this.checkBoxCameraFlipY = new System.Windows.Forms.CheckBox();
            this.checkBoxCameraFlipX = new System.Windows.Forms.CheckBox();
            this.checkBoxCameraFPSLimit = new System.Windows.Forms.CheckBox();
            this.labelCameraFPSValue = new System.Windows.Forms.Label();
            this.numericUpDownCameraFPSLimit = new System.Windows.Forms.NumericUpDown();
            this.labelCameraFPS = new System.Windows.Forms.Label();
            this.radioButtonCamera = new System.Windows.Forms.RadioButton();
            this.radioButtonMarkers = new System.Windows.Forms.RadioButton();
            this.radioButtonDemo = new System.Windows.Forms.RadioButton();
            this.groupBoxMarkers = new System.Windows.Forms.GroupBox();
            this.groupBoxMarkerControl = new System.Windows.Forms.GroupBox();
            this.buttonMarkerActive = new System.Windows.Forms.Button();
            this.numericUpDownMarkerThresh = new System.Windows.Forms.NumericUpDown();
            this.labelMarkerThresh = new System.Windows.Forms.Label();
            this.checkBoxMarkerSmoothing = new System.Windows.Forms.CheckBox();
            this.checkBoxMarkerHighlight = new System.Windows.Forms.CheckBox();
            this.labelMarkerData = new System.Windows.Forms.RichTextBox();
            this.buttonMarkerRemove = new System.Windows.Forms.Button();
            this.comboBoxMarkers = new System.Windows.Forms.ComboBox();
            this.buttonMarkerAdd = new System.Windows.Forms.Button();
            this.groupBoxDemo = new System.Windows.Forms.GroupBox();
            this.buttonDemoStartStop = new System.Windows.Forms.Button();
            this.comboBoxDemos = new System.Windows.Forms.ComboBox();
            this.labelDemoInstructions = new System.Windows.Forms.RichTextBox();
            this.buttonHelp = new System.Windows.Forms.Button();
            this.buttonBack = new System.Windows.Forms.Button();
            this.buttonNext = new System.Windows.Forms.Button();
            this.linkLabelHomepage = new System.Windows.Forms.LinkLabel();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxDisplay)).BeginInit();
            this.groupBoxCamera.SuspendLayout();
            this.groupBoxCameraInfo.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownCameraFPSLimit)).BeginInit();
            this.groupBoxMarkers.SuspendLayout();
            this.groupBoxMarkerControl.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownMarkerThresh)).BeginInit();
            this.groupBoxDemo.SuspendLayout();
            this.SuspendLayout();
            // 
            // buttonCameraProperties
            // 
            this.buttonCameraProperties.Location = new System.Drawing.Point(9, 19);
            this.buttonCameraProperties.Name = "buttonCameraProperties";
            this.buttonCameraProperties.Size = new System.Drawing.Size(137, 23);
            this.buttonCameraProperties.TabIndex = 17;
            this.buttonCameraProperties.Text = "Adjust Camera Properties";
            this.buttonCameraProperties.UseVisualStyleBackColor = true;
            this.buttonCameraProperties.Click += new System.EventHandler(this.buttonCameraProperties_Click);
            // 
            // comboBoxCameras
            // 
            this.comboBoxCameras.FormattingEnabled = true;
            this.comboBoxCameras.Location = new System.Drawing.Point(10, 19);
            this.comboBoxCameras.Name = "comboBoxCameras";
            this.comboBoxCameras.Size = new System.Drawing.Size(304, 21);
            this.comboBoxCameras.TabIndex = 14;
            this.comboBoxCameras.Text = "Select A Camera";
            this.comboBoxCameras.SelectedIndexChanged += new System.EventHandler(this.comboBoxCameras_SelectedIndexChanged);
            this.comboBoxCameras.DropDown += new System.EventHandler(this.comboBoxCameras_DropDown);
            // 
            // pictureBoxDisplay
            // 
            this.pictureBoxDisplay.BackColor = System.Drawing.Color.DimGray;
            this.pictureBoxDisplay.Location = new System.Drawing.Point(338, 12);
            this.pictureBoxDisplay.Name = "pictureBoxDisplay";
            this.pictureBoxDisplay.Size = new System.Drawing.Size(640, 480);
            this.pictureBoxDisplay.TabIndex = 19;
            this.pictureBoxDisplay.TabStop = false;
            this.pictureBoxDisplay.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pictureBoxDisplay_MouseMove);
            this.pictureBoxDisplay.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pictureBoxDisplay_MouseDown);
            this.pictureBoxDisplay.MouseUp += new System.Windows.Forms.MouseEventHandler(this.pictureBoxDisplay_MouseUp);
            // 
            // groupBoxCamera
            // 
            this.groupBoxCamera.Controls.Add(this.groupBoxCameraInfo);
            this.groupBoxCamera.Controls.Add(this.comboBoxCameras);
            this.groupBoxCamera.Location = new System.Drawing.Point(12, 59);
            this.groupBoxCamera.Name = "groupBoxCamera";
            this.groupBoxCamera.Size = new System.Drawing.Size(320, 390);
            this.groupBoxCamera.TabIndex = 20;
            this.groupBoxCamera.TabStop = false;
            this.groupBoxCamera.Text = "Camera Settings";
            // 
            // groupBoxCameraInfo
            // 
            this.groupBoxCameraInfo.Controls.Add(this.checkBoxCameraFlipY);
            this.groupBoxCameraInfo.Controls.Add(this.checkBoxCameraFlipX);
            this.groupBoxCameraInfo.Controls.Add(this.checkBoxCameraFPSLimit);
            this.groupBoxCameraInfo.Controls.Add(this.labelCameraFPSValue);
            this.groupBoxCameraInfo.Controls.Add(this.numericUpDownCameraFPSLimit);
            this.groupBoxCameraInfo.Controls.Add(this.labelCameraFPS);
            this.groupBoxCameraInfo.Controls.Add(this.buttonCameraProperties);
            this.groupBoxCameraInfo.Enabled = false;
            this.groupBoxCameraInfo.Location = new System.Drawing.Point(9, 48);
            this.groupBoxCameraInfo.Name = "groupBoxCameraInfo";
            this.groupBoxCameraInfo.Size = new System.Drawing.Size(305, 116);
            this.groupBoxCameraInfo.TabIndex = 20;
            this.groupBoxCameraInfo.TabStop = false;
            this.groupBoxCameraInfo.Text = "No Camera Selected";
            // 
            // checkBoxCameraFlipY
            // 
            this.checkBoxCameraFlipY.AutoSize = true;
            this.checkBoxCameraFlipY.Location = new System.Drawing.Point(155, 93);
            this.checkBoxCameraFlipY.Name = "checkBoxCameraFlipY";
            this.checkBoxCameraFlipY.Size = new System.Drawing.Size(119, 17);
            this.checkBoxCameraFlipY.TabIndex = 22;
            this.checkBoxCameraFlipY.Text = "Flip Image Vertically";
            this.checkBoxCameraFlipY.UseVisualStyleBackColor = true;
            this.checkBoxCameraFlipY.CheckedChanged += new System.EventHandler(this.checkBoxCameraFlip_CheckedChanged);
            // 
            // checkBoxCameraFlipX
            // 
            this.checkBoxCameraFlipX.AutoSize = true;
            this.checkBoxCameraFlipX.Location = new System.Drawing.Point(9, 93);
            this.checkBoxCameraFlipX.Name = "checkBoxCameraFlipX";
            this.checkBoxCameraFlipX.Size = new System.Drawing.Size(131, 17);
            this.checkBoxCameraFlipX.TabIndex = 22;
            this.checkBoxCameraFlipX.Text = "Flip Image Horizontally";
            this.checkBoxCameraFlipX.UseVisualStyleBackColor = true;
            this.checkBoxCameraFlipX.CheckedChanged += new System.EventHandler(this.checkBoxCameraFlip_CheckedChanged);
            // 
            // checkBoxCameraFPSLimit
            // 
            this.checkBoxCameraFPSLimit.AutoSize = true;
            this.checkBoxCameraFPSLimit.Location = new System.Drawing.Point(9, 70);
            this.checkBoxCameraFPSLimit.Name = "checkBoxCameraFPSLimit";
            this.checkBoxCameraFPSLimit.Size = new System.Drawing.Size(143, 17);
            this.checkBoxCameraFPSLimit.TabIndex = 21;
            this.checkBoxCameraFPSLimit.Text = "Limit Frames Per Second";
            this.checkBoxCameraFPSLimit.UseVisualStyleBackColor = true;
            this.checkBoxCameraFPSLimit.CheckedChanged += new System.EventHandler(this.checkBoxCameraFPSLimit_CheckedChanged);
            // 
            // labelCameraFPSValue
            // 
            this.labelCameraFPSValue.AutoSize = true;
            this.labelCameraFPSValue.Location = new System.Drawing.Point(153, 49);
            this.labelCameraFPSValue.Name = "labelCameraFPSValue";
            this.labelCameraFPSValue.Size = new System.Drawing.Size(28, 13);
            this.labelCameraFPSValue.TabIndex = 20;
            this.labelCameraFPSValue.Text = "0.00";
            // 
            // numericUpDownCameraFPSLimit
            // 
            this.numericUpDownCameraFPSLimit.Enabled = false;
            this.numericUpDownCameraFPSLimit.Increment = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.numericUpDownCameraFPSLimit.Location = new System.Drawing.Point(156, 68);
            this.numericUpDownCameraFPSLimit.Maximum = new decimal(new int[] {
            30,
            0,
            0,
            0});
            this.numericUpDownCameraFPSLimit.Name = "numericUpDownCameraFPSLimit";
            this.numericUpDownCameraFPSLimit.Size = new System.Drawing.Size(50, 20);
            this.numericUpDownCameraFPSLimit.TabIndex = 19;
            this.numericUpDownCameraFPSLimit.Value = new decimal(new int[] {
            30,
            0,
            0,
            0});
            this.numericUpDownCameraFPSLimit.Visible = false;
            this.numericUpDownCameraFPSLimit.ValueChanged += new System.EventHandler(this.numericUpDownCameraFPSLimit_ValueChanged);
            // 
            // labelCameraFPS
            // 
            this.labelCameraFPS.AutoSize = true;
            this.labelCameraFPS.Location = new System.Drawing.Point(6, 49);
            this.labelCameraFPS.Name = "labelCameraFPS";
            this.labelCameraFPS.Size = new System.Drawing.Size(140, 13);
            this.labelCameraFPS.TabIndex = 0;
            this.labelCameraFPS.Text = "Current Frames Per Second:";
            // 
            // radioButtonCamera
            // 
            this.radioButtonCamera.Appearance = System.Windows.Forms.Appearance.Button;
            this.radioButtonCamera.AutoSize = true;
            this.radioButtonCamera.Font = new System.Drawing.Font("Calibri", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.radioButtonCamera.Location = new System.Drawing.Point(12, 12);
            this.radioButtonCamera.Name = "radioButtonCamera";
            this.radioButtonCamera.Size = new System.Drawing.Size(100, 39);
            this.radioButtonCamera.TabIndex = 21;
            this.radioButtonCamera.TabStop = true;
            this.radioButtonCamera.Text = "Camera";
            this.radioButtonCamera.UseVisualStyleBackColor = true;
            this.radioButtonCamera.CheckedChanged += new System.EventHandler(this.radioButtonMode_CheckedChanged);
            // 
            // radioButtonMarkers
            // 
            this.radioButtonMarkers.Appearance = System.Windows.Forms.Appearance.Button;
            this.radioButtonMarkers.AutoSize = true;
            this.radioButtonMarkers.Enabled = false;
            this.radioButtonMarkers.Font = new System.Drawing.Font("Calibri", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.radioButtonMarkers.Location = new System.Drawing.Point(126, 12);
            this.radioButtonMarkers.Name = "radioButtonMarkers";
            this.radioButtonMarkers.Size = new System.Drawing.Size(107, 39);
            this.radioButtonMarkers.TabIndex = 22;
            this.radioButtonMarkers.TabStop = true;
            this.radioButtonMarkers.Text = "Markers";
            this.radioButtonMarkers.UseVisualStyleBackColor = true;
            this.radioButtonMarkers.CheckedChanged += new System.EventHandler(this.radioButtonMode_CheckedChanged);
            // 
            // radioButtonDemo
            // 
            this.radioButtonDemo.Appearance = System.Windows.Forms.Appearance.Button;
            this.radioButtonDemo.AutoSize = true;
            this.radioButtonDemo.Enabled = false;
            this.radioButtonDemo.Font = new System.Drawing.Font("Calibri", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.radioButtonDemo.Location = new System.Drawing.Point(247, 12);
            this.radioButtonDemo.Name = "radioButtonDemo";
            this.radioButtonDemo.Size = new System.Drawing.Size(83, 39);
            this.radioButtonDemo.TabIndex = 23;
            this.radioButtonDemo.TabStop = true;
            this.radioButtonDemo.Text = "Demo";
            this.radioButtonDemo.UseVisualStyleBackColor = true;
            this.radioButtonDemo.CheckedChanged += new System.EventHandler(this.radioButtonMode_CheckedChanged);
            // 
            // groupBoxMarkers
            // 
            this.groupBoxMarkers.Controls.Add(this.groupBoxMarkerControl);
            this.groupBoxMarkers.Controls.Add(this.comboBoxMarkers);
            this.groupBoxMarkers.Controls.Add(this.buttonMarkerAdd);
            this.groupBoxMarkers.Location = new System.Drawing.Point(12, 59);
            this.groupBoxMarkers.Name = "groupBoxMarkers";
            this.groupBoxMarkers.Size = new System.Drawing.Size(320, 390);
            this.groupBoxMarkers.TabIndex = 21;
            this.groupBoxMarkers.TabStop = false;
            this.groupBoxMarkers.Text = "Marker Settings";
            // 
            // groupBoxMarkerControl
            // 
            this.groupBoxMarkerControl.Controls.Add(this.buttonMarkerActive);
            this.groupBoxMarkerControl.Controls.Add(this.numericUpDownMarkerThresh);
            this.groupBoxMarkerControl.Controls.Add(this.labelMarkerThresh);
            this.groupBoxMarkerControl.Controls.Add(this.checkBoxMarkerSmoothing);
            this.groupBoxMarkerControl.Controls.Add(this.checkBoxMarkerHighlight);
            this.groupBoxMarkerControl.Controls.Add(this.labelMarkerData);
            this.groupBoxMarkerControl.Controls.Add(this.buttonMarkerRemove);
            this.groupBoxMarkerControl.Enabled = false;
            this.groupBoxMarkerControl.Location = new System.Drawing.Point(10, 48);
            this.groupBoxMarkerControl.Name = "groupBoxMarkerControl";
            this.groupBoxMarkerControl.Size = new System.Drawing.Size(304, 237);
            this.groupBoxMarkerControl.TabIndex = 25;
            this.groupBoxMarkerControl.TabStop = false;
            this.groupBoxMarkerControl.Text = "No Marker Selected";
            // 
            // buttonMarkerActive
            // 
            this.buttonMarkerActive.Location = new System.Drawing.Point(151, 15);
            this.buttonMarkerActive.Name = "buttonMarkerActive";
            this.buttonMarkerActive.Size = new System.Drawing.Size(68, 23);
            this.buttonMarkerActive.TabIndex = 6;
            this.buttonMarkerActive.Text = "Deactivate";
            this.buttonMarkerActive.UseVisualStyleBackColor = true;
            this.buttonMarkerActive.Click += new System.EventHandler(this.buttonMarkerActive_Click);
            // 
            // numericUpDownMarkerThresh
            // 
            this.numericUpDownMarkerThresh.Location = new System.Drawing.Point(251, 44);
            this.numericUpDownMarkerThresh.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.numericUpDownMarkerThresh.Name = "numericUpDownMarkerThresh";
            this.numericUpDownMarkerThresh.Size = new System.Drawing.Size(47, 20);
            this.numericUpDownMarkerThresh.TabIndex = 5;
            this.numericUpDownMarkerThresh.ValueChanged += new System.EventHandler(this.numericUpDownMarkerThresh_ValueChanged);
            // 
            // labelMarkerThresh
            // 
            this.labelMarkerThresh.AutoSize = true;
            this.labelMarkerThresh.Location = new System.Drawing.Point(152, 46);
            this.labelMarkerThresh.Name = "labelMarkerThresh";
            this.labelMarkerThresh.Size = new System.Drawing.Size(93, 13);
            this.labelMarkerThresh.TabIndex = 4;
            this.labelMarkerThresh.Text = "Marker Threshold:";
            // 
            // checkBoxMarkerSmoothing
            // 
            this.checkBoxMarkerSmoothing.AutoSize = true;
            this.checkBoxMarkerSmoothing.Checked = true;
            this.checkBoxMarkerSmoothing.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxMarkerSmoothing.Location = new System.Drawing.Point(6, 45);
            this.checkBoxMarkerSmoothing.Name = "checkBoxMarkerSmoothing";
            this.checkBoxMarkerSmoothing.Size = new System.Drawing.Size(124, 17);
            this.checkBoxMarkerSmoothing.TabIndex = 3;
            this.checkBoxMarkerSmoothing.Text = "Smooth Marker Data";
            this.checkBoxMarkerSmoothing.UseVisualStyleBackColor = true;
            this.checkBoxMarkerSmoothing.CheckedChanged += new System.EventHandler(this.checkBoxMarkerSmoothing_CheckedChanged);
            // 
            // checkBoxMarkerHighlight
            // 
            this.checkBoxMarkerHighlight.AutoSize = true;
            this.checkBoxMarkerHighlight.Checked = true;
            this.checkBoxMarkerHighlight.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxMarkerHighlight.Location = new System.Drawing.Point(5, 19);
            this.checkBoxMarkerHighlight.Name = "checkBoxMarkerHighlight";
            this.checkBoxMarkerHighlight.Size = new System.Drawing.Size(103, 17);
            this.checkBoxMarkerHighlight.TabIndex = 2;
            this.checkBoxMarkerHighlight.Text = "Highlight Marker";
            this.checkBoxMarkerHighlight.UseVisualStyleBackColor = true;
            this.checkBoxMarkerHighlight.CheckedChanged += new System.EventHandler(this.checkBoxMarkerHighlight_CheckedChanged);
            // 
            // labelMarkerData
            // 
            this.labelMarkerData.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.labelMarkerData.Location = new System.Drawing.Point(7, 70);
            this.labelMarkerData.Name = "labelMarkerData";
            this.labelMarkerData.ReadOnly = true;
            this.labelMarkerData.Size = new System.Drawing.Size(291, 161);
            this.labelMarkerData.TabIndex = 1;
            this.labelMarkerData.Text = "";
            // 
            // buttonMarkerRemove
            // 
            this.buttonMarkerRemove.Location = new System.Drawing.Point(225, 15);
            this.buttonMarkerRemove.Name = "buttonMarkerRemove";
            this.buttonMarkerRemove.Size = new System.Drawing.Size(73, 23);
            this.buttonMarkerRemove.TabIndex = 0;
            this.buttonMarkerRemove.Text = "Remove";
            this.buttonMarkerRemove.UseVisualStyleBackColor = true;
            this.buttonMarkerRemove.Click += new System.EventHandler(this.buttonMarkerRemove_Click);
            // 
            // comboBoxMarkers
            // 
            this.comboBoxMarkers.DisplayMember = "Name";
            this.comboBoxMarkers.Enabled = false;
            this.comboBoxMarkers.FormattingEnabled = true;
            this.comboBoxMarkers.Location = new System.Drawing.Point(165, 19);
            this.comboBoxMarkers.Name = "comboBoxMarkers";
            this.comboBoxMarkers.Size = new System.Drawing.Size(148, 21);
            this.comboBoxMarkers.TabIndex = 22;
            this.comboBoxMarkers.Text = "Edit An Existing Marker";
            this.comboBoxMarkers.SelectedIndexChanged += new System.EventHandler(this.comboBoxMarkers_SelectedIndexChanged);
            this.comboBoxMarkers.DropDown += new System.EventHandler(this.comboBoxMarkers_DropDown);
            // 
            // buttonMarkerAdd
            // 
            this.buttonMarkerAdd.Location = new System.Drawing.Point(10, 17);
            this.buttonMarkerAdd.Name = "buttonMarkerAdd";
            this.buttonMarkerAdd.Size = new System.Drawing.Size(151, 23);
            this.buttonMarkerAdd.TabIndex = 19;
            this.buttonMarkerAdd.Text = "Add A New Marker";
            this.buttonMarkerAdd.UseVisualStyleBackColor = true;
            this.buttonMarkerAdd.Click += new System.EventHandler(this.buttonMarkerAdd_Click);
            // 
            // groupBoxDemo
            // 
            this.groupBoxDemo.Controls.Add(this.buttonDemoStartStop);
            this.groupBoxDemo.Controls.Add(this.comboBoxDemos);
            this.groupBoxDemo.Controls.Add(this.labelDemoInstructions);
            this.groupBoxDemo.Location = new System.Drawing.Point(12, 59);
            this.groupBoxDemo.Name = "groupBoxDemo";
            this.groupBoxDemo.Size = new System.Drawing.Size(320, 390);
            this.groupBoxDemo.TabIndex = 26;
            this.groupBoxDemo.TabStop = false;
            this.groupBoxDemo.Text = "Demo Mode Instructions";
            // 
            // buttonDemoStartStop
            // 
            this.buttonDemoStartStop.Enabled = false;
            this.buttonDemoStartStop.Location = new System.Drawing.Point(161, 19);
            this.buttonDemoStartStop.Name = "buttonDemoStartStop";
            this.buttonDemoStartStop.Size = new System.Drawing.Size(153, 23);
            this.buttonDemoStartStop.TabIndex = 26;
            this.buttonDemoStartStop.Text = "Start Demo";
            this.buttonDemoStartStop.UseVisualStyleBackColor = true;
            this.buttonDemoStartStop.Click += new System.EventHandler(this.buttonDemoStartStop_Click);
            // 
            // comboBoxDemos
            // 
            this.comboBoxDemos.DisplayMember = "Name";
            this.comboBoxDemos.FormattingEnabled = true;
            this.comboBoxDemos.Location = new System.Drawing.Point(6, 21);
            this.comboBoxDemos.Name = "comboBoxDemos";
            this.comboBoxDemos.Size = new System.Drawing.Size(149, 21);
            this.comboBoxDemos.TabIndex = 25;
            this.comboBoxDemos.Text = "Select A Demo";
            this.comboBoxDemos.SelectedIndexChanged += new System.EventHandler(this.comboBoxDemos_SelectedIndexChanged);
            // 
            // labelDemoInstructions
            // 
            this.labelDemoInstructions.BackColor = System.Drawing.SystemColors.Control;
            this.labelDemoInstructions.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.labelDemoInstructions.Enabled = false;
            this.labelDemoInstructions.Location = new System.Drawing.Point(6, 48);
            this.labelDemoInstructions.Name = "labelDemoInstructions";
            this.labelDemoInstructions.ReadOnly = true;
            this.labelDemoInstructions.Size = new System.Drawing.Size(308, 336);
            this.labelDemoInstructions.TabIndex = 24;
            this.labelDemoInstructions.Text = "";
            // 
            // buttonHelp
            // 
            this.buttonHelp.Location = new System.Drawing.Point(12, 455);
            this.buttonHelp.Name = "buttonHelp";
            this.buttonHelp.Size = new System.Drawing.Size(147, 23);
            this.buttonHelp.TabIndex = 27;
            this.buttonHelp.Text = "Help!";
            this.buttonHelp.UseVisualStyleBackColor = true;
            this.buttonHelp.Click += new System.EventHandler(this.buttonHelp_Click);
            // 
            // buttonBack
            // 
            this.buttonBack.Enabled = false;
            this.buttonBack.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.buttonBack.Location = new System.Drawing.Point(176, 455);
            this.buttonBack.Name = "buttonBack";
            this.buttonBack.Size = new System.Drawing.Size(75, 37);
            this.buttonBack.TabIndex = 28;
            this.buttonBack.Text = "Back";
            this.buttonBack.UseVisualStyleBackColor = true;
            this.buttonBack.Click += new System.EventHandler(this.buttonBack_Click);
            // 
            // buttonNext
            // 
            this.buttonNext.Enabled = false;
            this.buttonNext.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.buttonNext.Location = new System.Drawing.Point(257, 455);
            this.buttonNext.Name = "buttonNext";
            this.buttonNext.Size = new System.Drawing.Size(75, 37);
            this.buttonNext.TabIndex = 29;
            this.buttonNext.Text = "Next";
            this.buttonNext.UseVisualStyleBackColor = true;
            this.buttonNext.Click += new System.EventHandler(this.buttonNext_Click);
            // 
            // linkLabelHomepage
            // 
            this.linkLabelHomepage.AutoSize = true;
            this.linkLabelHomepage.Location = new System.Drawing.Point(12, 479);
            this.linkLabelHomepage.Name = "linkLabelHomepage";
            this.linkLabelHomepage.Size = new System.Drawing.Size(150, 13);
            this.linkLabelHomepage.TabIndex = 30;
            this.linkLabelHomepage.TabStop = true;
            this.linkLabelHomepage.Text = "www.codeplex.com/touchless";
            this.linkLabelHomepage.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabelHomepage_LinkClicked);
            // 
            // TouchlessDemo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(991, 503);
            this.Controls.Add(this.linkLabelHomepage);
            this.Controls.Add(this.buttonNext);
            this.Controls.Add(this.buttonBack);
            this.Controls.Add(this.buttonHelp);
            this.Controls.Add(this.radioButtonDemo);
            this.Controls.Add(this.radioButtonCamera);
            this.Controls.Add(this.radioButtonMarkers);
            this.Controls.Add(this.pictureBoxDisplay);
            this.Controls.Add(this.groupBoxCamera);
            this.Controls.Add(this.groupBoxDemo);
            this.Controls.Add(this.groupBoxMarkers);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "TouchlessDemo";
            this.Text = "Touchless Demo";
            this.Load += new System.EventHandler(this.TouchlessDemo_Load);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.TouchlessDemo_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxDisplay)).EndInit();
            this.groupBoxCamera.ResumeLayout(false);
            this.groupBoxCameraInfo.ResumeLayout(false);
            this.groupBoxCameraInfo.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownCameraFPSLimit)).EndInit();
            this.groupBoxMarkers.ResumeLayout(false);
            this.groupBoxMarkerControl.ResumeLayout(false);
            this.groupBoxMarkerControl.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownMarkerThresh)).EndInit();
            this.groupBoxDemo.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonCameraProperties;
        private System.Windows.Forms.ComboBox comboBoxCameras;
        private System.Windows.Forms.PictureBox pictureBoxDisplay;
        private System.Windows.Forms.GroupBox groupBoxCamera;
        private System.Windows.Forms.RadioButton radioButtonCamera;
        private System.Windows.Forms.RadioButton radioButtonMarkers;
        private System.Windows.Forms.RadioButton radioButtonDemo;
        private System.Windows.Forms.GroupBox groupBoxMarkers;
        private System.Windows.Forms.Button buttonMarkerAdd;
        private System.Windows.Forms.ComboBox comboBoxMarkers;
        private System.Windows.Forms.GroupBox groupBoxMarkerControl;
        private System.Windows.Forms.RichTextBox labelMarkerData;
        private System.Windows.Forms.Button buttonMarkerRemove;
        private System.Windows.Forms.GroupBox groupBoxCameraInfo;
        private System.Windows.Forms.Label labelCameraFPS;
        private System.Windows.Forms.GroupBox groupBoxDemo;
        private System.Windows.Forms.CheckBox checkBoxMarkerHighlight;
        private System.Windows.Forms.CheckBox checkBoxMarkerSmoothing;
        private System.Windows.Forms.Label labelMarkerThresh;
        private System.Windows.Forms.NumericUpDown numericUpDownCameraFPSLimit;
        private System.Windows.Forms.NumericUpDown numericUpDownMarkerThresh;
        private System.Windows.Forms.Button buttonHelp;
        private System.Windows.Forms.Button buttonBack;
        private System.Windows.Forms.Button buttonNext;
        private System.Windows.Forms.Label labelCameraFPSValue;
        private System.Windows.Forms.CheckBox checkBoxCameraFPSLimit;
        private System.Windows.Forms.RichTextBox labelDemoInstructions;
        private System.Windows.Forms.LinkLabel linkLabelHomepage;
        private System.Windows.Forms.CheckBox checkBoxCameraFlipY;
        private System.Windows.Forms.CheckBox checkBoxCameraFlipX;
        private System.Windows.Forms.Button buttonMarkerActive;
        private System.Windows.Forms.ComboBox comboBoxDemos;
        private System.Windows.Forms.Button buttonDemoStartStop;
    }
}

