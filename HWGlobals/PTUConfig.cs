using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace HWGlobals
{
    public class PTUConfig
    {
        private PTUConfig()
        {
            MaxSpeed = 2000;
            LeftLimit = -160f;
            RightLimit = 160f;
            UpLimit = 30f;
            DownLimit = -30f;
        }

        public void Config(int maxspeed, float leftlimit, float rightlimit, float uplimit, float downlimit)
        {
            _instance.MaxSpeed = maxspeed;
            _instance.LeftLimit = leftlimit;
            _instance.RightLimit = rightlimit;
            _instance.UpLimit = uplimit;
            _instance.DownLimit = downlimit;
        }

        public int MaxSpeed { private set; get; }
        public float LeftLimit { private set; get; }
        public float RightLimit { private set; get; }
        public float UpLimit { private set; get; }
        public float DownLimit { private set; get; }

        private static PTUConfig _instance;

        public static PTUConfig Instance 
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new PTUConfig();
                }
                return _instance;
            }
        }

    }
}
