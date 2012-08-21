using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HWGlobals
{
    public class PositionArgs : EventArgs
    {
        public double Pos1 { get; set; }
        public double Pos2 { get; set; }
        public bool InPos { get; set; }     
    }
}
