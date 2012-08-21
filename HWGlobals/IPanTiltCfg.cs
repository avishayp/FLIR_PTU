using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HWGlobals
{

    public enum PtuDirections
    {
        None,
        PanCW,
        PanCCW,
        TiltUp,
        TiltDown,
        All
    }

    public delegate void PtuUserLimits(Dictionary<PtuDirections, float> dic);

    public interface IPanTiltCfg
    {
        event PtuUserLimits __UserLimitsUpdate;

        int SetMaximumSpeed(int speed);
        int SetSpeed(float speedPercent);
        int SetLimit(PtuDirections dir, float val);
        int SetLimits(float left, float right, float up, float down);
        int GetLimit(PtuDirections dir, out float val);
        int Reset();
    }

    // the moq
    public class PTUConfigDemo : IPanTiltCfg
    {
        public event PtuUserLimits __UserLimitsUpdate;

        public int SetMaximumSpeed(int speed)
        {
            return 0;
        }

        public int SetSpeed(float s)
        {
            return 0;
        }

        public int SetLimit(PtuDirections dir, float f)
        {
            return 0;
        }

        public int GetLimit(PtuDirections dir, out float val)
        {
            val = 0;
            return 0;
        }

        public int SetLimits(float l, float r, float u, float d)
        {
            return 0;
        }

        public int Reset()
        {
            return 0;
        }
    }
}
