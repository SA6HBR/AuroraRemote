using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
//using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO.Ports;
using System.Linq;
//using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Management;
using System.IO;

namespace Aurora_Remote
{

    public partial class Form1 : Form
    {
        private void Form1_Load(object sender, EventArgs e)
        {
            ToolTip ToolTip1 = new ToolTip();
            ToolTip1.SetToolTip(this.com0comButton, "com0com");
            ToolTip1.SetToolTip(this.SoundButton, "Sound control");
            ToolTip1.SetToolTip(this.DeviceButton, "Device manager");
            ToolTip1.SetToolTip(this.Taskmgr_button, "Task manager");

            ComPortListUpdate("Start");

            SerialPort1BackgroundWorkerData.WorkerSupportsCancellation = true;
            SerialPort1BackgroundWorkerData.WorkerReportsProgress = true;
            SerialPort1BackgroundWorkerData.DoWork += SerialPort1BackgroundWorkerData_DoWork;
            SerialPort1BackgroundWorkerData.ProgressChanged += SerialPort1BackgroundWorkerData_ProgressChanged;

            SerialPort2.PinChanged += new SerialPinChangedEventHandler(SerialPort2_PinChanged);

            this.Text = Version.NameAndNumber;
            GetSettings();

        }
        public Form1()
        {
            InitializeComponent();
            Display.SetDisplay(85, 20, 10);
            DisplayPictureBox.Size = new Size(Display.Width, Display.Height);
            this.Controls.Add(DisplayPictureBox);
            DisplayClear();
    
            TimerPTT.Tick += TimerPTToff;

            TimerRadio.Tick += TimerRadioOff;
            TimerRadio.Interval = 2000;

            TimerRadioAlive.Tick += TimerRadioAliveCheck;
            TimerRadioAlive.Interval = 5000;

            //Hide tabs. DoubleClick on logo to activate them
            tabControl1.TabPages.Remove(tabPage3);
            tabControl1.TabPages.Remove(tabPage4);
            tabControl1.TabPages.Remove(tabPage5);
        }
        private void SaveSettings()
        {
            try
            {
                Properties.Settings.Default.ComPortNumber1 = ComPort1Number.Text;
                Properties.Settings.Default.ComPortNumber2 = ComPort2Number.Text;
                Properties.Settings.Default.Ptt_timeout = TimerPTT.Interval / 1000;
                Properties.Settings.Default.Save();
            }
            catch (Exception ex)
            {
                Debug.WriteLine("SaveSettings: " + ex.Message);
                MessageBox.Show(ex.Message, "SaveSettings");
            }
        }
        private void GetSettings()
        {
                try
                {
                    ComPort1Number.Text = global::Aurora_Remote.Properties.Settings.Default.ComPortNumber1;
                    ComPort2Number.Text = global::Aurora_Remote.Properties.Settings.Default.ComPortNumber2;

                    if (global::Aurora_Remote.Properties.Settings.Default.Ptt_timeout != 0)
                    {
                        textBox_Ptt_timeout.Text = global::Aurora_Remote.Properties.Settings.Default.Ptt_timeout.ToString();
                        TimerPTT.Interval = global::Aurora_Remote.Properties.Settings.Default.Ptt_timeout * 1000;
                    }
                    else
                    {
                        textBox_Ptt_timeout.Text = "300";
                        TimerPTT.Interval = 300 * 1000;
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("GetSettings: " + ex.Message);
                    MessageBox.Show(ex.Message, "GetSettings");
                }
        }

        public static System.Windows.Forms.Timer TimerPTT = new System.Windows.Forms.Timer();
        public static System.Windows.Forms.Timer TimerRadio = new System.Windows.Forms.Timer();
        public static System.Windows.Forms.Timer TimerRadioAlive = new System.Windows.Forms.Timer();
        private void ComPortListUpdate(string program)
        {
            ComPort1Number.Items.Clear();
            ComPort2Number.Items.Clear();

            ComPort1Number.Sorted = true;
            ComPort2Number.Sorted = true;

            ComPort1Number.Items.AddRange(SerialPort.GetPortNames());
            ComPort2Number.Items.AddRange(SerialPort.GetPortNames());

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

        #region FormBody
        private void ButtonDown(object sender, MouseEventArgs e)
        {
            Button btn = (Button)sender;
            string btnText = btn.Text;
            ButtonActions(btnText,"Down");
        }
        private void ButtonDown(object sender, KeyEventArgs e)
        {
            Button btn = (Button)sender;
            string btnText = btn.Text;
            ButtonActions(btnText, "Down");
        }
        private void ButtonUp(object sender, MouseEventArgs e)
        {
            Button btn = (Button)sender;
            string btnText = btn.Text;
            ButtonActions(btnText, "Up");
        }
        private void ButtonUp(object sender, KeyEventArgs e)
        {
            Button btn = (Button)sender;
            string btnText = btn.Text;
            ButtonActions(btnText, "Up");
        }
        private void LogoDoubleClick(object sender, EventArgs e)
        {
            if (tabControl1.TabCount > 2)
            {
                tabControl1.TabPages.Remove(tabPage3);
                tabControl1.TabPages.Remove(tabPage4);
                tabControl1.TabPages.Remove(tabPage5);
            }
            else
            {
                tabControl1.TabPages.Add(tabPage3);
                tabControl1.TabPages.Add(tabPage4);
                tabControl1.TabPages.Add(tabPage5);
            }
        }
        #endregion FormBody

        #region FormDisplay
        private void UpdateDisplay(byte[] _array)
        {
            if (_array.Length >= 4)
            {
                int pos = _array[1] + 1;
                int row = _array[2];

                var list = new List<int> { 89, 95, 99, 101, 103, 113, 119, 129, 137, 143, 145, 147, 149, 153, 155, 161, 167 };

                Bitmap newDisplayPicture = new Bitmap(DisplayPictureBox.Image);
                Graphics displayPicture = Graphics.FromImage(newDisplayPicture);

                for (int aw = 0; aw < row; aw++)
                {
                    var intVar = pos + aw - 1;
                    var arr = new BitArray(BitConverter.GetBytes(_array[aw + 3]));

                    int bit = 8;

                    if (list.Contains(intVar))
                    {
                        bit = 7;
                    }

                    for (int b = 0; b < bit; b++)
                    {
                        if (arr[b] == true)
                        { displayPicture.FillRectangle(Brushes.Green, Display.GetPosXY(intVar, b, "x"), Display.GetPosXY(intVar, b, "y"), Display.Dot, Display.Dot); }
                        else
                        { displayPicture.FillRectangle(Brushes.White, Display.GetPosXY(intVar, b, "x"), Display.GetPosXY(intVar, b, "y"), Display.Dot, Display.Dot); }
                    }
                }
                DisplayPictureBox.Image = newDisplayPicture;
            }
        }
        private void UpdateIcons(byte[] _array)
        {
            if (_array.Length >= 4)
            {
                int pos = _array[1] + 1;
                int rows = _array[2];

                for (int row = 0; row < rows; row++)
                {

                    int start = -1;
                    int end = -1;

                    switch (pos + row - 1)
                    {
                        case 89:
                            start = 0; end = 60; break;
                        case 95:
                            start = 60; end = 125; break;
                        case 99:
                            start = 125; end = 160; break;
                        case 101:
                            start = 160; end = 200; break;
                        case 103:
                            start = 200; end = 240; break;
                        case 113:
                            start = 240; end = 315; break;
                        case 119:
                            start = 315; end = 410; break;
                        case 129:
                            start = 410; end = 480; break;
                        case 137:
                            start = 480; end = 525; break;
                        case 143:
                            start = 525; end = 600; break;
                        case 145:
                            start = 600; end = 623; break;
                        case 147:
                            start = 623; end = 666; break;
                        case 149:
                            start = 666; end = 700; break;
                        case 153:
                            start = 700; end = 775; break;
                        case 155:
                            start = 775; end = 835; break;
                        case 161:
                            start = 835; end = 875; break;
                        case 167:
                            start = 875; end = 920; break;
                    }

                    if (start >= 0)
                    {
                        var arr = new BitArray(BitConverter.GetBytes(_array[row + 3]));
                        bool pic = arr[7];
                        Bitmap newDisplayPicture = new Bitmap(DisplayPictureBox.Image);
                        Graphics displayPicture = Graphics.FromImage(newDisplayPicture);

                        for (int aw = 0; aw < (end - start); aw++)
                        {
                            for (int ah = 0; ah < 8; ah++)
                            {

                                var arr2 = new BitArray(BitConverter.GetBytes(AuroraIcons.topIcons[start + aw, ah]));

                                for (int b = 0; b < 8; b++)
                                {
                                    if (arr2[b] == true && pic)
                                    { displayPicture.FillRectangle(Brushes.Green, start + 8 + aw, b + ah * 8, 1, 1); }
                                    else
                                    { displayPicture.FillRectangle(Brushes.White, start + 8 + aw, b + ah * 8, 1, 1); }
                                }
                            }
                        }
                        DisplayPictureBox.Image = newDisplayPicture;

                    }
                }
            }
        }
        private void DisplayClear()
        {
            Bitmap displayPicture = new Bitmap(Display.Width, Display.Height);
            Graphics displayBackGround = Graphics.FromImage(displayPicture);

            displayBackGround.FillRectangle(Brushes.Gray, 0, 0, Display.Width, Display.Height);
            displayBackGround.FillRectangle(Brushes.White, 0, 0, Display.Width, 65);

            for (int h = 66; h < Display.Height + 1; h += (Display.Dot + 1))
            {
                for (int w = 1; w < Display.Width; w += (Display.Dot + 1))
                {
                    displayBackGround.FillRectangle(Brushes.White, w, h, Display.Dot, Display.Dot);
                }
            }
            DisplayPictureBox.Image = displayPicture;
        }
        #endregion FormDisplay

        #region FormTab - Operating
        private void RefreshButton_Click(object sender, EventArgs e)
        {
            ComPortListUpdate("Button");
        }
        private void ComPort1Connect_Click(object sender, EventArgs e)
        {
           ComPort1Status(ComPortON1());
        }
        private void ComPort1Disconnect_Click(object sender, EventArgs e)
        {
            ComPort1OFF();
            ComPort1Status(false);
        }
        private void ComPort2Connect_Click(object sender, EventArgs e)
        {
            ComPort2Status(ComPortON2());
        }
        private void ComPort2Disconnect_Click(object sender, EventArgs e)
        {
            ComPort2OFF();
            ComPort2Status(false);
        }
        private void ComPort1Status(bool status)
        {
            ComPort1Connect.Enabled = !status;
            ComPort1Disconnect.Enabled = status;
            ComPort1ConnectB.Enabled = !status;
            ComPort1DisconnectB.Enabled = status;
            ComPort1Number.Enabled = !status;

            vpp_0v.Enabled = status;
            vpp_12v.Enabled = status;
            vpp_13v.Enabled = status;
            vpp_0v.Checked = true;
        }
        private void ComPort2Status(bool status)
        {
            ComPort2Connect.Enabled = !status;
            ComPort2Disconnect.Enabled = status;
            ComPort2Number.Enabled = !status;
        }
        private void Com0ComButton_Click(object sender, EventArgs e)
        {
            string pathx86 = Environment.GetEnvironmentVariable("ProgramFiles(x86)") + @"\com0com\setupg.exe";
            string pathx64 = Environment.GetEnvironmentVariable("ProgramFiles") + @"\com0com\setupg.exe";
            string path ="";

            if (File.Exists(pathx86)) { path = pathx86; }
            else if (File.Exists(pathx64)) { path = pathx64; }
            else MessageBox.Show("Cannot find com0com, maybe it not installed?", "com0com");

            try
            {
                if (path.Length > 2) {Process.Start(path);}
            }
            catch (Exception ex)
            {
                Debug.WriteLine("com0com: " + ex.Message);
                MessageBox.Show(ex.Message, "com0com");
            }
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
        private void ButtonDownPTT(object sender, KeyEventArgs e)
        {
            this.FunctionPttRts(true, "Button");
        }
        private void ButtonUpPTT(object sender, KeyEventArgs e)
        {
            this.FunctionPttRts(false, "Button");
        }
        private void ButtonDownPTT(object sender, MouseEventArgs e)
        {
            this.FunctionPttRts(true, "Button");
        }
        private void ButtonUpPTT(object sender, MouseEventArgs e)
        {
            this.FunctionPttRts(false, "Button");
        }
        private void PowerStatusTextToColor(string StatusText)
        {
            switch (StatusText)
            {
                case "PTT":
                    Com.statusColor = Color.Red;
                    break;
                case "TXD":
                    Com.statusColor = Color.Green;
                    break;
                case "ERROR":
                    Com.statusColor = Color.Orange;
                    break;
                case "ON":
                    Com.statusColor = Color.Green;
                    break;
                default:
                    Com.statusColor = Color.Black;
                    break;
            }
        }
        private void PowerStatusTextUpdate(string StatusText)
        {
            if (Com.statusText != StatusText)
            {
                if (InvokeRequired)
                {
                    this.Invoke(new Action<string>(PowerStatusTextUpdate), new object[] { StatusText });
                    return;
                }

                PowerStatusTextToColor(StatusText);
                Com.statusText = StatusText;

                powerStatus.Text = Com.statusText;
                powerStatus.ForeColor = Com.statusColor;
            }
        }
        #endregion FormTab - Operating

        #region FormTab - Log
        private void TrashButton_Click(object sender, EventArgs e)
        {
            DebugLogRemoveLines(0);
        }
        private void DebugLogTextInsert(string program, string text)
        {
            if (logActiveBox.Checked)
            {
                if (InvokeRequired)
                {
                    this.Invoke(new Action<string, string>(DebugLogTextInsert), new object[] { program, text });
                    return;
                }
                if (timeStampBox.Checked)
                {
                    textBox1.Text = textBox1.Text.Insert(0, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " - " + program + " : " + text + "\r\n");
                }
                else
                {
                    textBox1.Text = textBox1.Text.Insert(0, text + "\r\n");
                }
                DebugLogRemoveLines(int.Parse(LogLinesBox.Text));
            }
        }
        private void DebugLogRemoveLines(int rows)
        {
            if (textBox1.Lines.Length > rows)
            {
                textBox1.Lines = textBox1.Lines.Take(rows).ToArray();
            }
        }
        #endregion FormTab - Log

        #region FormTab - Settings
        private void ButtonByteA_Click(object sender, EventArgs e)
        {
            byte[] inputString = new byte[] { 0x00, 0x00, 0x00, 0x00 };

            inputString[0] = byte.Parse(textBox_byte_0a.Text, NumberStyles.AllowHexSpecifier);
            inputString[1] = byte.Parse(textBox_byte_1a.Text, NumberStyles.AllowHexSpecifier);
            inputString[2] = byte.Parse(textBox_byte_2a.Text, NumberStyles.AllowHexSpecifier);
            inputString[3] = byte.Parse(textBox_byte_3a.Text, NumberStyles.AllowHexSpecifier);

            KeySend("ButtonByteA",inputString);
        }
        private void ButtonByteB_Click(object sender, EventArgs e)
        {
            byte[] inputString = new byte[] { 0x00, 0x00, 0x00, 0x00 };

            inputString[0] = byte.Parse(textBox_byte_0b.Text, NumberStyles.AllowHexSpecifier);
            inputString[1] = byte.Parse(textBox_byte_1b.Text, NumberStyles.AllowHexSpecifier);
            inputString[2] = byte.Parse(textBox_byte_2b.Text, NumberStyles.AllowHexSpecifier);
            inputString[3] = byte.Parse(textBox_byte_3b.Text, NumberStyles.AllowHexSpecifier);

            KeySend("ButtonByteB", inputString);
        }
        #endregion FormTab - Settings

        #region FormTab - Programminginterface
        private void VPP_0v_CheckedChanged(object sender, EventArgs e)
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
        #endregion FormTab - Programminginterface

        #region Communication to Radio
        private void ButtonActions(string btnText, string btnAction)
        {
            int btnType = 0;
            switch (btnText)
            {
                case "ServiceMode":
                    btnType = 1;
                    break;
                case "êEM":
                    btnText = "EM";
                    btnType = 1;
                    break;
                case "POWER":
                    btnText = "POWER";
                    btnType = 1;

                    if (btnAction == "Up")
                    {
                        TimerRadioAlive.Enabled = TimerRadio.Enabled;
                    }
                    else
                    {
                        TimerRadioAlive.Enabled = false;
                    }

                    break;
                case "5UP":
                    btnText = "UP";
                    break;
                case "6DOWN":
                    btnText = "DOWN";
                    break;
                case "XHGT":
                    btnText = "LS";
                    break;
                case "8ENT":
                    btnText = "ENTER";
                    break;                    
            }
            string btnTextSend = btnText;

            if (btnAction == "Up")
            {
                btnTextSend = "";
            }

            byte[] buttonBytes;
            if (btnType == 0)
            {
                buttonBytes = Aurora_Remote.Display.GetButtonBytes(btnTextSend);
            }
            else
            {
                buttonBytes = Aurora_Remote.Display.GetKeyBytes(btnTextSend);
            }

//            if (btnText == "POWER" && !TimerRadio.Enabled && SerialPort1.IsOpen)
//            {
//                PowerON();
//            }
//            else
//            {
                KeySend("Button-"+ btnText, buttonBytes);
//            }
//
        }
        private void KeySend(string sender, byte[] _key)
        {
            if (SerialPort1.IsOpen)
            {
                if (sender.Length > 5 && sender.Substring(sender.Length - 5, 5) == "POWER" && !TimerRadio.Enabled)
                {
                    RadioStatus(true);
                    SerialPort1.BreakState = true;
                    Thread.Sleep(700);
                    SerialPort1.BreakState = false;
                    RadioStatus(true);
                    TimerRadioAlive.Enabled = true;
                }
                else if (TimerRadio.Enabled)
                {
                    SerialPort1.Write(_key, 0, _key.Length);
                } else
                {
                    DebugLogTextInsert(sender + ": ", "Radio off. Press On!");
                }

                if (debugBox.Checked)
                {
                    DebugLogTextInsert(sender+": ", BitConverter.ToString(_key, 0, _key.Length).Replace("-", " "));
                }
            }
            else
            {
                DebugLogTextInsert(sender + ": ", "Connection to Radio not open!");
            }
        }
        private void FunctionPttRts(Boolean button, string sender)
        {
            byte[] pttOn  = Aurora_Remote.Display.GetButtonBytes("PTT");
            byte[] pttOff = Aurora_Remote.Display.GetButtonBytes("");

            RadioStatus(true, button);

//            Invoke((MethodInvoker)delegate
//            {
                if (SerialPort1.IsOpen)
                {
                    if (button)
                    {
                        button_PTT.BackColor = Color.Red;
                        SerialPort1.RtsEnable = true;
                        TimerPTT.Enabled = true;
                        KeySend("FunctionPttRts", pttOn);
                        if (PTTLogBox.Checked)
                        {
                            DebugLogTextInsert(sender, "PTT: on");
                        }
                    }
                    else
                    {
                        button_PTT.BackColor = default;
                        button_PTT.UseVisualStyleBackColor = true;
                        SerialPort1.RtsEnable = false;
                        TimerPTT.Enabled = false;
                        KeySend("FunctionPttRts", pttOff);
                        if (PTTLogBox.Checked)
                        {
                            DebugLogTextInsert(sender, "PTT: off");
                        }
                    }

                }

//            });
        }
        private void TimerPTToff(object sender, EventArgs e)
        {
            // Send a PTT of via RTS
            this.FunctionPttRts(false, "Timer");
            TimerPTT.Enabled = false;
        }
        private void TimerRadioOff(object sender, EventArgs e)
        {
            RadioStatus(false);
            TimerRadio.Enabled = false;
        }
        private void TimerRadioAliveCheck(object sender, EventArgs e)
        {
            if (SerialPort1.IsOpen && TimerRadio.Enabled)
            {
                byte[] displayBackLight = new byte[] { 0x21, 0x02, 0x83, 0x83 };
                displayBackLight[3] = Com.statusByte;
                KeySend("Alive", displayBackLight);
            }
            TimerRadioAlive.Enabled = TimerRadio.Enabled;
        }
        #endregion Communication to Radio

        #region SerialPort1
        public static SerialPort SerialPort1 = new SerialPort();
        private bool ComPortON1()
        {
            try
            {
                SaveSettings();

                SerialPort1.ReadTimeout = 1000;
                SerialPort1.WriteTimeout = 1000;
                SerialPort1.BaudRate = 9600;
                SerialPort1.PortName = ComPort1Number.Text;
                SerialPort1.Open();
                SerialPort1.DiscardInBuffer();
                SerialPort1.DiscardOutBuffer();

                SerialPort1BackgroundWorkerData.RunWorkerAsync();

                SerialPort1.RtsEnable = false;
                SerialPort1.DtrEnable = false;

                TimerRadio.Enabled = true;
                byte[] keyOn = new byte[] { 0x00, 0x21, 0x02, 0x85 };
                KeySend("ComPortON1", keyOn);

                //RadioStatus(true);

                return true;

            }
            catch (Exception ex)
            {
                if (SerialPort1.IsOpen) SerialPort1.Close();

                Debug.WriteLine("ComPortON1: " + ex.Message);
                MessageBox.Show(ex.Message, "ComPortError " + SerialPort_value.port_name1);

                return false;

            }
        }
        private void ComPort1OFF()
        {

            if (SerialPort1BackgroundWorkerData.IsBusy)
            {
                SerialPort1BackgroundWorkerData.CancelAsync();

                while (SerialPort1BackgroundWorkerData.IsBusy)
                {
                    Application.DoEvents();
                    System.Threading.Thread.Sleep(20);
                }

            }

            SerialPort1.RtsEnable = false;
            SerialPort1.DtrEnable = false;

            SerialPort1.Close();
            RadioStatus(false);

        }

        public static BackgroundWorker SerialPort1BackgroundWorkerData = new BackgroundWorker();
        private static void SerialPort1BackgroundWorkerData_DoWork(object sender, DoWorkEventArgs e)
        {
            var buffer = new byte[4096];

            while (!SerialPort1BackgroundWorkerData.CancellationPending)
            {
                try
                {
                    if (SerialPort1.IsOpen)
                    {
                        var c = SerialPort1.Read(buffer, 0, buffer.Length);
                        SerialPort1BackgroundWorkerData.ReportProgress(0, new SerialPort_Data() { Data_sp1 = buffer.Take(c).ToArray() });
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("SerialPortBackgroundWorkerData1_DoWork: " + ex.Message);
                }
            }

        }
        private void SerialPort1BackgroundWorkerData_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            var sp = e.UserState as SerialPort_Data;
            if (comRxBox.Checked && sp.Data_sp1.Length > 0)
            {
                DebugLogTextInsert(SerialPort_value.port_name1, "Com1 RX: " + BitConverter.ToString(sp.Data_sp1, 0, sp.Data_sp1.Length).Replace("-", " "));
            }

            for (int i = 0; i < sp.Data_sp1.Length; i++)
            {
                //NEW
                if (Com.firstByte==false && sp.Data_sp1[i]== Com.startByte)
                {
                    if (Com.rxPrint == 36)
                    {
                        DebugLogTextInsert(SerialPort_value.port_name1, "Channel: " + BitConverter.ToString(Com.rxArray, 24, 1).Replace("-", " "));
                    }
                    else if (Com.rxPrint == 11 )
                    {
                        DebugLogTextInsert(SerialPort_value.port_name1, "Ref. code: " + BitConverter.ToString(Com.rxArray, 0, Com.rxPrint + 1).Replace("-", " "));
                    }
                    else if (Com.rxPrint >= 0 && Com.rxArray[1] != Convert.ToByte("0xde", 16))
                    {
                        DebugLogTextInsert(SerialPort_value.port_name1, "New code: " + BitConverter.ToString(Com.rxArray, 0, Com.rxPrint + 1).Replace("-", " "));
                    }

                    if (DumpData.Checked && Com.rxPrint >= 0) // && sp.Data_sp1.Length > 0)
                    {
                        DebugLogTextInsert(SerialPort_value.port_name1, "Dump: " + BitConverter.ToString(Com.rxArray, 0, Com.rxPrint + 1).Replace("-", " "));
                    }

                    Com.firstByte = true;
                    Com.rxPrint = -1;
                    Com.rxLength = 9;
                    Array.Clear(Com.rxArray, 0, Com.rxArray.Length);
                }

                if (Com.firstByte)
                {
                    Com.rxPrint += 1;
                    Com.rxArray[Com.rxPrint] = sp.Data_sp1[i];
                    Com.rxLength = Com.GetBytesLength(Com.rxArray, Com.rxPrint);
                }
                else
                {
                    Com.rxPrint += 1;
                    Com.rxArray[Com.rxPrint] = sp.Data_sp1[i];
                    Com.rxLength = Com.GetBytesLength(Com.rxArray, Com.rxPrint);
                }

                if (Com.firstByte && Com.rxLength <= Com.rxPrint)
                {
                    Com.lastByte = true;
                }

                if (Com.firstByte && Com.lastByte)
                {
                   // if (Com.rxPrint >= 0 && Com.rxArray[2] != Com.rxPrint-2 && Com.rxArray[1] != Convert.ToByte("0xe7", 16))
                   // {
                   //     DebugLogTextInsert(SerialPort_value.port_name1, "ER2: " + BitConverter.ToString(Com.rxArray, 0, Com.rxPrint + 1).Replace("-", " "));
                   // }

                    if (DumpData.Checked && Com.rxPrint >= 0) // && sp.Data_sp1.Length > 0)
                    {
                        //DebugLogTextInsert(SerialPort_value.port_name1, "Dump: " + BitConverter.ToString(Com.rxArray, 0, Com.rxLength + 1).Replace("-", " "));
                        DebugLogTextInsert(SerialPort_value.port_name1, "Dump: " + BitConverter.ToString(Com.rxArray, 0, Com.rxPrint + 1).Replace("-", " "));
                    }
                    Com.firstByte = false;
                    Com.lastByte = false;
                    Com.rxPrint = -1;
  //                  Com.rxLength = 9;
                    if (Com.rxLength >= 0)
                    {
                        if (Com.rxArray[1] == Com.controlByte)
                        {
                            Com.controlStatus = true;
                            Com.statusByte = Com.rxArray[2];

                            byte[] displayBackLight = new byte[] { 0x21, 0x02, 0x83, 0x83 };
                            displayBackLight[3] = Com.rxArray[2];
                       //     KeySend("new", displayBackLight);

                        }
                        else
                        {
                            //                            Com.SetByte(Com.rxArray, Com.rxPrint);
                            UpdateDisplay(Com.rxArray);
                            UpdateIcons(Com.rxArray);
                        }
                    }
                    Array.Clear(Com.rxArray, 0, Com.rxArray.Length);
                    RadioStatus(true);
                }


                //NEW
                /*
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

                                    //if (checkBox_Ack.Checked)
                                    //{
                                    //    DebugLogTextInsert("WD: ", BitConverter.ToString(SerialPort_value.data_receiving1, 0, SerialPort_value.data_receiving1.Length).Replace("-", " "));
                                    //}

                                    clear++;
                                }

                                if (SerialPort_value.data_receiving1int[0].ToString("x2").Equals("ff"))
                                {
                                    if (SerialPort_value.data_receiving1int.Length >= 3 && SerialPort_value.data_receiving1.Length == SerialPort_value.data_receiving1int[2] + 3)
                                    {
                                        SerialPort_value.status_receiving1 = 0;
                                       // UpdateDisplay2(SerialPort_value.data_receiving1int);

                                        //if (checkBox_Print.Checked)
                                        //{
                                        //    DebugLogTextInsert("Print: ", BitConverter.ToString(SerialPort_value.data_receiving1, 0, SerialPort_value.data_receiving1.Length).Replace("-", " "));
                                        //}

                                        clear++;
                                    }

                                    if (SerialPort_value.data_receiving1int.Length >= 3
                                        && SerialPort_value.data_receiving1int[1].ToString("x2").Equals("e7")
                                        )
                                    {
                                        byte[] displayBackLight = new byte[] { 0x21, 0x02, 0x83, 0x83 };
                                        displayBackLight[3] = SerialPort_value.data_receiving1[2];
                                        //SerialPort1.Write(displayBackLight, 0, displayBackLight.Length);
                                        KeySend("SerialPortBackgroundWorkerData1_ProgressChanged", displayBackLight);

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

                                        //if (checkBox_Light.Checked)
                                        //{
                                        //    DebugLogTextInsert("displayBackLight: ", BitConverter.ToString(SerialPort_value.data_receiving1, 0, SerialPort_value.data_receiving1.Length).Replace("-", " "));
                                        //}

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
                */
            }
        }

        #endregion SerialPort1

        #region SerialPort2
        public static SerialPort SerialPort2 = new SerialPort();
        private bool ComPortON2()
        {
            try
            {
                SerialPort2.ReadTimeout = 1000;
                SerialPort2.WriteTimeout = 1000;
                SerialPort2.BaudRate = 9600;
                SerialPort2.PortName = ComPort2Number.Text;

                SerialPort2.Open();
                SerialPort2.DiscardInBuffer();
                SerialPort2.DiscardOutBuffer();

                return true;
            }
            catch (Exception ex)
            {
                if (SerialPort2.IsOpen) SerialPort2.Close();

                Debug.WriteLine("ComPortON2: " + ex.Message);
                MessageBox.Show(ex.Message, "ComPortError " + SerialPort_value.port_name2);

                return false;
            }
        }
        private void ComPort2OFF()
        {
            SerialPort2.Close();
        }
        private void SerialPort2_PinChanged(object sender, SerialPinChangedEventArgs e)
        {
                FunctionPttRts(SerialPort2.CtsHolding, SerialPort_value.port_name2);
        }
        #endregion SerialPort2

        static class SerialPort_value
        {
            public static string port_name1 = "Radio";
            public static string port_name2 = "Program";

            public static int status_receiving1;

            public static byte[] data_receiving1 = new byte[0];
            public static int[] data_receiving1int = new int[0];
        }
        public class SerialPort_Data
        {
            public byte[] Data_sp1 { get; set; }
        }
        private void RadioStatus(Boolean _status, Boolean _send=false)
        {
                //if (InvokeRequired)
                //{
                //    this.Invoke(new Action<Boolean, Boolean>(RadioStatus), new object[] { _status, _send });
                //    return;
                //}

                bool _comStatus = SerialPort1.IsOpen;
                bool _rtsStatus = SerialPort1.RtsEnable;

                if (_comStatus && _rtsStatus)
                {
                    PowerStatusTextUpdate("PTT");
                  //  PowerSend(true);
                    _status = true;
                }
                else if (_comStatus && _status && _send)
                {
                    PowerStatusTextUpdate("TXD");
                }
                else if (!_comStatus && _status && _send)
                {
                    PowerStatusTextUpdate("ERROR");
                    DebugLogTextInsert("ERROR: ", "Check comport!");
                }
                else if (_comStatus && _status && !_send)
                {
                    PowerStatusTextUpdate("ON");
                    //PowerSend(true);
                }
                else
                {
                    PowerStatusTextUpdate("OFF");
                    DisplayClear(); 
                    //PowerSend(false);
                }
            TimerRadio.Enabled = false;
            TimerRadio.Enabled = _status;
            TimerRadioAlive.Enabled = _status;
        }
        //private void PowerSend(bool _status)
        //{
        // //   if (InvokeRequired)
        // //   {
        // //       this.Invoke(new Action<Boolean>(PowerSend), new object[] { _status });
        // //       return;
        // //   }
        //    bool _preStatus = TimerRadio.Enabled;
        //    bool _comStatus = SerialPort1.IsOpen;

        //    if (_comStatus && _status && _status != _preStatus)
        //    {
        //        //byte[] keyOn = new byte[] { 0x00, 0x21, 0x02, 0x85 };
        //        //KeySend("PowerSend", keyOn);
        //    }
        //    else if (_status != _preStatus)
        //    {
        //        DisplayClear();
        //    }

        //}

    }
}
