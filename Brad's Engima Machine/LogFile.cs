using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Brad_s_Engima_Machine
{
    class LogFile
    {
        private string defaultPath = @"F:\VS Projects\Project\Lucfer-009\EngimaMachine\Brad's Engima Machine\log files\";
        private string address;

        private string currentTime = DateTime.Now.ToString("HH:mm:ss");

        private StreamWriter mainFile; 

        public LogFile(string u_ID)
        {
            address = defaultPath + $"{u_ID}.txt";
            mainFile = new StreamWriter(address); // Name of file is the date passed through alongside the ID.
        }

        public void Close()
        {
            Write("logFile.Close()", "Logging terminated.");
            mainFile.Close();

        } // Closes log file

        public void Write(string commandAction, string note)
        {
            mainFile.WriteLine($"{currentTime}  > {commandAction, -35} | {note}");
        }
    }
}
