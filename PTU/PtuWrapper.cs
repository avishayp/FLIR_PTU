using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Net;
using System.Net.Sockets;
using System.ComponentModel;
using System.Reflection;
using System.Threading;
using HWGlobals;

namespace PTU
{

    interface IPTUWrapper
    {
        int Init(Object data);
        int Reset();
        int SetLimit(PtuDirections dir, float val);
        int GetLimit(PtuDirections dir, out float val);
        void ResetLimits();
        int SetSpeed(int speed);
        int SetMaximumSpeed(int speed);
        int MoveAbsPositionPan(float pos);
        int MoveAbsPositionTilt(float pos);
        int MoveAbsolute(float pan, float tilt);
        void StopAll();
        float CurrentPosPan { get; }
        float CurrentPosTilt { get; }
        int JogAbsPositionPan(float target);
        int JogAbsPositionTilt(float target);
        int JogRelative(int ax, int offset);

        int SendMessage(OPCODES opcode);
        int SendMessage(OPCODES opcode, int arg);

        void Poll(int interval);

        bool IsInLimits(int ax, float pos);            
    }

    interface IBytesProvider
    {
        String GetOpCodeBytes(OPCODES command);
        String GetOpCodeBytes(OPCODES command, int arg);
    }

    public enum OPCODES
    {
        ILEGAL = -1,
        NONE,
        PAN_POSITION,
        TILT_POSITION,
        PAN_RESOLUTION,
        TILT_RESOLUTION,
        HALT_ALL,
        HALT_PAN,
        HALT_TILT,
        AWAIT,
        ENCODERS_CORRECTION_CONTROL,
        PAN_SPEED,
        TILT_SPEED,
        PAN_UPPER_SPEED,
        TILT_UPPER_SPEED,
        PAN_ACCELERATION,
        TILT_ACCELERATION,
        RESET,
        PAN_MOVE_POWER_HIGH,
        TILT_MOVE_POWER_HIGH,
        ENABLE_USER_LIMITS,
        DISABLE_LIMITS,
        LIMITS_ENFORCEMENT_QUERY,
        PAN_MAX_USER_LIMIT,
        PAN_MIN_USER_LIMIT,
        TILT_MAX_USER_LIMIT,
        TILT_MIN_USER_LIMIT,
        ECHO_ENABLE,
        ECHO_DISABLE,
        REPORT_VERBOSE,
        REPORT_TERSE,
        UNIFIED_POSITION_SPEED
    }

