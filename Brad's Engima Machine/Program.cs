using System;
using System.IO;

namespace Brad_s_Engima_Machine
{
    class Program
    {
        static void Main(string[] args)
        {
            GU.Print("Hello World!");
            Machine engima = new Machine("Test Machine", 26);
            engima.powerOn(); // Starts the machine
        }
    }
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
            log.Close();
        }

        private string GetMessageFromUser()
        {
            string input = "";
            bool test = true;
            bool flag;
            while(test == true)
            {
                flag = false;
                input = GU.GetStringFromUser("Please enter the message").ToUpper();
                foreach (char v in input)
                {
                    int index = GU.AlphaCharToIntIndex(v);
                    if ((index < 0 || index > 25) & index != -33) // -33 is the index value for SPACE, allows spaces to be allowed through
                    {
                        flag = true;
                    }
                }

                if (flag == false) { test = false; }
                else
                {
                    GU.Print("ERROR | Please ensure you've entered acceptable characters,");
                    GU.Print("      | Only letters of the alphabet are allowed in traditonal enigma.");
                }
            }
            return input;
            
        }



    }
}
