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
            engima.PowerOn(); // Starts the machine
        }
    }
    
}
