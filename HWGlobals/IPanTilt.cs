using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace HWGlobals
{
    public delegate void PositionChangedDelegate(object sender, PositionArgs args);
    public delegate void PtuOutOfLimits(bool pan, bool tilt);

    public interface IPanTilt
    {
        event PositionChangedDelegate PositionChanged;
        event EventHandler __Stopped;
        event PtuOutOfLimits __OutOfLimits;
        
        bool IsInitialized
        {           
            get;
        }

        int Init(object conData);
        int Close();

        int MoveAxisAsync(float pos1, float pos2);
        int StopAxis(int ax);
        int ReadPosition(int ax, out float pos);
        int JogAxis(int ax, int dir);
        int JogAxis(int ax, int dir, float vel);
    }

    // The moq
    public class IPanTilt_moq : IPanTilt
    {
        public event PositionChangedDelegate PositionChanged;

        // AP
        public event EventHandler __Stopped;
        public event PtuOutOfLimits __OutOfLimits;

        public bool IsInitialized
        {
            get { return true; }
        }

        public int Init(object conData)
        {
            return 1;
        }

        public int Close()
        {
            return 1;
        }

        public int MoveAxisAsync(float pos1, float pos2)
        {
            return 1;
        }

        public int StopAxis(int ax)
        {
            return 1;
        }

        public int ReadPosition(int ax, out float pos)
        {
            pos = 0;
            return 1;
        }

        public int JogAxis(int ax, int dir)
        {
            return 1;
        }

        public int JogAxis(int ax, int dir, float vel)
        {
            return 1;
        }
    }
}
