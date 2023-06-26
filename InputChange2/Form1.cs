

using System.Runtime.InteropServices;
using System.Text;

namespace InputChange2
{
    public partial class Form1 : Form
    {
        public string ExePath
        {
            get
            {
                string path = System.AppDomain.CurrentDomain.BaseDirectory;
                return path;
            }
        }
        enum InputEnum
        {
            eng = 0,
            fulleng,
            fullhifakana,
            fullkatakana,
            halfkatakana,
        }

        public Form1()
        {
            InitializeComponent();
        }


        HttpServer httpServer = new HttpServer();

        StringBuilder sb = new StringBuilder();
        private void Form1_Load(object sender, EventArgs e)
        {
            CheckForIllegalCrossThreadCalls = false;
            httpServer.Start();

            httpServer.OnInput = new Action<string>((type) =>
            {
                sb.Clear();
                label1.Text = "";
                sb.AppendLine(type);
                // KanjiMode
                // IMENonconvert
                InputEnum inputEnum = (InputEnum)Enum.Parse(typeof(InputEnum), type);
                if (inputEnum == InputEnum.eng)
                {
                    SendInputKeyPressAndRelease(Keys.IMEConvert);
                }
                else if (inputEnum == InputEnum.fulleng)
                {
                    SendInputKeyPressAndRelease(Keys.IMEConvert);
                    SendInputKeyPressAndRelease(Keys.KanjiMode);
                    SendInputKeyPressAndRelease(Keys.IMENonconvert);
                }
                else if (inputEnum == InputEnum.fullhifakana)
                {
                    SendInputKeyPressAndRelease(Keys.IMEConvert);
                    SendInputKeyPressAndRelease(Keys.KanjiMode);
                }
                else if (inputEnum == InputEnum.fullkatakana)
                {
                    SendInputKeyPressAndRelease(Keys.IMEConvert);
                    SendInputKeyPressAndRelease(Keys.KanjiMode);
                    SendInputKeyPressAndRelease(Keys.IMENonconvert);
                }
                else if (inputEnum == InputEnum.halfkatakana)
                {
                    SendInputKeyPressAndRelease(Keys.IMEConvert);
                    SendInputKeyPressAndRelease(Keys.KanjiMode);
                    SendInputKeyPressAndRelease(Keys.IMENonconvert);
                    SendInputKeyPressAndRelease(Keys.IMENonconvert);
                }
                label1.Text = sb.ToString();
            });

        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            httpServer.Stop();

        }

        private void button1_Click(object sender, EventArgs e)
        {

        }
        private void SendInputKeyPressAndRelease(Keys key)
        {
            sb.AppendLine(key.ToString());
            Thread.Sleep(1);
            Input[] inputs = new Input[2];

            int vsc = NativeMethods.MapVirtualKey((int)key, MAPVK_VK_TO_VSC);

            inputs[0] = new Input();
            inputs[0].Type = 1; // KeyBoard = 1
            inputs[0].ui.Keyboard.VirtualKey = (short)key;
            inputs[0].ui.Keyboard.ScanCode = (short)vsc;
            inputs[0].ui.Keyboard.Flags = 0;
            inputs[0].ui.Keyboard.Time = 0;
            inputs[0].ui.Keyboard.ExtraInfo = IntPtr.Zero;

            inputs[1] = new Input();
            inputs[1].Type = 1; // KeyBoard = 1
            inputs[1].ui.Keyboard.VirtualKey = (short)key;
            inputs[1].ui.Keyboard.ScanCode = (short)vsc;
            inputs[1].ui.Keyboard.Flags = KEYEVENTF_KEYUP;
            inputs[1].ui.Keyboard.Time = 0;
            inputs[1].ui.Keyboard.ExtraInfo = IntPtr.Zero;

            NativeMethods.SendInput(inputs.Length, inputs, Marshal.SizeOf(inputs[0]));
        }

        private void timer1_Tick(object sender, EventArgs e)
        {

        }

        private static class NativeMethods
        {
            [DllImport("user32.dll", SetLastError = true)]
            public extern static void SendInput(int nInputs, Input[] pInputs, int cbsize);

            [DllImport("user32.dll", EntryPoint = "MapVirtualKeyA")]
            public extern static int MapVirtualKey(int wCode, int wMapType);

            //[DllImport("user32.dll", SetLastError = true)]
            //public extern static IntPtr GetMessageExtraInfo();
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct POINT
        {
            public int x;
            public int y;
        }

        [StructLayout(LayoutKind.Sequential)]
        struct MouseInput
        {
            public int X;
            public int Y;
            public int Data;
            public int Flags;
            public int Time;
            public IntPtr ExtraInfo;
        }

        [StructLayout(LayoutKind.Sequential)]
        struct KeyboardInput
        {
            public short VirtualKey;
            public short ScanCode;
            public int Flags;
            public int Time;
            public IntPtr ExtraInfo;
        }

        [StructLayout(LayoutKind.Sequential)]
        struct HardwareInput
        {
            public int uMsg;
            public short wParamL;
            public short wParamH;
        }

        [StructLayout(LayoutKind.Sequential)]
        struct Input
        {
            public int Type;
            public InputUnion ui;
        }

        [StructLayout(LayoutKind.Explicit)]
        struct InputUnion
        {
            [FieldOffset(0)]
            public MouseInput Mouse;
            [FieldOffset(0)]
            public KeyboardInput Keyboard;
            [FieldOffset(0)]
            public HardwareInput Hardware;
        }

        private const int KEYEVENTF_EXTENDEDKEY = 0x0001;
        private const int KEYEVENTF_KEYUP = 0x0002;
        private const int KEYEVENTF_SCANCODE = 0x0008;
        private const int KEYEVENTF_UNICODE = 0x0004;

        private const int MAPVK_VK_TO_VSC = 0;

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine(e.KeyValue);
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            SendInputKeyPressAndRelease(Keys.IMEConvert);
        }
    }
}