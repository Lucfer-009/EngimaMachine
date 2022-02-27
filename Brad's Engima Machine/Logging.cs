using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Brad_s_enigma_Machine
{
    public class LogFile
    {
        static private string address;
        static private string currentTime = DateTime.Now.ToString("HH:mm:ss");
        static private StreamWriter mainFile;
        static private Random rand = new Random();

        public LogFile()
        {
            string u_ID = $"{DateTime.Now.ToString("HH.mm.ss")} - {DateTime.Now.ToString("d MMMM")}";
            address = FileLocationHandler.logFileLocation_R + $"{u_ID}.txt";
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
