﻿namespace Tester
{
    partial class PTUTester
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
            this.ptuControl1 = new Tester.PTUControl();
            this.SuspendLayout();
            // 
            // ptuControl1
            // 
            this.ptuControl1.Enabled = false;
            this.ptuControl1.Location = new System.Drawing.Point(12, 12);
            this.ptuControl1.Name = "ptuControl1";
            this.ptuControl1.Size = new System.Drawing.Size(392, 251);
            this.ptuControl1.TabIndex = 0;
            // 
            // PTUTester
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(409, 270);
            this.Controls.Add(this.ptuControl1);
            this.Name = "PTUTester";
            this.Text = "PTUTester";
            this.ResumeLayout(false);

        }

        #endregion

        private PTUControl ptuControl1;
    }
}