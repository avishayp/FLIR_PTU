using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using HWGlobals;
using System.Net;
using System.Net.Sockets;

namespace PTU
{
    public class MovingDevice : IPanTilt, IPanTiltCfg
    {
        #region ///////   P R I V A T E   ///////

        private IPTUWrapper PTU;
        float targetAz;
        float targetEl;
        float POS_TOLL = 0.04f;
        private bool initialized;
        private float PodEl;
        private float PodAz;

        private float SpeedPercent { get; set; }
        private int _maxSpeed = 2000;
        private float MaxPan { get; set; }
        private float MaxTilt { get; set; }
        private float MinPan { get; set; }
        private float MinTilt { get; set; }

        private Timer _keepAlive;

        #endregion

        public event PositionChangedDelegate PositionChanged;
        public event EventHandler __Stopped;
        public event PtuOutOfLimits __OutOfLimits;

        private void InvokeLimitsUpdate()
        {
            if (__UserLimitsUpdate != null)
            {
                Dictionary<PtuDirections, float> dic = new Dictionary<PtuDirections, float>();
                dic[PtuDirections.PanCCW] = MinPan;
                dic[PtuDirections.PanCW] = MaxPan;
                dic[PtuDirections.TiltDown] = MinTilt;
                dic[PtuDirections.TiltUp] = MaxTilt;
                __UserLimitsUpdate(dic);
            }
        }

        private void InvokeOutOfLimits(bool pan, bool tilt)
        {
            if (__OutOfLimits != null)
            {
                __OutOfLimits(pan, tilt);
            }
        }

        private void InvokeStopped()
        {
            if (__Stopped != null)
            {
                __Stopped(null, null);
            }
        }

        public int MaxSpeed
        {
            get { return _maxSpeed; }
        }

        public int JogAxis(int ax, int dir, float speed)
        {
            SetSpeedIfNeeded(speed);
            return JogAxis(ax, dir);
        }

        public int Speed { get; private set; }

        private int MoveClicks(int ax, int step)
        {
            return PTU.JogRelative(ax, step);
        }

        private void Config()
        {
            Reset();
            Wait(5000);
            SetDefaultSpeed();
            EnableEncodersCorrection();
            DisableEcho();  // PTU will not echo back commands
            EnableTerseMode();  // PTU will return numeric output
            ReadLimits();
        }

        private void ReadLimits()
        {
            float f;
            PTU.GetLimit(PtuDirections.PanCCW, out f);
            MinPan = f;
            PTU.GetLimit(PtuDirections.PanCW, out f);
            MaxPan = f;
            PTU.GetLimit(PtuDirections.TiltDown, out f);
            MinTilt = f;
            PTU.GetLimit(PtuDirections.TiltUp, out f);
            MaxTilt = f;
            InvokeLimitsUpdate();
        }

        private void SetDefaultSpeed()
        {
            SetAcceleration(2000);
            SetMaximumSpeed(2000);
            SetSpeed(50);   // 10% of maximum
        }

        private void DisableEcho()
        {
            PTU.SendMessage(OPCODES.ECHO_DISABLE);
        }

        private void EnableTerseMode()
        {
            PTU.SendMessage(OPCODES.REPORT_TERSE);
        }

        private void SetAcceleration(int acc)
        {
            PTU.SendMessage(OPCODES.PAN_ACCELERATION, acc);
            PTU.SendMessage(OPCODES.TILT_ACCELERATION, acc);
        }

        private void DisableLimits()
        {
            PTU.ResetLimits();
        }

        private void EnableEncodersCorrection()
        {
            PTU.SendMessage(OPCODES.ENCODERS_CORRECTION_CONTROL);
        }

        private void InvokePositionChanged(PositionArgs args)
        {
            PositionChangedDelegate position = PositionChanged;
            if (position != null) position(this, args);
        }

        public int InitIP(IPAddress IP)
        {
            Sender _socTCP;
            try
            {
                IPAddress hostIP = IP;

                _socTCP = new Sender();

                IPEndPoint ep = new IPEndPoint(hostIP, Properties.Settings.Default.PTUPort);

                _socTCP.Connect(ep);
                initialized = true;

                PTU.Init(_socTCP);
            }
            catch
            {
                initialized = false;
                return -1;
            }
            return 0;
        }

        private void heartbitFunc(object stateInfo)
        {
            GetPosition();
        }

        private void GetPosition()
        {
            float pan = PTU.CurrentPosPan;
            float tilt = PTU.CurrentPosTilt;
            if ((PodAz != pan) || (PodEl != tilt))
            {
                PodAz = pan;
                PodEl = tilt;
                InvokePositionChanged(new PositionArgs() { Pos1 = PodAz, Pos2 = PodEl });
            }
        }

