using System;
using System.IO;

namespace Brad_s_Engima_Machine
{
    class Program
    {
        static void Main(string[] args)
        {
            GU.Print("Hello World!");
            Machine engima = new Machine("Test Machine");
        }
    }
    class Machine
    {
        private string launchTime;
        private string name;

        public Machine(string name, string logFileAddress = @"C:\Users\Brad\Documents\GitHub\EngimaMachine\Brad's Engima Machine\log files\")
        {
            launchTime = DateTime.Now.ToString("F"); // Logs the time at which the Machine was instanciated.
            this.name = name;

        }

    }
}
