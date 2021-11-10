using System;

namespace Brad_s_Engima_Machine
{
    class Program
    {
        static void Main(string[] args)
        {
            GU.Print("Hello World!");
            Machine engima = new Machine();
        }
    }
    class Machine
    {
        private string launchTime = DateTime.Now.ToString("F");

        public Machine(string logFileAddress = "")
        {
            GU.Print(launchTime);



        }

    }
}
