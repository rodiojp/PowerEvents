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

namespace PowerEvents.WinForm
{
    public partial class PowerEventsForm : Form
    {
        //Log4Net logger
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public int WindowWidth { get; private set; } = 27; //32 for Windows 7
        public int WindowHeight { get; private set; } = 24; //24 for Windows 7

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
                if (intWindowWidth>0 && intWindowWidth<100 &&
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
    }
}
