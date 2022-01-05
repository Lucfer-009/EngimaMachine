using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Brad_s_Engima_Machine
{
    static class FileLocationHandler
    {
        static private string mainDirectory = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
        static private string settingsPath = @"\settings\";
        static private string masterConfigPath = mainDirectory + settingsPath;

        static public readonly string logFileLocation_R = masterConfigPath + @"log files\";
        static public readonly string machineData_R = masterConfigPath + @"machine data\";
        static public readonly string cogs_R = masterConfigPath + @"cogs\";
        static public readonly string reversers_R = masterConfigPath + @"ukws\";
        static public readonly string messages_R = masterConfigPath + @"messages\";
        static public readonly string switchboard_R = masterConfigPath + @"switchboard\";


    }
}
