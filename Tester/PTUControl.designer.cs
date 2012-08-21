namespace Tester
{
    partial class PTUControl
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PTUControl));
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.nudTilt = new System.Windows.Forms.NumericUpDown();
            this.nudPan = new System.Windows.Forms.NumericUpDown();
            this.grpBoxJog = new System.Windows.Forms.GroupBox();
            this.btnHome = new System.Windows.Forms.Button();
            this.btnElNeg = new System.Windows.Forms.Button();
            this.btnAzPos = new System.Windows.Forms.Button();
            this.btnAzNeg = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.btnElPos = new System.Windows.Forms.Button();
            this.nudJogSpeed = new System.Windows.Forms.NumericUpDown();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnInvoker = new System.Windows.Forms.Button();
            this.btnStop = new System.Windows.Forms.Button();
            this.btnMoveTo = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.lblTilt = new System.Windows.Forms.Label();
            this.lblPan = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.nudTilt)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudPan)).BeginInit();
            this.grpBoxJog.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudJogSpeed)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(7, 71);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(75, 17);
            this.label2.TabIndex = 16;
            this.label2.Text = "Elevation";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(7, 29);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 17);
            this.label1.TabIndex = 17;
            this.label1.Text = "Azimuth";
            // 
            // nudTilt
            // 
            this.nudTilt.DecimalPlaces = 2;
            this.nudTilt.Location = new System.Drawing.Point(89, 68);
            this.nudTilt.Maximum = new decimal(new int[] {
            360,
            0,
            0,
            0});
            this.nudTilt.Minimum = new decimal(new int[] {
            360,
            0,
            0,
            -2147483648});
            this.nudTilt.Name = "nudTilt";
            this.nudTilt.Size = new System.Drawing.Size(76, 23);
            this.nudTilt.TabIndex = 14;
            this.nudTilt.ValueChanged += new System.EventHandler(this.nudTilt_ValueChanged);
            // 
            // nudPan
            // 
            this.nudPan.DecimalPlaces = 2;
            this.nudPan.Location = new System.Drawing.Point(89, 26);
            this.nudPan.Maximum = new decimal(new int[] {
            360,
            0,
            0,
            0});
            this.nudPan.Minimum = new decimal(new int[] {
            360,
            0,
            0,
            -2147483648});
            this.nudPan.Name = "nudPan";
            this.nudPan.Size = new System.Drawing.Size(76, 23);
            this.nudPan.TabIndex = 15;
            this.nudPan.ValueChanged += new System.EventHandler(this.nudPan_ValueChanged);
            // 
            // grpBoxJog
            // 
            this.grpBoxJog.Controls.Add(this.btnHome);
            this.grpBoxJog.Controls.Add(this.btnElNeg);
            this.grpBoxJog.Controls.Add(this.btnAzPos);
            this.grpBoxJog.Controls.Add(this.btnAzNeg);
            this.grpBoxJog.Controls.Add(this.label3);
            this.grpBoxJog.Controls.Add(this.btnElPos);
            this.grpBoxJog.Controls.Add(this.nudJogSpeed);
            this.grpBoxJog.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.grpBoxJog.Location = new System.Drawing.Point(4, 3);
            this.grpBoxJog.Name = "grpBoxJog";
            this.grpBoxJog.Size = new System.Drawing.Size(180, 194);
            this.grpBoxJog.TabIndex = 10;
            this.grpBoxJog.TabStop = false;
            this.grpBoxJog.Text = "Move relative";
            // 
            // btnHome
            // 
            this.btnHome.BackColor = System.Drawing.SystemColors.InactiveCaptionText;
            this.btnHome.Image = ((System.Drawing.Image)(resources.GetObject("btnHome.Image")));
            this.btnHome.Location = new System.Drawing.Point(72, 68);
            this.btnHome.Name = "btnHome";
            this.btnHome.Size = new System.Drawing.Size(38, 35);
            this.btnHome.TabIndex = 22;
            this.btnHome.UseVisualStyleBackColor = false;
            this.btnHome.Click += new System.EventHandler(this.btnHome_Click);
            // 
            // btnElNeg
            // 
            this.btnElNeg.BackColor = System.Drawing.Color.PaleGreen;
            this.btnElNeg.Image = global::Tester.Properties.Resources.ScrollPane_arrowDown;
            this.btnElNeg.Location = new System.Drawing.Point(70, 110);
            this.btnElNeg.Name = "btnElNeg";
            this.btnElNeg.Size = new System.Drawing.Size(42, 38);
            this.btnElNeg.TabIndex = 0;
            this.btnElNeg.UseVisualStyleBackColor = false;
            this.btnElNeg.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btnElNeg_MouseDown);
            this.btnElNeg.MouseUp += new System.Windows.Forms.MouseEventHandler(this.btnElNeg_MouseUp);
            // 
            // btnAzPos
            // 
            this.btnAzPos.BackColor = System.Drawing.Color.PaleGreen;
            this.btnAzPos.Image = global::Tester.Properties.Resources.ScrollPane_arrowRight;
            this.btnAzPos.Location = new System.Drawing.Point(119, 66);
            this.btnAzPos.Name = "btnAzPos";
            this.btnAzPos.Size = new System.Drawing.Size(42, 38);
            this.btnAzPos.TabIndex = 0;
            this.btnAzPos.UseVisualStyleBackColor = false;
            this.btnAzPos.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btnAzPos_MouseDown);
            this.btnAzPos.MouseUp += new System.Windows.Forms.MouseEventHandler(this.btnAzPos_MouseUp);
            // 
            // btnAzNeg
            // 
            this.btnAzNeg.BackColor = System.Drawing.Color.PaleGreen;
            this.btnAzNeg.Image = global::Tester.Properties.Resources.ScrollPane_arrowLeft;
            this.btnAzNeg.Location = new System.Drawing.Point(21, 66);
            this.btnAzNeg.Name = "btnAzNeg";
            this.btnAzNeg.Size = new System.Drawing.Size(42, 38);
            this.btnAzNeg.TabIndex = 0;
            this.btnAzNeg.UseVisualStyleBackColor = false;
            this.btnAzNeg.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btnAzNeg_MouseDown);
            this.btnAzNeg.MouseUp += new System.Windows.Forms.MouseEventHandler(this.btnAzNeg_MouseUp);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(15, 166);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(82, 17);
            this.label3.TabIndex = 8;
            this.label3.Text = "Speed [%]";
            // 
            // btnElPos
            // 
            this.btnElPos.BackColor = System.Drawing.Color.PaleGreen;
            this.btnElPos.Image = global::Tester.Properties.Resources.ScrollPane_arrowUp;
            this.btnElPos.Location = new System.Drawing.Point(70, 23);
            this.btnElPos.Name = "btnElPos";
            this.btnElPos.Size = new System.Drawing.Size(42, 38);
            this.btnElPos.TabIndex = 0;
            this.btnElPos.UseVisualStyleBackColor = false;
            this.btnElPos.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btnElPos_MouseDown);
            this.btnElPos.MouseUp += new System.Windows.Forms.MouseEventHandler(this.btnElPos_MouseUp);
            // 
            // nudJogSpeed
            // 
            this.nudJogSpeed.DecimalPlaces = 1;
            this.nudJogSpeed.Location = new System.Drawing.Point(104, 164);
            this.nudJogSpeed.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.nudJogSpeed.Name = "nudJogSpeed";
            this.nudJogSpeed.Size = new System.Drawing.Size(65, 23);
            this.nudJogSpeed.TabIndex = 7;
            this.nudJogSpeed.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnInvoker);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.btnStop);
            this.groupBox1.Controls.Add(this.nudTilt);
            this.groupBox1.Controls.Add(this.btnMoveTo);
            this.groupBox1.Controls.Add(this.nudPan);
            this.groupBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.groupBox1.Location = new System.Drawing.Point(205, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(180, 194);
            this.groupBox1.TabIndex = 20;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Move absolute";
            // 
            // btnInvoker
            // 
            this.btnInvoker.Enabled = false;
            this.btnInvoker.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnInvoker.Location = new System.Drawing.Point(-58, 116);
            this.btnInvoker.Name = "btnInvoker";
            this.btnInvoker.Size = new System.Drawing.Size(91, 32);
            this.btnInvoker.TabIndex = 20;
            this.btnInvoker.Text = "InvokeMe";
            this.btnInvoker.UseVisualStyleBackColor = true;
            this.btnInvoker.Visible = false;
            // 
            // btnStop
            // 
            this.btnStop.Image = ((System.Drawing.Image)(resources.GetObject("btnStop.Image")));
            this.btnStop.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnStop.Location = new System.Drawing.Point(49, 154);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(88, 29);
            this.btnStop.TabIndex = 19;
            this.btnStop.Text = "Stop";
            this.btnStop.UseVisualStyleBackColor = true;
            this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
            // 
            // btnMoveTo
            // 
            this.btnMoveTo.Image = global::Tester.Properties.Resources.arrow_right;
            this.btnMoveTo.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnMoveTo.Location = new System.Drawing.Point(49, 110);
            this.btnMoveTo.Name = "btnMoveTo";
            this.btnMoveTo.Size = new System.Drawing.Size(88, 29);
            this.btnMoveTo.TabIndex = 13;
            this.btnMoveTo.Text = "Move";
            this.btnMoveTo.UseVisualStyleBackColor = true;
            this.btnMoveTo.Click += new System.EventHandler(this.btnMoveTo_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.lblTilt);
            this.groupBox2.Controls.Add(this.lblPan);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.groupBox2.ForeColor = System.Drawing.SystemColors.ActiveCaption;
            this.groupBox2.Location = new System.Drawing.Point(5, 199);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(379, 46);
            this.groupBox2.TabIndex = 21;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Current position";
            // 
            // lblTilt
            // 
            this.lblTilt.BackColor = System.Drawing.Color.PapayaWhip;
            this.lblTilt.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblTilt.Location = new System.Drawing.Point(279, 21);
            this.lblTilt.Name = "lblTilt";
            this.lblTilt.Size = new System.Drawing.Size(94, 19);
            this.lblTilt.TabIndex = 23;
            // 
            // lblPan
            // 
            this.lblPan.BackColor = System.Drawing.Color.PapayaWhip;
            this.lblPan.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblPan.Location = new System.Drawing.Point(85, 21);
            this.lblPan.Name = "lblPan";
            this.lblPan.Size = new System.Drawing.Size(94, 19);
            this.lblPan.TabIndex = 22;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(14, 21);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(65, 17);
            this.label5.TabIndex = 21;
            this.label5.Text = "Azimuth";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(201, 21);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(75, 17);
            this.label4.TabIndex = 21;
            this.label4.Text = "Elevation";
            // 
            // PTUControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.grpBoxJog);
            this.Name = "PTUControl";
            this.Size = new System.Drawing.Size(392, 251);
            ((System.ComponentModel.ISupportInitialize)(this.nudTilt)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudPan)).EndInit();
            this.grpBoxJog.ResumeLayout(false);
            this.grpBoxJog.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudJogSpeed)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnStop;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown nudTilt;
        private System.Windows.Forms.NumericUpDown nudPan;
        private System.Windows.Forms.Button btnMoveTo;
        private System.Windows.Forms.GroupBox grpBoxJog;
        private System.Windows.Forms.Button btnElNeg;
        private System.Windows.Forms.Button btnAzPos;
        private System.Windows.Forms.Button btnAzNeg;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnElPos;
        private System.Windows.Forms.NumericUpDown nudJogSpeed;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnHome;
        private System.Windows.Forms.Button btnInvoker;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label lblTilt;
        private System.Windows.Forms.Label lblPan;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
    
    }
}