        private bool IsInPosition
        {
            get { return ((Math.Abs(PodAz - targetAz) < POS_TOLL) && (Math.Abs(PodEl - targetEl) < POS_TOLL)); }
        }

        private void Wait(int t)
        {
            Thread.Sleep(t);
        }

        public bool IsInitialized
        {
            get { return initialized; }
        }

        public int Init(object conData)
        {
            PTU = new PtuEthernetWrapper();
            if (InitIP(IPAddress.Parse(conData.ToString())) != 0)
                return -1;

            // set PTU defaults:
            Config();
            PTU.Poll(100);  // PTU internal poll
            // start poll position
            _keepAlive = new Timer(heartbitFunc, null, 200, 200);

            return 0;
        }

        public int Close()
        {
            return 0;
        }

        public int MoveAxisAsync(float pos1, float pos2)
        {
            bool isp = !PTU.IsInLimits(0, pos1), ist = !PTU.IsInLimits(1, pos2);
            if (isp || ist)
            {
                InvokeOutOfLimits(isp, ist);
                InvokeStopped();
                return 0;
            }

            InvokePositionChanged(new PositionArgs()
            {
                Pos1 = pos1,
                Pos2 = pos2,
            });

            targetAz = pos1;
            targetEl = pos2;

            if (!IsInPosition)
            {
                PTU.MoveAbsolute(targetAz, targetEl);
            }

            return 0;
        }

        public int StopAxis(int ax)
        {
            if ((PTU != null) && IsInitialized)
            {
                PTU.StopAll();
            }
            InvokeStopped();
            return 0;
        }

        public int ReadPosition(int ax, out float pos)
        {
            pos = (ax == 0 ? PodAz : PodEl);
            return 0;
        }

        private int SetSpeedIfNeeded(float speedPercent)
        {
            if (SpeedPercentToInt(speedPercent) != SpeedPercentToInt(SpeedPercent))
            {
                SpeedPercent = speedPercent;
                return PTU.SetSpeed((int)(SpeedPercent / 100.0 * MaxSpeed));
            }
            return 0;
        }

        private int SpeedPercentToInt(float percent)
        {
            return (int)(percent / 100 * MaxSpeed);
        }

        public int JogAxis(int ax, int step)
        {
            if (ax > 1)
            {
                return MoveClicks(ax - 2, step);
            }

            return (ax == 0 ? PTU.JogAbsPositionPan(GetMaxMove(ax, step)) : PTU.JogAbsPositionTilt(GetMaxMove(ax, step)));
        }

        private float GetMaxMove(int ax, int step)
        {
            if (ax == 0)
            {
                return (step < 0 ? MinPan : MaxPan);
            }
            else
            {
                return (step < 0 ? MinTilt : MaxTilt);
            }
        }

        #region IPanTiltCfg Members

        public event PtuUserLimits __UserLimitsUpdate;

        public int SetMaximumSpeed(int speed)
        {
            _maxSpeed = speed;
            return PTU.SetMaximumSpeed(speed);
        }

        public int GetLimit(PtuDirections dir, out float lim)
        {
            return PTU.GetLimit(dir, out lim);
        }

        public int SetSpeed(float speedPercent)
        {
            return SetSpeedIfNeeded(speedPercent);
        }

        public int SetLimit(PtuDirections dir, float f)
        {
            return PTU.SetLimit(dir, f);
        }

        public int SetLimits(float left, float right, float up, float down)
        {
            PTU.SetLimit(PtuDirections.PanCCW, left);
            PTU.SetLimit(PtuDirections.PanCW, right);
            PTU.SetLimit(PtuDirections.TiltUp, up);
            PTU.SetLimit(PtuDirections.TiltDown, down);
            ReadLimits();
            return 0;
        }

        public int Reset()
        {
            return PTU.Reset();
        }

        #endregion

        #region ///////     P A R S E R     ///////

        private bool TryParse(string msg, out int res)
        {
            res = 0;
            double dummy;
            if (String.IsNullOrEmpty(msg))
            {
                return false;
            }

            String[] starr = msg.Split('*');
            msg = starr[starr.Length - 1];

            foreach (string str in msg.Split(' '))
            {
                if (Double.TryParse(str, out dummy))
                {
                    res = (int)dummy;
                    return true;
                }
            }
            return false;
        }

        #endregion

    }

    class Sender
    {
        public Sender()
        {
            _sock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        }

        private Socket _sock;
        private object _locker = new object();

        public int Send(Byte[] data)
        {
            return _sock.Send(data);
        }

        public void Connect(EndPoint ipe)
        {
            _sock.Connect(ipe);
        }

        public int Available
        {
            get { return _sock.Available; }
        }

        public int Receive(Byte[] buffer)
        {
            return _sock.Receive(buffer);
        }

        public object Locker { get { return _locker; } }

    }
}
