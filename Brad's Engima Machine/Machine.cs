using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Brad_s_Engima_Machine
{
    class Machine
    {
        private static string ID;

        private string launchTime;
        private string launchTimeSrt;
        private string name;
        private string logFileAddress;

        private int defaultArraySize;

        private Random rand = new Random();

        private Cog[] machineCogs = new Cog[3];

        private LogFile log;

        public Machine(string name, int defaultArraySize, string logFileAddress = @"C:\Users\Brad\Documents\GitHub\EngimaMachine\Brad's Engima Machine\log files\")
        {
            launchTime = DateTime.Now.ToString("F"); // Logs the time at which the Machine was instanciated.
            launchTimeSrt = DateTime.Now.ToString("G"); // Logs a short version of the dateTime for the name of the logfile

            this.name = name;
            this.defaultArraySize = defaultArraySize;
            this.logFileAddress = logFileAddress;

            ID = $"{rand.Next(1000, 9999)}-{rand.Next(1000, 9999)}-{rand.Next(1000, 9999)}"; // ID system. Assigns each instance of Machine a number.

            log = new LogFile(ID);  // File name here is the short date + the ID of the Machine
        }

        public void powerOn()
        {
            log.Write("Machine.powerOn()", "First boot of engima machine");
            
        }

        private bool checkIfTraditionalCompatible(string input)
        { // Ensures that a user only enters letters or spaces. Will ask them to re-enter the message otherwise.
            bool test = false;
            bool flag = false;

            foreach (char v in input)
            {
                int index = GU.AlphaCharToIntIndex(v);
                if ((index < 0 || index > 25) & index != -33) // -33 is the index value for SPACE, allows spaces to be allowed through
                {
                    flag = true;
                }
            }

            if (flag == false) { test = true; }

            return test;

        }



    }

}
