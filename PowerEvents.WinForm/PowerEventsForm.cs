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
    }
}
