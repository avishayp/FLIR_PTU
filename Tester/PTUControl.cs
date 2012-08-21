using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using HWGlobals;

namespace Tester
{
    public partial class PTUControl : UserControl
    {

        public PTUControl()
        {
            InitializeComponent();
        }

        public bool Init(IPanTilt ptu, IPanTiltCfg ptucfg)
        {
            if (ptu != null && ptucfg != null)
            {
                _ptu = ptu;
                _ptuCfg = ptucfg;
                RegisterEvents();
                return true;
            }
            return false;
        }

        private void RegisterEvents()
        {
            _ptu.PositionChanged += new PositionChangedDelegate(_ptu_PositionChanged);
            _ptu.__Stopped += new EventHandler(_ptu___Stopped);
            _ptu.__OutOfLimits += new PtuOutOfLimits(_ptu___OutOfLimits);
        }

        void _ptu___Stopped(object sender, EventArgs e)
        {
            AllowMove(true);
        }

        void _ptu_PositionChanged(object sender, PositionArgs args)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new PositionChangedDelegate(_ptu_PositionChanged), sender, args);
                return;
            }

            lblPan.Text = args.Pos1.ToString("F3");
            lblTilt.Text = args.Pos2.ToString("F3");
        }

        void _ptu___OutOfLimits(bool pan, bool tilt)
        {
            if (pan)
            {
                nudPan.BackColor = Color.Red;
            }
            if (tilt)
            {
                nudTilt.BackColor = Color.Red;
            }
        }

        void Instance_InPositionChanged(object sender, PositionArgs args)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new PositionChangedDelegate(Instance_InPositionChanged), sender, args);
                return;
            }

            if (args.InPos)
            {
                AllowMove(true);
            }
            else
                lblPan.BackColor = lblTilt.BackColor = Color.Red;
        }

        private void AllowMove(bool yassoo)
        {
            grpBoxJog.Enabled = yassoo;
            btnMoveTo.Enabled = yassoo;
        }

        private void btnMoveTo_Click(object sender, EventArgs e)
        {
            MoveAbs((float)nudPan.Value, (float)nudTilt.Value);
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            StopAll();
        }

        private void SetSpeed()
        {
            _ptuCfg.SetSpeed((float)nudJogSpeed.Value);
        }

        private void btnElPos_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                MoveOne(1, 1);
                return;
            }

            SetSpeed();
            _ptu.JogAxis(1, 1, (float)nudJogSpeed.Value);
        }

        private void MoveOne(int ax, int step)
        {
            _ptu.JogAxis(ax + 2, step);
        }

        private void btnAzPos_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                MoveOne(0, 1);
                return;
            }

            SetSpeed();
            _ptu.JogAxis(0, 1, (float)nudJogSpeed.Value);
        }

        private void btnElNeg_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                MoveOne(1, -1);
                return;
            }

            SetSpeed();
            _ptu.JogAxis(1, -1, (float)nudJogSpeed.Value);
        }

        private void btnAzNeg_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                MoveOne(0, -1);
                return;
            }

            SetSpeed();
            _ptu.JogAxis(0, -1, (float)nudJogSpeed.Value);
        }

        private void btnElPos_MouseUp(object sender, MouseEventArgs e)
        {
            _ptu.StopAxis(1);
        }

        private void btnAzPos_MouseUp(object sender, MouseEventArgs e)
        {
            _ptu.StopAxis(0);
        }

        private void btnElNeg_MouseUp(object sender, MouseEventArgs e)
        {
            _ptu.StopAxis(1);
        }

        private void btnAzNeg_MouseUp(object sender, MouseEventArgs e)
        {
            _ptu.StopAxis(0);
        }

        private void SetHighSpeed()
        {
            _ptuCfg.SetSpeed(85);    // AP: should be 85% of maximum
        }

        private void btnHome_Click(object sender, EventArgs e)
        {
            MoveAbs(0, 0);
        }

        private void MoveAbs(float az, float el)
        {
            SetHighSpeed();
            AllowMove(false);
            _ptu.MoveAxisAsync(az, el);
        }

        private void StopAll()
        {
            _ptu.StopAxis(0);
            _ptu.StopAxis(1);
        }

        private void ResetPTU()
        {
            StopAll();
            _ptuCfg.Reset();
        }

        private void nudPan_ValueChanged(object sender, EventArgs e)
        {
            SetColor();
        }

        private void nudTilt_ValueChanged(object sender, EventArgs e)
        {
            SetColor();
        }

        private void SetColor()
        {
            nudPan.BackColor = nudTilt.BackColor = Color.White;
        }

        private IPanTilt _ptu;
        private IPanTiltCfg _ptuCfg;

    }
}