    public class OPCODE_ASCII : IBytesProvider
    {
        private OPCODE_ASCII() { Init(); }
        private static OPCODE_ASCII _instance;
        public static OPCODE_ASCII Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new OPCODE_ASCII();
                }
                return _instance;
            }
        }

        private void Init()
        {
            _asciiCodes = new Dictionary<OPCODES, string>();
            InitDictionaries();
        }

        private Dictionary<OPCODES, String> _asciiCodes;

        private void InitDictionaries()
        {
            _asciiCodes[OPCODES.PAN_POSITION] = "PP";
            _asciiCodes[OPCODES.TILT_POSITION] = "TP";
            _asciiCodes[OPCODES.PAN_RESOLUTION] = "PR";
            _asciiCodes[OPCODES.TILT_RESOLUTION] = "TR";
            _asciiCodes[OPCODES.HALT_ALL] = "H";
            _asciiCodes[OPCODES.HALT_PAN] = "HP";
            _asciiCodes[OPCODES.HALT_TILT] = "HT";
            _asciiCodes[OPCODES.AWAIT] = "A";
            _asciiCodes[OPCODES.ENCODERS_CORRECTION_CONTROL] = "CEC";
            _asciiCodes[OPCODES.PAN_SPEED] = "PS";
            _asciiCodes[OPCODES.TILT_SPEED] = "TS";
            _asciiCodes[OPCODES.PAN_UPPER_SPEED] = "PU";
            _asciiCodes[OPCODES.TILT_UPPER_SPEED] = "TU";
            _asciiCodes[OPCODES.PAN_ACCELERATION] = "PA";
            _asciiCodes[OPCODES.TILT_ACCELERATION] = "TA";
            _asciiCodes[OPCODES.RESET] = "RE";
            _asciiCodes[OPCODES.PAN_MOVE_POWER_HIGH] = "PMH";
            _asciiCodes[OPCODES.TILT_MOVE_POWER_HIGH] = "TMH";
            _asciiCodes[OPCODES.ENABLE_USER_LIMITS] = "LU";
            _asciiCodes[OPCODES.DISABLE_LIMITS] = "LD";
            _asciiCodes[OPCODES.LIMITS_ENFORCEMENT_QUERY] = "L";   // limits on/off
            _asciiCodes[OPCODES.PAN_MAX_USER_LIMIT] = "PXU";
            _asciiCodes[OPCODES.PAN_MIN_USER_LIMIT] = "PNU";
            _asciiCodes[OPCODES.TILT_MAX_USER_LIMIT] = "TXU";
            _asciiCodes[OPCODES.TILT_MIN_USER_LIMIT] = "TNU";

            _asciiCodes[OPCODES.ECHO_ENABLE] = "EE";
            _asciiCodes[OPCODES.ECHO_DISABLE] = "ED";
            _asciiCodes[OPCODES.REPORT_VERBOSE] = "FV";
            _asciiCodes[OPCODES.REPORT_TERSE] = "FT";

            _asciiCodes[OPCODES.UNIFIED_POSITION_SPEED] = "B";
        }

        public String GetOpCodeBytes(OPCODES opcode)
        {
            String cmd = String.Empty;
            if (_asciiCodes.TryGetValue(opcode, out cmd))
            {
                cmd += Environment.NewLine;
            }
            return cmd;
        }

        public String GetOpCodeBytes(OPCODES opcode, int arg)
        {
            String cmd = String.Empty;
            if (_asciiCodes.TryGetValue(opcode, out cmd))
            {
                cmd += arg.ToString();
                cmd += Environment.NewLine;
            }
            return cmd;
        }
    }

    public class PtuEthernetWrapper : IPTUWrapper
    {
        private IBytesProvider _provider = OPCODE_ASCII.Instance;
        private Sender _sock;
        Timer _timerHeartbit;

        public void Poll(int interval)
        {
            _timerHeartbit = new Timer(GetPositionFunc, null, interval, interval);
        }

        private int _resolutionPan, _resolutionTilt, _lastPan, _lastTilt;
        private float _limitLeft, _limitRight, _limitUp, _limitDown;

        private void GetPositionFunc(Object dummy)
        {
            SyncPositionPoll();
        }

        public bool IsInLimits(int ax, float pos)
        {
            float min = (ax == 0 ? _limitLeft : _limitDown);
            float max = (ax == 0 ? _limitRight : _limitUp);

            return ((pos > min) && (pos < max));
        }

        public int SendMessage(OPCODES opcode)
        {
            return SendString(_provider.GetOpCodeBytes(opcode));
        }

        public int SendMessage(OPCODES opcode, int arg)
        {
            return SendString(_provider.GetOpCodeBytes(opcode, arg));
        }

        public String PtuOpcode(OPCODES opcode)
        {
            return _provider.GetOpCodeBytes(opcode);
        }

        public String PtuOpcode(OPCODES opcode, int data)
        {
            return _provider.GetOpCodeBytes(opcode, data);
        }

        private Byte[] FromString(String str)
        {
            return String.IsNullOrEmpty(str) ? null : Array.ConvertAll<char, Byte>(str.ToCharArray(), Convert.ToByte);
        }

        private String ToString(Byte[] stream)
        {
            return (stream == null ? String.Empty : new String(Array.ConvertAll<Byte, char>(stream, Convert.ToChar)));
        }

        private int SendString(String data)
        {
            int sendAns = 0;

            if (!String.IsNullOrEmpty(data) && IsInitialized)
            {
                lock (_sock.Locker)
                {
                    try
                    {
                        sendAns = Send(data);
                    }
                    catch { }
                }
            }
            return sendAns;
        }

        private void Wait()
        {
            Wait(20);
        }

        private void Wait(int t)
        {
            Thread.Sleep(t);
        }

        private bool Receive(out int report)
        {
            report = 0;
            int recCount = 0;
            if (_sock.Available > 0)
            {
                byte[] data = new byte[_sock.Available];
                lock (_sock.Locker)
                {
                    recCount = _sock.Receive(data);
                }
                if (recCount > 0)
                    return TryParseData(data, out report);
            }
            return false;
        }

        private int Send(String str)
        {
            return _sock.Send(FromString(str));
        }

        private bool SyncSendReceive(OPCODES opcode, out int result)
        { 
            result = 0;
            int recCount;
            String query = _provider.GetOpCodeBytes(opcode);

            if (String.IsNullOrEmpty(query) || !IsInitialized)
            {
                return false;
            }

            try
            {
                lock (_sock.Locker)
                {
                    if (Send(query) > 0)
                    {
                        Wait();
                        // receive:
                        if (_sock.Available > 0)
                        {
                            byte[] data = new byte[_sock.Available];
                            recCount = _sock.Receive(data);
                            if (recCount > 0)
                            {
                                return TryParseData(data, out result);
                            }
                        }
                    }
                }
            }
            catch
            {
            }
            return false;
        }

        private bool TryParseData(Byte[] data, out int report)
        {
            report = 0;
            double d;
            String s = GetLastCommand(data);

            if (Double.TryParse(s, out d))
            {
                report = (int)d;
                return true;
            }
            return false;
        }

        private string GetLastCommand(byte[] data)
        {
            String raw = ToString(data);
            if (!String.IsNullOrEmpty(raw))
            {
                String[] starr = raw.Split("*".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                if (starr.Length > 0)
                {
                    return starr[starr.Length - 1];
                }
            }
            return String.Empty;
        }

        private bool IsInitialized { set; get; }

        #region IPTUWrapper Members

        public int Init(object data)
        {
            _sock = (Sender)data;
            IsInitialized = true;
            GetResolution();
            GetUserLimits();
            return 1;
        }

        private void GetUserLimits()
        {
            int res;
            if (SyncSendReceive(OPCODES.PAN_MAX_USER_LIMIT, out res))
            {
                _limitRight = ToDegs(0, res);
            }
            if (SyncSendReceive(OPCODES.PAN_MIN_USER_LIMIT, out res))
            {
                _limitLeft = ToDegs(0, res);
            }
            if (SyncSendReceive(OPCODES.TILT_MAX_USER_LIMIT, out res))
            {
                _limitUp = ToDegs(1, res);
            }
            if (SyncSendReceive(OPCODES.TILT_MIN_USER_LIMIT, out res))
            {
                _limitDown = ToDegs(1, res);
            }
        }

        private void GetResolution()
        {
            int res;
            if (SyncSendReceive(OPCODES.PAN_RESOLUTION, out res))
            {
                _resolutionPan = res;
            }
            if (SyncSendReceive(OPCODES.TILT_RESOLUTION, out res))
            {
                _resolutionTilt = res;
            }
        }

        private void SyncPositionPoll()
        {
            String query = _provider.GetOpCodeBytes(OPCODES.UNIFIED_POSITION_SPEED);
            Byte[] stream = FromString(query);

            try
            {
                lock (_sock.Locker)
                {
                    _sock.Send(stream);
                    Wait();
                    // receive:
                    if (_sock.Available > 0)
                    {
                        byte[] data = new byte[_sock.Available];
                        if (_sock.Receive(data) > 0)
                        {
                            ParseUnifiedPosition(data);
                        }
                    }
                }
            }
            catch
            {
            }
        }

        private void ParseUnifiedPosition(byte[] raw)
        {
            int ans;
            String[] res = GetLastCommand(raw).Split(" ".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

            if (res != null && res.Length > 0)
            {
                if (int.TryParse(res[0], out ans))
                {
                    _lastPan = ans;
                }

                if (res.Length > 1)
                {
                    if (int.TryParse(res[1], out ans))
                    {
                        _lastTilt = ans;
                    }
                }
            }
        }

        public int Reset()
        {
            String cmd = _provider.GetOpCodeBytes(OPCODES.RESET) + _provider.GetOpCodeBytes(OPCODES.AWAIT);
            return SendString(cmd);
        }

        public int SetLimit(PtuDirections dir, float val)
        {
            switch (dir)
            {
                case PtuDirections.PanCCW:
                    SetLeftLimit(val);
                    break;
                case PtuDirections.PanCW:
                    SetRightLimit(val);
                    break;
                case PtuDirections.TiltUp:
                    SetUpLimit(val);
                    break;
                case PtuDirections.TiltDown:
                    SetDownLimit(val);
                    break;
            }
            Await();
            return 0;
        }

        public int GetLimit(PtuDirections dir, out float lim)
        {
            lim = 0;
            switch (dir)
            {
                case PtuDirections.PanCCW:
                    lim = _limitLeft;
                    break;
                case PtuDirections.PanCW:
                    lim = _limitRight;
                    break;
                case PtuDirections.TiltUp:
                    lim = _limitUp;
                    break;
                case PtuDirections.TiltDown:
                    lim = _limitDown;
                    break;
            }
            return 0;
        }

        private void Await()
        {
            SendString(_provider.GetOpCodeBytes(OPCODES.AWAIT));
        }

        private int SetLeftLimit(float left)
        {
            if (SendString(_provider.GetOpCodeBytes(OPCODES.PAN_MIN_USER_LIMIT, ToSteps(0, left))) > 0)
            {
                _limitLeft = left;
            }
            return 0;
        }

        private int SetRightLimit(float right)
        {
            if (SendString(_provider.GetOpCodeBytes(OPCODES.PAN_MAX_USER_LIMIT, ToSteps(0, right))) > 0)
            {
                _limitRight = right;
            }
            return 0;
        }

        private int SetUpLimit(float up)
        {
            if (SendString(_provider.GetOpCodeBytes(OPCODES.TILT_MAX_USER_LIMIT, ToSteps(1, up))) > 0)
            {
                _limitUp = up;
            }
            return 0;
        }

        private int SetDownLimit(float down)
        {
            if (SendString(_provider.GetOpCodeBytes(OPCODES.TILT_MIN_USER_LIMIT, ToSteps(1, down))) > 0)
            {
                _limitDown = down;
            }
            return 0;
        }

        public void ResetLimits()
        {
            SendString(_provider.GetOpCodeBytes(OPCODES.DISABLE_LIMITS));
        }

        public int SetSpeed(int speed)
        {
            String cmd = _provider.GetOpCodeBytes(OPCODES.PAN_SPEED, speed)
                + _provider.GetOpCodeBytes(OPCODES.TILT_SPEED, speed);

            return SendString(cmd);
        }

        public int SetMaximumSpeed(int speed)
        {
            String cmd = _provider.GetOpCodeBytes(OPCODES.PAN_UPPER_SPEED, speed)
                + _provider.GetOpCodeBytes(OPCODES.TILT_UPPER_SPEED, speed);

            return SendString(cmd);
        }

        public int MoveAbsPositionPan(float pos)
        {
            return SendString(_provider.GetOpCodeBytes(OPCODES.PAN_POSITION, ToSteps(0, pos)));
        }

        public int MoveAbsPositionTilt(float pos)
        {
            return SendString(_provider.GetOpCodeBytes(OPCODES.TILT_POSITION, ToSteps(1, pos)));
        }

        public int MoveAbsolute(float pan, float tilt)
        {
            String cmd = _provider.GetOpCodeBytes(OPCODES.PAN_POSITION, ToSteps(0, pan))
                + _provider.GetOpCodeBytes(OPCODES.TILT_POSITION, ToSteps(1, tilt));
            return SendString(cmd);
        }

        public void StopAll()
        {
            SendString(_provider.GetOpCodeBytes(OPCODES.HALT_ALL));
        }

        public float CurrentPosPan
        {
            get { return ToDegs(0, _lastPan); }
        }

        public float CurrentPosTilt
        {
            get { return ToDegs(1, _lastTilt); }
        }

        public int JogAbsPositionPan(float target)
        {
            return MoveAbsPositionPan(target);
        }

        public int JogAbsPositionTilt(float target)
        {
            return MoveAbsPositionTilt(target);
        }

        public int JogRelative(int ax, int step)
        {
            return (ax == 0 ? SendString(_provider.GetOpCodeBytes(OPCODES.PAN_POSITION, _lastPan + step)) : SendString(_provider.GetOpCodeBytes(OPCODES.TILT_POSITION, _lastTilt + step)));
        }

        #endregion

        private int ResolutionPan
        { // 1 click = _resolution arc seconds
            get
            {
                if (_resolutionPan == 0)
                {
                    GetResolutionPan();
                }
                return _resolutionPan;
            }
        }

        private void GetResolutionPan()
        {
            int res;
            if (SyncSendReceive(OPCODES.PAN_RESOLUTION, out res))
            {
                _resolutionPan = res;
            }
        }

        private void GetResolutionTilt()
        {
            int res;
            if (SyncSendReceive(OPCODES.TILT_RESOLUTION, out res))
            {
                _resolutionTilt = res;
            }
        }

        private int ResolutionTilt
        { // 1 click = _resolution arc seconds
            get
            {
                if (_resolutionTilt == 0)
                {
                    GetResolutionTilt();
                }
                return _resolutionTilt;
            }
        }

        private float ToDegs(int ax, int clicks)
        {
            return UnitsConvertor.ToDegs(clicks, AxResolution(ax));
        }

        private int ToSteps(int ax, float degs)
        {
            return UnitsConvertor.ToClicks(degs, AxResolution(ax));
        }

        private int AxResolution(int ax)
        {
            return (ax == 0 ? ResolutionPan : ResolutionTilt);
        }
    }

    public static class UnitsConvertor
    {
        public static float ToDegs(int clicks, int resolution)
        {
            return (clicks * resolution * SECONDS_TO_DEGS);
        }

        public static int ToClicks(float degs, int resolution)
        { 
            return (int)(degs / (resolution * SECONDS_TO_DEGS));
        }

        private const float SECONDS_TO_DEGS = 0.0002778f;
    }

}
