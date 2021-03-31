using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;
using log4net;
using PowerEvents.Domain.Extensions;
using PowerEvents.Domain.Windows;
using System.Timers;
using PowerEvents.WindowsInput;
using PowerEvents.WindowsInput.Native;
using Microsoft.Win32;
using PowerEvents.Domain;

namespace PowerEvents.WinForm
{
    public partial class PowerEventsForm : Form
    {
        //Log4Net logger
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private const int NoUserTimerCounterDefault = 0;
        public int WindowWidth { get; private set; } = 27; //32 for Windows 7
        public int WindowHeight { get; private set; } = 24; //24 for Windows 7
        public System.Timers.Timer PowerTimer { get; private set; }
        public double TimerInterval { get; private set; } = 20 * 1000; // (60+55)*1000; // sec
        public int NoUserTimerCounter { get; private set; } = NoUserTimerCounterDefault;
        public bool CursorMoved { get; private set; }
        public MouseCursorPoint LastCursorPosition { get; private set; }

        public PowerEventsForm()
        {
            InitializeComponent();
            ReadSettings();
        }

        private void ReadSettings()
        {
            //Log.Error(new Exception("Test").ToLogString());
            try
            {
                System.Collections.Specialized.NameValueCollection appSettings = ConfigurationManager.AppSettings;
                string strWindowWidth = appSettings["WindowWidth"];
                int intWindowWidth = Convert.ToInt32(strWindowWidth);
                string strWindowHeight = appSettings["WindowHeight"];
                int intWindowHeight = Convert.ToInt32(strWindowHeight);
                if (intWindowWidth > 0 && intWindowWidth < 100 &&
                    intWindowHeight > 0 && intWindowHeight < 100)
                {
                    WindowWidth = intWindowWidth;
                    WindowHeight = intWindowHeight;
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex.ToLogString());
            }
        }

        private void miExit_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void PowerEventsForm_Load(object sender, EventArgs e)
        {
            Size = new Size(WindowWidth, WindowHeight);
            int boundWidth = Screen.PrimaryScreen.Bounds.Width;
            int x = boundWidth - Width;
            SetDesktopLocation(x / 2, 0);

            StayOnTop();
            SetVideoMode();
            EstablishTimer();
            CheckAutoStart();
        }

        private void CheckAutoStart()
        {
            string valueNameExePath = Application.ExecutablePath;
            RegistryKey reg = Registry.CurrentUser.OpenSubKey(SystemConstants.REG_SUB_KEY_NAME);
            object value = reg.GetValue(SystemConstants.REG_MAIN_APP_KEY_NAME);
            bool bCheck = value != null;
            if (bCheck && value.GetType().Name == "String")
            {
                bCheck = valueNameExePath.Equals(value);
            }
            miAutoStart.Checked = bCheck;
        }

        private void miAutoStart_Click(object sender, EventArgs e)
        {
            //The second parameter should be set to true if you need write access to the key
            RegistryKey reg = Registry.CurrentUser.OpenSubKey(SystemConstants.REG_SUB_KEY_NAME, true);
            if (!miAutoStart.Checked)
            {
                string valueNameExePath = Application.ExecutablePath;
                try
                {
                    reg.SetValue(SystemConstants.REG_MAIN_APP_KEY_NAME, valueNameExePath);
                    reg.Close();
                }
                catch (Exception ex)
                {
                    Log.Error(ex.ToLogString());
                }
            }
            else
            {
                try
                {
                    reg.DeleteValue(SystemConstants.REG_MAIN_APP_KEY_NAME);
                    reg.Close();
                }
                catch (Exception ex)
                {
                    Log.Error(ex.ToLogString());
                }
            }
            CheckAutoStart();
        }

        /// <summary>
        /// Initialize the timer for the first time then inside of m_waitableTimer_OnTimerCompleted()
        /// </summary>
        private void EstablishTimer()
        {
            PowerTimer = new System.Timers.Timer();
            PowerTimer.Elapsed += new System.Timers.ElapsedEventHandler(OnTimerEvent);
            PowerTimer.Interval = TimerInterval;
            PowerTimer.Enabled = true;
            Log.InfoFormat("Try to set Timer @ {0:yyyy/MM/dd HH:mm:ss} for Interval {1}; first OnTimeEvent @ {2:yyyy/MM/dd HH:mm:ss}",
                DateTime.Now, TimerInterval, DateTime.Now.AddMilliseconds(TimerInterval));
        }
        /// <summary>
        /// Specify what you want to happen when the Elapsed event is raised
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnTimerEvent(object sender, ElapsedEventArgs e)
        {
            Log.InfoFormat("OnTime @ {0:yyyy/MM/dd HH:mm:ss} for Interval {1}; next OnTimeEvent @ {2:yyyy/MM/dd HH:mm:ss}",
                DateTime.Now, NoUserTimerCounter, DateTime.Now.AddMilliseconds(TimerInterval));
            CursorMoved = CheckIfCursorMoved();
            if (!CursorMoved)
            {
                NoUserTimerCounter++;
                ThereIsNoUser();
            }
            else
            {
                NoUserTimerCounter = NoUserTimerCounterDefault;
                Log.Info("There is a User");
            }
        }

        private void MouseMovePointerRelative(int dx, int dy)
        {
            Log.Info("Mouse Move Pointer Relative");
            InputSimulator sim = new InputSimulator();
            sim.Mouse
                .MoveMouseBy(dx, dy)
                .Sleep(1000)
                .MoveMouseBy(-dx, -dy);
        }

        private bool CheckIfCursorMoved()
        {
            bool sucess = User32.GetCursorPos(out MouseCursorPoint lpPoint);
            bool moved = !LastCursorPosition.Equals(lpPoint);
            LastCursorPosition = lpPoint;
            return moved;
        }

        private void ThereIsNoUser()
        {
            Log.Info("There is no User");
            if (NoUserTimerCounter % 15 == 0)
            {
                PressWindowsKey();
                MouseMovePointerRelative(100, 100);
                CheckIfCursorMoved();
            }
        }

        private void PressWindowsKey()
        {
            Log.Info("Pressed Windows Key");
            InputSimulator sim = new InputSimulator();
            sim.Keyboard
                .KeyPress(VirtualKeyCode.LWIN)
                .Sleep(1000)
                .KeyPress(VirtualKeyCode.LWIN);
        }

        /// <summary>
        /// Stay on Top
        /// </summary>
        private void StayOnTop()
        {
            WindowPositionFlags topFlags = WindowPositionFlags.IgnoreMove | WindowPositionFlags.IgnoreResize;
            User32.SetWindowPos(Handle, HWndInsertAfter.TopMost, 0, 0, 0, 0, topFlags);
        }
        /// <summary>
        /// Set display required a video mode (like)
        /// </summary>
        private void SetVideoMode()
        {
            ExecutionState videoFlags = ExecutionState.ES_CONTINUOUS | ExecutionState.ES_DISPLAY_REQUIRED;
            Kernel32.SetThreadExecutionState(videoFlags);
        }

        private void PowerEventsForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (PowerTimer != null)
            {
                PowerTimer.Enabled = false;
            }
        }
    }
}
