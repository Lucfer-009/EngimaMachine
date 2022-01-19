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
        static private string address;
        static private string currentTime = DateTime.Now.ToString("HH:mm:ss");
        static private string currentTimePathKey = DateTime.Now.ToString("HH.mm.ss");
        static private string currentDate = DateTime.Now.ToString("d MMMM");
        static private StreamWriter mainFile;
        static private Random rand = new Random();

        static LogFile()
        {
            string u_ID = $"{currentTimePathKey} - {currentDate}";
            address = FileLocationHandler.logFileLocation_R + $"{u_ID}.txt";
            mainFile = new StreamWriter(address); // Name of file is the date passed through alongside the ID.
        }

        static public void Close()
        {
            Write("logFile.Close()", "Logging terminated.");
            mainFile.Close();

        } // Closes log file

        static public void Write(string commandAction, string note)
        {
            mainFile.WriteLine($"{currentTime}  > {commandAction, -35} | {note}");
        }
    }
}
