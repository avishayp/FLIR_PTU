using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using PTU;
using System.Net;
using HWGlobals;

namespace Tester
{
    public partial class PTUTester : Form
    {
        const String PTU_IP = "192.168.1.47";

        public PTUTester()
        {
            InitializeComponent();
        }

        public void RunTest()
        {
            MovingDevice md = new MovingDevice();
            if (md.Init(PTU_IP) == 0)
            {
                if (ptuControl1.Init(md as IPanTilt, md as IPanTiltCfg))
                    ptuControl1.Enabled = true;
            }
        }

    }
}
