using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerEvents.Domain
{
    public static class SystemConstants
    {
        //Registry Keys
        public const string REG_SUB_KEY_NAME = @"Software\Microsoft\Windows\CurrentVersion\Run";
        public const string REG_MAIN_APP_KEY_NAME = @"PowerEvents";

        // Error list
        public const string MISSING_HEALTH_CHECK_SECTION_ERR = "HealthChecksSection is not defined";
        // Messages
        public const string TOTAL_FILE_SIZE_MSG = "Total Space Saved {0}";

    }
}
