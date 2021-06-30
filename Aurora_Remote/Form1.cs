using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Management;
using System.IO;

namespace Aurora_Remote
{

    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            Display.setDisplay(85, 20, 10);
            DisplayPictureBox.Size = new Size(Display.Width, Display.Height);
            this.Controls.Add(DisplayPictureBox);
            DisplayBG();
    
            TimerPTT_RTS.Tick += TimerPTT_RTS_of;
            TimerRadio.Tick += TimerRadio_of;
            TimerRadio.Interval = 5000;

        }


        private void button_byte_a_Click(object sender, EventArgs e)
        {

            byte[] inputString = new byte[] { 0x00, 0x00, 0x00, 0x00 };

            inputString[0] = byte.Parse(textBox_byte_0a.Text, NumberStyles.AllowHexSpecifier);
            inputString[1] = byte.Parse(textBox_byte_1a.Text, NumberStyles.AllowHexSpecifier);
            inputString[2] = byte.Parse(textBox_byte_2a.Text, NumberStyles.AllowHexSpecifier);
            inputString[3] = byte.Parse(textBox_byte_3a.Text, NumberStyles.AllowHexSpecifier);

            SerialPort1.Write(inputString, 0, inputString.Length);

            if (checkBox_Print.Checked)
            {
                DebugLogTextInsert("button_byte_a: ", BitConverter.ToString(inputString, 0, inputString.Length).Replace("-", " "));
            }
        }
        private void button_byte_b_Click(object sender, EventArgs e)
        {

            byte[] inputString = new byte[] { 0x00, 0x00, 0x00, 0x00 };

            inputString[0] = byte.Parse(textBox_byte_0b.Text, NumberStyles.AllowHexSpecifier);
            inputString[1] = byte.Parse(textBox_byte_1b.Text, NumberStyles.AllowHexSpecifier);
            inputString[2] = byte.Parse(textBox_byte_2b.Text, NumberStyles.AllowHexSpecifier);
            inputString[3] = byte.Parse(textBox_byte_3b.Text, NumberStyles.AllowHexSpecifier);

            SerialPort1.Write(inputString, 0, inputString.Length);

            if (checkBox_Print.Checked)
            {
                DebugLogTextInsert("button_byte_b: ", BitConverter.ToString(inputString, 0, inputString.Length).Replace("-", " "));
            }
        }


        private void UpdateDisplay2(int _start, int[] _array)
        {
            int pos = 0;
            int row = 0;

            var list = new List<int> { 89, 95, 99, 101, 103, 113, 119, 129, 137, 143, 145, 147, 149, 153, 155, 161, 167 };

            int arrWidth = _array.Length;

            int start = 0;
            int end = 0;
            bool pic = false;


            Bitmap newDisplayPicture = new Bitmap(DisplayPictureBox.Image);
            Graphics displayPicture = Graphics.FromImage(newDisplayPicture);

            for (int aw = 0; aw < arrWidth; aw++)
            {
                if (aw <= 2)
                {
                    pos = _array[1];
                    row = _array[2];
                }
                if (aw >= 3 && aw < row + 3)
                {
                    var arr = new BitArray(BitConverter.GetBytes(_array[aw]));

                    int bit = 8;

                    var intVar = pos + aw - 3;

                    if (list.Contains(intVar))
                    {
                        bit = 7;
                    }

                    for (int b = 0; b < bit; b++)
                    {
                        if (arr[b] == true)
                        { displayPicture.FillRectangle(Brushes.Green, Display.getPosXY(pos + aw - 3, b, "x"), Display.getPosXY(pos + aw - 3, b, "y"), Display.Dot, Display.Dot); }
                        else
                        { displayPicture.FillRectangle(Brushes.White, Display.getPosXY(pos + aw - 3, b, "x"), Display.getPosXY(pos + aw - 3, b, "y"), Display.Dot, Display.Dot); }

                    }

                }

            }

            DisplayPictureBox.Image = newDisplayPicture;

            for (int aw = 0; aw < arrWidth; aw++)
            {
                if (aw <= 2)
                {
                    pos = _array[1];
                    row = _array[2];
                }
                if (aw >= 3 && aw < row + 3)
                {
                    var arr = new BitArray(BitConverter.GetBytes(_array[aw]));
                    var intVar = pos + aw - 3;

                    if (list.Contains(intVar))
                    {

                        start = 0;
                        end = 0;
                        pic = arr[7];

                        if (intVar == 89) { start = 0; end = 60; }
                        if (intVar == 95) { start = 60; end = 125; }
                        if (intVar == 99) { start = 125; end = 160; }
                        if (intVar == 101) { start = 160; end = 200; }
                        if (intVar == 103) { start = 200; end = 240; }
                        if (intVar == 113) { start = 240; end = 315; }
                        if (intVar == 119) { start = 315; end = 410; }
                        if (intVar == 129) { start = 410; end = 480; }
                        if (intVar == 137) { start = 480; end = 525; }
                        if (intVar == 143) { start = 525; end = 600; }
                        if (intVar == 145) { start = 600; end = 623; }
                        if (intVar == 147) { start = 623; end = 666; }
                        if (intVar == 149) { start = 666; end = 700; }
                        if (intVar == 153) { start = 700; end = 775; }
                        if (intVar == 155) { start = 775; end = 835; }
                        if (intVar == 161) { start = 835; end = 875; }
                        if (intVar == 167) { start = 875; end = 920; }

                        UpdateDisplay3(start, end, pic);

                    }

                }

            }

        }
        private void UpdateDisplay3(int _start, int _end, bool _show)
        {
            int pos = _start + 8;

            Bitmap newDisplayPicture = new Bitmap(DisplayPictureBox.Image);
            Graphics displayPicture = Graphics.FromImage(newDisplayPicture);

            for (int aw = 0; aw < (_end - _start); aw++)
            {
                for (int ah = 0; ah < 8; ah++)
                {

                    var arr = new BitArray(BitConverter.GetBytes(aurora_icons.topIcons[_start + aw, ah]));

                    for (int b = 0; b < 8; b++)
                    {
                        if (arr[b] == true && _show)
                        { displayPicture.FillRectangle(Brushes.Green, pos + aw, b + ah * 8, 1, 1); }
                        else
                        { displayPicture.FillRectangle(Brushes.White, pos + aw, b + ah * 8, 1, 1); }

                    }


                }

            }

            DisplayPictureBox.Image = newDisplayPicture;
        }



        private void DisplayBG() { 

            Bitmap displayPicture = new Bitmap(Display.Width, Display.Height);
            Graphics displayBackGround = Graphics.FromImage(displayPicture);

            displayBackGround.FillRectangle(Brushes.Gray, 0, 0, Display.Width, Display.Height);
            displayBackGround.FillRectangle(Brushes.White, 0, 0, Display.Width, 65);

            for (int h = 66; h < Display.Height + 1; h+=(Display.Dot+1))
            {
                for (int w = 1; w < Display.Width; w+= (Display.Dot + 1))
                {
                        displayBackGround.FillRectangle(Brushes.White, w, h, Display.Dot, Display.Dot);
                    
                }
            }

            DisplayPictureBox.Image = displayPicture;
        }


        private void DebugLogTextInsert(string program, string text)
        {
            if (InvokeRequired)
            {
                this.Invoke(new Action<string,string>(DebugLogTextInsert), new object[] { program, text });
                return;
            }
           
            textBox1.Text = textBox1.Text.Insert(0, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " - " + program + " : " + text + "\r\n");
            DebugLogRemoveLines(50);
        }

        private void DebugLogRemoveLines(int rows)
        {

            if (textBox1.Lines.Length > rows)
            {
                textBox1.Lines = textBox1.Lines.Take(rows).ToArray();
            }
        }

        public static SerialPort SerialPort1 = new SerialPort();
        public static BackgroundWorker BackgroundWorker1 = new BackgroundWorker();
        
        public static SerialPort SerialPort2 = new SerialPort();
        public static BackgroundWorker BackgroundWorker2 = new BackgroundWorker();

        public static System.Windows.Forms.Timer TimerPTT_RTS = new System.Windows.Forms.Timer();
        public static System.Windows.Forms.Timer TimerRadio = new System.Windows.Forms.Timer();

        private void TimerPTT_RTS_of(object sender, EventArgs e)
        {
            // Send a PTT of via RTS
            this.FunctionPttRts(false, "Timer");
            TimerPTT_RTS.Enabled = false;
        }
        private void TimerRadio_of(object sender, EventArgs e)
        {
            RadioStatus(false);
            TimerRadio.Enabled = false;
        }
        private void comportUpdate(string program)
        {

            ComPortNumber1.Items.Clear();
            ComPortNumber2.Items.Clear();

            ComPortNumber1.Sorted = true;
            ComPortNumber2.Sorted = true;

            ComPortNumber1.Items.AddRange(SerialPort.GetPortNames());
            ComPortNumber2.Items.AddRange(SerialPort.GetPortNames());


            ManagementObjectSearcher searcher = new ManagementObjectSearcher("root\\cimv2",
                "SELECT * FROM Win32_PnPEntity where Caption is not null");

            foreach (ManagementObject CNCA in searcher.Get())
            {
                if (CNCA["DeviceID"].ToString().IndexOf("CNCA") > 0)
                {
                    foreach (ManagementObject CNCB in searcher.Get())
                    {
                        try
                        {
                            if (CNCB["DeviceID"].ToString().IndexOf("CNCB") > 0 && CNCA["DeviceID"].ToString().Substring(17) == CNCB["DeviceID"].ToString().Substring(17))
                            {
                                int startA = CNCA["Caption"].ToString().IndexOf("(COM") + 1;
                                int startB = CNCB["Caption"].ToString().IndexOf("(COM") + 1;
                                int endA = CNCA["Caption"].ToString().IndexOf(")", startA);
                                int endB = CNCB["Caption"].ToString().IndexOf(")", startB);
                                DebugLogTextInsert("com0com", CNCA["Caption"].ToString().Substring(startA, endA - startA) + " - " + CNCB["Caption"].ToString().Substring(startB, endB - startB));
                            }
                        }
                        catch (Exception)
                        {

                            throw;
                        }


                    }
                }

            }

            foreach (ManagementObject queryObj in searcher.Get())
            {
                if ((queryObj["Caption"].ToString().IndexOf("(COM") > 0) && (queryObj["Caption"].ToString().IndexOf("com0com") < 0))
                {
                    DebugLogTextInsert("Serial interface", queryObj["Caption"].ToString());
                }
            }


            DebugLogTextInsert(program, "Check Comport");
        }
        private void ComPortConnect1_Click(object sender, EventArgs e)
        {
            try
            {
                SaveSettings();

                SerialPort1.BaudRate = 9600;
                SerialPort1.PortName = ComPortNumber1.Text;
                SerialPort1.Open();
                SerialPort1.DiscardInBuffer();
                SerialPort1.DiscardOutBuffer();

                BackgroundWorker1.RunWorkerAsync();



                ComPortConnect1.Enabled = false;
                ComPortDisconnect1.Enabled = true;
                ComPortConnect1b.Enabled = false;
                ComPortDisconnect1b.Enabled = true;

                vpp_0v.Enabled = true;
                vpp_12v.Enabled = true;
                vpp_13v.Enabled = true;

                vpp_0v.Checked = true;

                SerialPort1.RtsEnable = false;
                SerialPort1.DtrEnable = false;

                ComPortNumber1.Enabled = false;

                SerialPort_value.status_BreakState1 = SerialPort1.BreakState;
                SerialPort_value.status_CDHolding1 = SerialPort1.CDHolding;
                SerialPort_value.status_CtsHolding1 = SerialPort1.CtsHolding;
                SerialPort_value.status_DsrHolding1 = SerialPort1.DsrHolding;

            }
            catch (Exception ex)
            {
                if (SerialPort1.IsOpen) SerialPort1.Close();

                ComPortConnect1.Enabled = true;
                ComPortDisconnect1.Enabled = false;
                ComPortConnect1b.Enabled = true;
                ComPortDisconnect1b.Enabled = false;

                vpp_0v.Enabled = false;
                vpp_12v.Enabled = false;
                vpp_13v.Enabled = false;

                ComPortNumber1.Enabled = true;

                Debug.WriteLine("ComPortConnect1_Click: " + ex.Message);
                MessageBox.Show(ex.Message, "ComPortError " + SerialPort_value.port_name1);
            }
        }
        private void ComPortDisconnect1_Click(object sender, EventArgs e)
        {

            if (BackgroundWorker1.IsBusy)
            {
                BackgroundWorker1.CancelAsync();

                while (BackgroundWorker1.IsBusy)
                {
                    Application.DoEvents();
                    System.Threading.Thread.Sleep(20);
                }

            }

            vpp_0v.Enabled = false;
            vpp_12v.Enabled = false;
            vpp_13v.Enabled = false;

            vpp_0v.Checked = true;

            SerialPort1.RtsEnable = false;
            SerialPort1.DtrEnable = false;

            SerialPort1.Close();
            ComPortConnect1.Enabled = true;
            ComPortDisconnect1.Enabled = false;
            ComPortConnect1b.Enabled = true;
            ComPortDisconnect1b.Enabled = false;

            ComPortNumber1.Enabled = true;

        }
        private static void BackgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            var buffer = new byte[4096];

            while (!BackgroundWorker1.CancellationPending)
            {
                try
                {
                    if (SerialPort1.IsOpen)
                    {
                        var c = SerialPort1.Read(buffer, 0, buffer.Length);
                        BackgroundWorker1.ReportProgress(0, new SerialPort_Data() { Data_sp1 = buffer.Take(c).ToArray() });
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("BackgroundWorker1_DoWork: " + ex.Message);
                }
            }

        }
 
        private void ComPortConnect2_Click(object sender, EventArgs e)
        {
            try
            {
                SerialPort2.BaudRate = 9600;
                SerialPort2.PortName = ComPortNumber2.Text;
                
                SerialPort2.DtrEnable = setDTR2.Checked;
                SerialPort2.RtsEnable = setRTS2.Checked;
                
                SerialPort2.Open();
                SerialPort2.DiscardInBuffer();
                SerialPort2.DiscardOutBuffer();

                BackgroundWorker2.RunWorkerAsync();



                ComPortConnect2.Enabled = false;
                ComPortDisconnect2.Enabled = true;
                ComPortNumber2.Enabled = false;

                SerialPort_value.status_BreakState2 = SerialPort2.BreakState;
                SerialPort_value.status_CDHolding2 = SerialPort2.CDHolding;
                SerialPort_value.status_CtsHolding2 = SerialPort2.CtsHolding;
                SerialPort_value.status_DsrHolding2 = SerialPort2.DsrHolding;


            }
            catch (Exception ex)
            {
                if (SerialPort2.IsOpen) SerialPort2.Close();

                ComPortConnect2.Enabled = true;
                ComPortDisconnect2.Enabled = false;

                ComPortNumber2.Enabled = true;

                Debug.WriteLine("ComPortConnect2_Click: " + ex.Message);
                MessageBox.Show(ex.Message, "ComPortError " + SerialPort_value.port_name2);
            }
        }
        private void ComPortDisconnect2_Click(object sender, EventArgs e)
        {

            if (BackgroundWorker2.IsBusy)
            {
                BackgroundWorker2.CancelAsync();

                while (BackgroundWorker2.IsBusy)
                {
                    Application.DoEvents();
                    System.Threading.Thread.Sleep(20);
                }

            }

            SerialPort2.Close();
            ComPortConnect2.Enabled = true;
            ComPortDisconnect2.Enabled = false;

            ComPortNumber2.Enabled = true;

        }
        public static void Wait(int milliseconds)
        {
            var timer1 = new System.Windows.Forms.Timer();
            if (milliseconds == 0 || milliseconds < 0) return;

            timer1.Interval = milliseconds;
            timer1.Enabled = true;
            timer1.Start();

            timer1.Tick += (s, e) =>
            {
                timer1.Enabled = false;
                timer1.Stop();
            };

            while (timer1.Enabled)
            {
                Application.DoEvents();
            }
        }
        private static void BackgroundWorker2_DoWork(object sender, DoWorkEventArgs e)
        {
            var buffer = new byte[4096];

            while (!BackgroundWorker2.CancellationPending )
                {
                    try
                    {
                    if (SerialPort2.IsOpen)
                    {
                        var    c = SerialPort2.Read(buffer, 0, buffer.Length);
                        BackgroundWorker2.ReportProgress(0, new SerialPort_Data() { Data_sp2 = buffer.Take(c).ToArray() });

                    }
                }
                catch (Exception ex)
                    {
                    Debug.WriteLine("BackgroundWorker2_DoWork: " + ex.Message);
                    ;
                }
               
            }

        }
        static class SerialPort_value
        {
            public static string port_name1 = "Radio";
            public static string port_name2 = "Program 1";

            public static bool status_BreakState1 = false;
            public static bool status_CDHolding1 = false;
            public static bool status_CtsHolding1 = false;
            public static bool status_DsrHolding1 = false;

            public static bool status_BreakState2 = false;
            public static bool status_CDHolding2 = false;
            public static bool status_CtsHolding2 = false;
            public static bool status_DsrHolding2 = false;

            public static int status_receiving1;

            public static byte[] data_receiving1 = new byte[0];
            public static int[] data_receiving1int = new int[0];
            public static byte[] data_receiving2 = new byte[0];
            public static int[] data_receiving2int = new int[0];
        }
        public class SerialPort_Data
        {
            public byte[] Data_sp1 { get; set; }
            public byte[] Data_sp2 { get; set; }
            public byte[] Data_sp3 { get; set; }
            public byte[] Data_sp4 { get; set; }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            ToolTip ToolTip1 = new ToolTip();
            ToolTip1.SetToolTip(this.com0comButton, "com0com");
            ToolTip1.SetToolTip(this.SoundButton, "Sound control");
            ToolTip1.SetToolTip(this.DeviceButton, "Device manager");
            ToolTip1.SetToolTip(this.Taskmgr_button, "Task manager");

            comportUpdate("Start");

            SerialPort1.PinChanged += new SerialPinChangedEventHandler(SerialPort1_PinChanged);
            SerialPort2.PinChanged += new SerialPinChangedEventHandler(SerialPort2_PinChanged);

            SerialPort1.ReadTimeout = SerialPort1.WriteTimeout = 1000;
            SerialPort2.ReadTimeout = SerialPort2.WriteTimeout = 1000;

            BackgroundWorker1.WorkerSupportsCancellation = true;
            BackgroundWorker1.WorkerReportsProgress = true;
            BackgroundWorker1.DoWork += BackgroundWorker1_DoWork;
            BackgroundWorker1.ProgressChanged += BackgroundWorker1_ProgressChanged;

            BackgroundWorker2.WorkerSupportsCancellation = true;
            BackgroundWorker2.WorkerReportsProgress = true;
            BackgroundWorker2.DoWork += BackgroundWorker2_DoWork;
            BackgroundWorker2.ProgressChanged += BackgroundWorker2_ProgressChanged;

            this.Text = Version.NameAndNumber;
            GetSettings();

        }

        private void SaveSettings()
        {
                try
                {

                    Properties.Settings.Default.ComPortNumber1 = ComPortNumber1.Text;
                    Properties.Settings.Default.ComPortNumber2 = ComPortNumber2.Text;
                    Properties.Settings.Default.Ptt_timeout = TimerPTT_RTS.Interval / 1000;

                    Properties.Settings.Default.Save();
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("SaveSettings: " + ex.Message);
                    MessageBox.Show(ex.Message, "SaveSettings");
                }
            //}

        }
        private void GetSettings()
        {
            if (true == true)
            {
                try
                {

                    ComPortNumber1.Text = global::Aurora_Remote.Properties.Settings.Default.ComPortNumber1;
                    ComPortNumber2.Text = global::Aurora_Remote.Properties.Settings.Default.ComPortNumber2;

                    if (global::Aurora_Remote.Properties.Settings.Default.Ptt_timeout != 0)
                    {
                        textBox_Ptt_timeout.Text = global::Aurora_Remote.Properties.Settings.Default.Ptt_timeout.ToString();
                        TimerPTT_RTS.Interval = global::Aurora_Remote.Properties.Settings.Default.Ptt_timeout * 1000;
                    }
                    else
                    {
                        textBox_Ptt_timeout.Text = "300";
                        TimerPTT_RTS.Interval = 300 * 1000;
                    }



                }
                catch (Exception ex)
                {
                    Debug.WriteLine("GetSettings: " + ex.Message);
                    MessageBox.Show(ex.Message, "GetSettings");
                }
            }

        }

        private void BackgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            var sp = e.UserState as SerialPort_Data;

            if (DumpData1.Checked && sp.Data_sp1.Length > 0)
            {
                DebugLogTextInsert(SerialPort_value.port_name1, "DumpData: " + BitConverter.ToString(sp.Data_sp1, 0, sp.Data_sp1.Length).Replace("-", " "));

            }

            for (int i = 0; i < sp.Data_sp1.Length; i++)
            {

                int clear = 0;

                if (sp.Data_sp1[i].ToString("x2").Equals("ff")
                        && SerialPort_value.data_receiving1.Length > 3
                        && SerialPort_value.data_receiving1[0] == 255
                        && SerialPort_value.data_receiving1[2] >= 17
                        )
                {
                    SerialPort_value.status_receiving1 = 0;
                    DebugLogTextInsert("Miss: ", BitConverter.ToString(SerialPort_value.data_receiving1, 0, SerialPort_value.data_receiving1.Length).Replace("-", " "));
                    Array.Clear(SerialPort_value.data_receiving1, 0, SerialPort_value.data_receiving1.Length);
                    Array.Resize(ref SerialPort_value.data_receiving1, 0);
                    Array.Clear(SerialPort_value.data_receiving1int, 0, SerialPort_value.data_receiving1int.Length);
                    Array.Resize(ref SerialPort_value.data_receiving1int, 0);

                }



                Array.Resize(ref SerialPort_value.data_receiving1, SerialPort_value.data_receiving1.Length + 1);
                SerialPort_value.data_receiving1[SerialPort_value.data_receiving1.Length - 1] = sp.Data_sp1[i];
                Array.Resize(ref SerialPort_value.data_receiving1int, SerialPort_value.data_receiving1int.Length + 1);
                SerialPort_value.data_receiving1int[SerialPort_value.data_receiving1int.Length - 1] = sp.Data_sp1[i];



                if (SerialPort_value.data_receiving1int[0] != 255
                    && SerialPort_value.data_receiving1int.Length >= 2)
                {
                    SerialPort_value.status_receiving1 = 0;

                    if (checkBox_Ack.Checked)
                    {
                        DebugLogTextInsert("WD: ", BitConverter.ToString(SerialPort_value.data_receiving1, 0, SerialPort_value.data_receiving1.Length).Replace("-", " "));
                    }

                    clear++;

                }



                if (SerialPort_value.data_receiving1int[0].ToString("x2").Equals("ff"))
                {
                    if (SerialPort_value.data_receiving1int.Length >= 3 && SerialPort_value.data_receiving1.Length == SerialPort_value.data_receiving1int[2] + 3)
                    {
                        SerialPort_value.status_receiving1 = 0;
                        UpdateDisplay2(0, SerialPort_value.data_receiving1int);

                        if (checkBox_Print.Checked)
                        {
                            DebugLogTextInsert("Print: ", BitConverter.ToString(SerialPort_value.data_receiving1, 0, SerialPort_value.data_receiving1.Length).Replace("-", " "));
                        }

                        clear++;
                    }

                    if (SerialPort_value.data_receiving1int.Length >= 3
                        && SerialPort_value.data_receiving1int[1].ToString("x2").Equals("e7")
                        )
                    {

                        byte[] displayBackLight = new byte[] { 0x21, 0x02, 0x83, 0x83 };
                        displayBackLight[3] = SerialPort_value.data_receiving1[2];
                        SerialPort1.Write(displayBackLight, 0, displayBackLight.Length);


                        if (SerialPort_value.data_receiving1int[2].ToString("x2").Equals("ef"))
                        {
                            // Light up redlamp
                            RadioStatus(true, true);
                        }
                        else
                        {
                            // BackLight level
                            RadioStatus(true);
                        }



                        SerialPort_value.status_receiving1 = 0;

  
                        if (checkBox_Light.Checked)
                        {
                            DebugLogTextInsert("displayBackLight: ", BitConverter.ToString(SerialPort_value.data_receiving1, 0, SerialPort_value.data_receiving1.Length).Replace("-", " "));
                        }

                        clear++;
                    }
                }


                if (clear >= 1)
                {
                    Array.Clear(SerialPort_value.data_receiving1, 0, SerialPort_value.data_receiving1.Length);
                    Array.Resize(ref SerialPort_value.data_receiving1, 0);
                    Array.Clear(SerialPort_value.data_receiving1int, 0, SerialPort_value.data_receiving1int.Length);
                    Array.Resize(ref SerialPort_value.data_receiving1int, 0);
                }

            }
        }
        private void BackgroundWorker2_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            var sp = e.UserState as SerialPort_Data;

            if (DumpData2.Checked && sp.Data_sp2.Length>0)
            {
                DebugLogTextInsert(SerialPort_value.port_name2, "DumpData: "  + BitConverter.ToString(sp.Data_sp2, 0, sp.Data_sp2.Length).Replace("-", " "));

            }

                    Array.Clear(SerialPort_value.data_receiving2, 0, SerialPort_value.data_receiving2.Length);
                    Array.Resize(ref SerialPort_value.data_receiving2, 0);
                    Array.Clear(SerialPort_value.data_receiving2int, 0, SerialPort_value.data_receiving2int.Length);
                    Array.Resize(ref SerialPort_value.data_receiving2int, 0);
        }

        private void SerialPort1_PinChanged(object sender, SerialPinChangedEventArgs e)
        {
            if (InvokeRequired)
            {
                this.Invoke(new Action<object, SerialPinChangedEventArgs>(SerialPort1_PinChanged), new object[] { sender, e });
                return;
            }
            
            if (e.EventType == SerialPinChange.Break && PinChange1.Checked && SerialPort_value.status_BreakState1 != SerialPort1.BreakState)
            {
                this.DebugLogTextInsert(SerialPort_value.port_name1, "BreakState:" + SerialPort1.BreakState.ToString());
                SerialPort_value.status_BreakState1 = SerialPort1.BreakState;
            }

            if (e.EventType == SerialPinChange.CDChanged && PinChange1.Checked && SerialPort_value.status_CDHolding1 != SerialPort1.CDHolding)
            {
                this.DebugLogTextInsert(SerialPort_value.port_name1, "CDHolding:" + SerialPort1.CDHolding.ToString());
                SerialPort_value.status_CDHolding1 = SerialPort1.CDHolding;
            }

            if (e.EventType == SerialPinChange.CtsChanged && PinChange1.Checked && SerialPort_value.status_CtsHolding1 != SerialPort1.CtsHolding)
            {
                this.DebugLogTextInsert(SerialPort_value.port_name1, "CtsHolding:" + SerialPort1.CtsHolding.ToString());
                SerialPort_value.status_CtsHolding1 = SerialPort1.CtsHolding;
            }

            if (e.EventType == SerialPinChange.DsrChanged && PinChange1.Checked && SerialPort_value.status_DsrHolding1 != SerialPort1.DsrHolding)
            {
                this.DebugLogTextInsert(SerialPort_value.port_name1, "DsrHolding:" + SerialPort1.DsrHolding.ToString());
                SerialPort_value.status_DsrHolding1 = SerialPort1.DsrHolding;
            }

            if (e.EventType == SerialPinChange.Ring && PinChange1.Checked)
            {
                this.DebugLogTextInsert(SerialPort_value.port_name1, "Ring:?");
            }

            if (e.EventType == SerialPinChange.CtsChanged && CTS2RTS1.Checked && SerialPort2.IsOpen)
            {
                SerialPort2.RtsEnable = SerialPort1.CtsHolding;
                setRTS2.Checked = SerialPort1.CtsHolding;
            }

            if (e.EventType == SerialPinChange.DsrChanged && DSR2DTR1.Checked && SerialPort2.IsOpen)
            {
                SerialPort2.DtrEnable = SerialPort1.DsrHolding;
                setDTR2.Checked = SerialPort1.DsrHolding;
            }

        }
        private void SerialPort2_PinChanged(object sender, SerialPinChangedEventArgs e)
        {
            if (InvokeRequired)
            {
                this.Invoke(new Action<object, SerialPinChangedEventArgs>(SerialPort2_PinChanged), new object[] { sender, e });
                return;
            }

            if (e.EventType == SerialPinChange.Break && PinChange2.Checked && SerialPort_value.status_BreakState2 != SerialPort2.BreakState)
            {
                this.DebugLogTextInsert(SerialPort_value.port_name2, "BreakState:" + SerialPort2.BreakState.ToString());
                SerialPort_value.status_BreakState2 = SerialPort2.BreakState;
            }

            if (e.EventType == SerialPinChange.CDChanged && PinChange2.Checked && SerialPort_value.status_CDHolding2 != SerialPort2.CDHolding)
            {
                this.DebugLogTextInsert(SerialPort_value.port_name2, "CDHolding:" + SerialPort2.CDHolding.ToString());
                SerialPort_value.status_CDHolding2 = SerialPort2.CDHolding;
            }

            if (e.EventType == SerialPinChange.CtsChanged && PinChange2.Checked && SerialPort_value.status_CtsHolding2 != SerialPort2.CtsHolding)
            {
                this.DebugLogTextInsert(SerialPort_value.port_name2, "CtsHolding:" + SerialPort2.CtsHolding.ToString());
                SerialPort_value.status_CtsHolding2 = SerialPort2.CtsHolding;
            }

            if (e.EventType == SerialPinChange.DsrChanged && PinChange2.Checked && SerialPort_value.status_DsrHolding2 != SerialPort2.DsrHolding)
            {
                this.DebugLogTextInsert(SerialPort_value.port_name2, "DsrHolding:" + SerialPort2.DsrHolding.ToString());
                SerialPort_value.status_DsrHolding2 = SerialPort2.DsrHolding;
            }

            if (e.EventType == SerialPinChange.Ring && PinChange2.Checked)
            {
                this.DebugLogTextInsert(SerialPort_value.port_name2, "Ring:?");
            }


            if (e.EventType == SerialPinChange.CtsChanged && RTS2PTT.Checked)
            {
                FunctionPttRts(SerialPort2.CtsHolding, SerialPort_value.port_name2);
            }


            if (e.EventType == SerialPinChange.CtsChanged && CTS2RTS2.Checked && SerialPort1.IsOpen)
            {
                SerialPort1.RtsEnable = SerialPort2.CtsHolding;
                setRTS1.Checked = SerialPort2.CtsHolding;
            }

            if (e.EventType == SerialPinChange.DsrChanged && DSR2DTR2.Checked && SerialPort1.IsOpen)
            {
                SerialPort1.DtrEnable = SerialPort2.DsrHolding;
                setDTR1.Checked = SerialPort2.DsrHolding;
            }
        }
        private void FunctionPttRts(Boolean button, string sender)
        {

            byte[] pttOn = new byte[] { 0x21, 0x02, 0x00, 0x95 };
            byte[] pttOff = new byte[] { 0x21, 0x02, 0x00, 0x00 };

            RadioStatus(true, button);

            Invoke((MethodInvoker)delegate
            {
                if (SerialPort1.IsOpen)
                {
                    if (button)
                    {
                        button_PTT.BackColor = Color.Red;
                        SerialPort1.RtsEnable = true;
                        TimerPTT_RTS.Enabled = true;
                        keySend(pttOn);
                        DebugLogTextInsert(sender, "PTT/RTS: on");
                    }
                    else
                    {
                        button_PTT.BackColor = default(Color);
                        button_PTT.UseVisualStyleBackColor = true;
                        SerialPort1.RtsEnable = false;
                        TimerPTT_RTS.Enabled = false;
                        keySend(pttOff);
                        DebugLogTextInsert(sender, "PTT/RTS: off");
                    }

                }

            });


        }

        private void RadioStatus(Boolean _status, Boolean _send=false)
        {
            if (checkBox_Status.Checked)
            {

            
            if (InvokeRequired)
            {
                this.Invoke(new Action<Boolean, Boolean>(RadioStatus), new object[] { _status, _send });
                return;
            }

            bool _radioStatus = TimerRadio.Enabled;
            bool _comStatus = SerialPort1.IsOpen;
            bool _rtsStatus = SerialPort1.RtsEnable;

            if (_comStatus && _rtsStatus)
                {
                    powerStatus.Text = "PTT";
                    powerStatus.ForeColor = Color.Red;
                    powerSend(true);
                    _status = true;
                }
            else if (_comStatus && _status && _send)
            {
                powerStatus.Text = "TXD";
                powerStatus.ForeColor = Color.Green;
            }
            else if (!_comStatus && _status && _send)
            {
                powerStatus.Text = "ERROR";
                powerStatus.ForeColor = Color.Orange;
                DebugLogTextInsert("ERROR: ", "Check comport!");
            }
                else if (_comStatus && _status && !_send)
                {
                    powerStatus.Text = "ON";
                    powerStatus.ForeColor = Color.Green;
                    powerSend(true);
                }
                else
                {
                    DisplayBG();
                    powerStatus.Text = "OFF";
                    powerStatus.ForeColor = Color.Black;
                    powerSend(false);
                }
            }
            TimerRadio.Enabled = false;
            TimerRadio.Enabled = _status;
      
        }
        private void keySend (byte[] _key)
        {
            RadioStatus(true, true);

            if (SerialPort1.IsOpen)
            {
                SerialPort1.Write(_key, 0, _key.Length);

                if (checkBox_Print.Checked)
                {
                    DebugLogTextInsert("KeySend: ", BitConverter.ToString(_key, 0, _key.Length).Replace("-", " "));
                }
            }

        }

        private void powerSend(bool _status)
        {
            bool _preStatus = TimerRadio.Enabled;
            bool _comStatus = SerialPort1.IsOpen;

            if (_comStatus && _status && _status != _preStatus)
            {
                byte[] keyOn = new byte[] { 0x00, 0x21, 0x02, 0x85 };
                SerialPort1.Write(keyOn, 0, keyOn.Length);
                
                if (checkBox_Print.Checked)
                {
                    DebugLogTextInsert("PowerSend: ", BitConverter.ToString(keyOn, 0, keyOn.Length).Replace("-", " "));
                }
            }
            else if (_status != _preStatus)
            {
                DisplayBG();
            }

        }

        private void button_PowerON_Click(object sender, EventArgs e)
        {
            RadioStatus(true, true); 
            
            if (SerialPort1.IsOpen)
            {
                SerialPort1.DtrEnable = true;
                Thread.Sleep(700);
                SerialPort1.DtrEnable = false;
            }
            }
        private void button_PowerOFF_Click(object sender, EventArgs e)
        {
            byte[] keyDown = new byte[] { 0x21, 0x02, 0x08, 0xff };
            keySend(keyDown);
            Thread.Sleep(2000);

            byte[] keyUp = new byte[] { 0x21, 0x02, 0x00, 0xff };
            keySend(keyUp);
        }


        private void button_micSw1_Click(object sender, EventArgs e)
        {
            byte[] keyPress = new byte[] { 0x21, 0x02, 0x01, 0xff, 0x21, 0x02, 0x00, 0xff };
            keySend(keyPress);
        }
        private void button_EM_Click(object sender, EventArgs e)
        {
            byte[] keyPress = new byte[] { 0x21, 0x02, 0x10, 0xff, 0x21, 0x02, 0x00, 0xff };
            keySend(keyPress);
        }
        private void button_ServiceMode_Click(object sender, EventArgs e)
        {
            /*
            The radio normally display the sign on message for about 3 seconds and DEFORE this message
            disappear, press and hold BOTH the ON/OFF and the RED ALARM bottom.
            Enter the code ’10011704’ followed by “Ent” . The radio has now entered service mode.
            */
            byte[] keyPress = new byte[] { 0x21, 0x02, 0x18, 0xff, 0x21, 0x02, 0x00, 0xff };
            keySend(keyPress);
        }

        private void button_A_Click(object sender, EventArgs e)
        {
            byte[] keyPress = new byte[] { 0x21, 0x02, 0x00, 0x83, 0x21, 0x02, 0x00, 0x00 };
            keySend(keyPress);
        }
        private void button_B_Click(object sender, EventArgs e)
        {
            byte[] keyPress = new byte[] { 0x21, 0x02, 0x00, 0x82, 0x21, 0x02, 0x00, 0x00 };
            keySend(keyPress);
        }
        private void button_C_Click(object sender, EventArgs e)
        {
            byte[] keyPress = new byte[] { 0x21, 0x02, 0x00, 0x81, 0x21, 0x02, 0x00, 0x00 };
            keySend(keyPress);
        }
        private void button_D_Click(object sender, EventArgs e)
        {
            byte[] keyPress = new byte[] { 0x21, 0x02, 0x00, 0x80, 0x21, 0x02, 0x00, 0x00 };
            keySend(keyPress);
        }
        private void button_1_Click(object sender, EventArgs e)
        {
            byte[] keyPress = new byte[] { 0x21, 0x02, 0x00, 0x87, 0x21, 0x02, 0x00, 0x00 };
            keySend(keyPress);
        }
        private void button_2_Click(object sender, EventArgs e)
        {
            byte[] keyPress = new byte[] { 0x21, 0x02, 0x00, 0x86, 0x21, 0x02, 0x00, 0x00 };
            keySend(keyPress);
        }
        private void button_3_Click(object sender, EventArgs e)
        {
            byte[] keyPress = new byte[] { 0x21, 0x02, 0x00, 0x85, 0x21, 0x02, 0x00, 0x00 };
            keySend(keyPress);
        }
        private void button_4_Click(object sender, EventArgs e)
        {
            byte[] keyPress = new byte[] { 0x21, 0x02, 0x00, 0x8B, 0x21, 0x02, 0x00, 0x00 };
            keySend(keyPress);
        }
        private void button_5_Click(object sender, EventArgs e)
        {
            byte[] keyPress = new byte[] { 0x21, 0x02, 0x00, 0x8A, 0x21, 0x02, 0x00, 0x00 };
            keySend(keyPress);
        }
        private void button_6_Click(object sender, EventArgs e)
        {
            byte[] keyPress = new byte[] { 0x21, 0x02, 0x00, 0x89, 0x21, 0x02, 0x00, 0x00 };
            keySend(keyPress);
        }
        private void button_7_Click(object sender, EventArgs e)
        {
            byte[] keyPress = new byte[] { 0x21, 0x02, 0x00, 0x8f, 0x21, 0x02, 0x00, 0x00 };
            keySend(keyPress);
        }
        private void button_8_Click(object sender, EventArgs e)
        {
            byte[] keyPress = new byte[] { 0x21, 0x02, 0x00, 0x8e, 0x21, 0x02, 0x00, 0x00 };
            keySend(keyPress);
        }
        private void button_9_Click(object sender, EventArgs e)
        {
            byte[] keyPress = new byte[] { 0x21, 0x02, 0x00, 0x8d, 0x21, 0x02, 0x00, 0x00 };
            keySend(keyPress);
        }
        private void button_0_Click(object sender, EventArgs e)
        {
            byte[] keyPress = new byte[] { 0x21, 0x02, 0x00, 0x8c, 0x21, 0x02, 0x00, 0x00 };
            keySend(keyPress);
        }
        private void button_Star_Click(object sender, EventArgs e)
        {
            byte[] keyPress = new byte[] { 0x21, 0x02, 0x00, 0x93, 0x21, 0x02, 0x00, 0x00 };
            keySend(keyPress);
        }
        private void button_Hash_Click(object sender, EventArgs e)
        {
            byte[] keyPress = new byte[] { 0x21, 0x02, 0x00, 0x91, 0x21, 0x02, 0x00, 0x00 };
            keySend(keyPress);
        }
        private void button_M_Click(object sender, EventArgs e)
        {
            byte[] keyPress = new byte[] { 0x21, 0x02, 0x00, 0x90, 0x21, 0x02, 0x00, 0x00 };
            keySend(keyPress);
        }
        private void button_S_Click(object sender, EventArgs e)
        {
            byte[] keyPress = new byte[] { 0x21, 0x02, 0x00, 0x92, 0x21, 0x02, 0x00, 0x00 };
            keySend(keyPress);
        }
        private void button_HGT_Click(object sender, EventArgs e)
        {
            byte[] keyPress = new byte[] { 0x21, 0x02, 0x00, 0x84, 0x21, 0x02, 0x00, 0x00 };
            keySend(keyPress);
        }
        private void button_ENT_Click(object sender, EventArgs e)
        {
            byte[] keyPress = new byte[] { 0x21, 0x02, 0x00, 0x88, 0x21, 0x02, 0x00, 0x00 };
            keySend(keyPress);
        }
        private void button_UP_Click(object sender, EventArgs e)
        {
            byte[] keyPress = new byte[] { 0x21, 0x02, 0x00, 0x99, 0x21, 0x02, 0x00, 0x00 };
            keySend(keyPress);
        }
        private void button_Down_Click(object sender, EventArgs e)
        {
            byte[] keyPress = new byte[] { 0x21, 0x02, 0x00, 0x9A, 0x21, 0x02, 0x00, 0x00 };
            keySend(keyPress);
        }



        private void button_PTT_KeyDown(object sender, KeyEventArgs e)
        {
            this.FunctionPttRts(true, "Button");
        }
        private void button_PTT_KeyUp(object sender, KeyEventArgs e)
        {
            this.FunctionPttRts(false, "Button");
        }
        private void button_PTT_MouseDown(object sender, MouseEventArgs e)
        {
            this.FunctionPttRts(true, "Button");
        }
        private void button_PTT_MouseUp(object sender, MouseEventArgs e)
        {
            this.FunctionPttRts(false, "Button");
        }


        private void setDTR_CheckedChanged(object sender, EventArgs e)
        {
            if (!DSR2DTR1.Checked)
            {
                SerialPort2.DtrEnable = setDTR2.Checked;
            }

        }
        private void setDTR1_CheckedChanged(object sender, EventArgs e)
        {
            if (!DSR2DTR2.Checked)
            {
                SerialPort1.DtrEnable = setDTR1.Checked;
            }
        }

        private void setRTS_CheckedChanged(object sender, EventArgs e)
        {
            if (!CTS2RTS1.Checked)
            {
                SerialPort2.RtsEnable = setRTS2.Checked;
            }
        }
        private void setRTS1_CheckedChanged(object sender, EventArgs e)
        {
            if (!CTS2RTS2.Checked)
            {
                SerialPort1.RtsEnable = setRTS1.Checked;
            }
        }

        private void TrashButton_Click(object sender, EventArgs e)
        {
            DebugLogRemoveLines(0);
        }
        private void RefreshButton_Click(object sender, EventArgs e)
        {
            comportUpdate("Button");
        }

        private void vpp_0v_CheckedChanged(object sender, EventArgs e)
        {
            if (vpp_13v.Checked)
            {
                SerialPort1.RtsEnable = true;
                SerialPort1.DtrEnable = false;
            }
            else if (vpp_12v.Checked)
            {
                SerialPort1.RtsEnable = false;
                SerialPort1.DtrEnable = true;

            }
            else
            {
                SerialPort1.RtsEnable = false;
                SerialPort1.DtrEnable = false;

            }

        }

        private void com0comButton_Click(object sender, EventArgs e)
        {
            string pathx86 = Environment.GetEnvironmentVariable("ProgramFiles(x86)") + @"\com0com\setupg.exe";
            string pathx64 = Environment.GetEnvironmentVariable("ProgramFiles") + @"\com0com\setupg.exe";

            if (File.Exists(pathx86)) { Process.Start(pathx86); }
            else if (File.Exists(pathx64)) { Process.Start(pathx64); }
            else MessageBox.Show("Cannot find com0com, maybe it not installed?", "com0com");
        }
        private void SoundButton_Click(object sender, EventArgs e)
        {
            Process.Start("mmsys.cpl");
        }
        private void DeviceButton_Click(object sender, EventArgs e)
        {
            Process.Start("devmgmt.msc");
        }
        private void Taskmgr_button_Click(object sender, EventArgs e)
        {
            Process.Start("Taskmgr.exe");

        }

        private void button_ON_KeyDown(object sender, KeyEventArgs e)
        {

            if (SerialPort1.IsOpen && !TimerRadio.Enabled)
            {
                RadioStatus(true);
                SerialPort1.DtrEnable = true;
                Thread.Sleep(700);
                SerialPort1.DtrEnable = false;
                RadioStatus(true);

            }
            else
            {
                byte[] keyDown = new byte[] { 0x21, 0x02, 0x08, 0xff };
                keySend(keyDown);
            }
        }
        private void button_ON_KeyUp(object sender, KeyEventArgs e)
        {
            byte[] keyUp = new byte[] { 0x21, 0x02, 0x00, 0xff };
            keySend(keyUp);
            
        }
        private void button_ON_MouseDown(object sender, MouseEventArgs e)
        {
            if (SerialPort1.IsOpen && !TimerRadio.Enabled)
            {
                RadioStatus(true);
                SerialPort1.DtrEnable = true;
                Thread.Sleep(700);
                SerialPort1.DtrEnable = false;

            }
            else
            {
                byte[] keyDown = new byte[] { 0x21, 0x02, 0x08, 0xff };
                keySend(keyDown);
            }


        }
        private void button_ON_MouseUp(object sender, MouseEventArgs e)
        {
            byte[] keyUp = new byte[] { 0x21, 0x02, 0x00, 0xff };
            keySend(keyUp);

        }
    }


    static class Display
    {
        public static int Width;
        public static int Height;
        public static int Dot;

        public static void setDisplay(int _Width, int _Height,int _Dot)
        {
            Width = _Width * (_Dot + 1) + 1;
            Height = (_Height) * (_Dot + 1) + 1 + 65;
            Dot = _Dot;
        }
        public static int getPosXY(int _posX, int _posY, string _xy)
        {
            int tempPos = 0;

            if (_posY == 7) { _posY++; }
            
            if (_posX >= 84) 
            { 
                _posX -= 84;
                _posY += 10;
            }

            if (_xy == "x") { tempPos = (_posX) * (Dot + 1) + 1; }
            if (_xy == "y") { tempPos = (_posY) * (Dot + 1) + 1 + 65; }

            

            return tempPos;
        }
    }

}
