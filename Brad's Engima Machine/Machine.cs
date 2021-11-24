using System;
using System.Collections.Generic;
using System.IO;
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

        private CogArray[] machineCogs = new CogArray[3];

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

        public void PowerOn()
        {
            log.Write("Machine.powerOn()", "First boot of engima machine");

            List<int> choiceOfCogs = new List<int> { 1, 2, 3, 4, 5 };
            for(int x = 0; x < 3; x++)
            {
                GU.Print("Enter your cogs from right to left from the options below: "); // Gathers essential settings
                int C = GetCogChoice($"{x+1}", ref choiceOfCogs);
                int S = GU.GetIntWithinBound($"Enter start position for cog {C}", 0, defaultArraySize); // Start Position
                int R = GU.GetIntWithinBound($"Enter ring position for cog {C}", 0, defaultArraySize); // Ring Position

                int shift;
                if (R > S) { shift = defaultArraySize + (S - R); } // Calculates the relative positive shift (e.g. with an index size of 10. A shift of -2 becomes +8. A circular array)
                else { shift = S - R; }
                machineCogs[x] = new CogArray(shift, defaultArraySize, $"cog{x+1}.txt");
            }
            
        }

        private void KeyByKeyEntry()
        {

        }

        private void LiveRead()
        {

        }

        private void FileRead()
        {

        }

        

        private int GetCogChoice(string position, ref List<int> choiceOfCogs)
        {
            int cog = -1;

            bool clear = false;
            while(clear == false)
            {
                GU.Print("- Avaliable Cogs -");
                foreach(int N in choiceOfCogs)
                {
                    GU.Print($"> Cog {N}");
                }
                cog = GU.GetIntWithinBound($"Enter cog {position} ", 1, 5);
                if(choiceOfCogs.Contains(cog))
                {
                    choiceOfCogs.Remove(cog);
                    clear = true;
                }
                else
                {
                    GU.Print("ERROR | You can not enter the same cog more than once!");
                }
            }
                
            if(cog == -1) { throw new Exception("Issue with cog assignment in Machine.GetCogChoice()"); }
            return cog;
        }

        private bool CheckIfTraditionalCompatible(string input)
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
