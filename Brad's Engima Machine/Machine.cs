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

        private static Random rand = new Random();

        private CogArray[] machineCogs = new CogArray[3];
        private SwitchArray switchBoard;
        private ReverserArray ukw;

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
            InitialiseCogs();
            InitialiseSwitchboard();
            
        }

        private void InitialiseCogs()
        {
            List<int> choiceOfCogs = new List<int> { 1, 2, 3, 4, 5 };
            for (int x = 0; x < 3; x++)
            {
                GU.Print("Enter your cogs from right to left from the options below: "); // Gathers essential settings
                int C = GetCogChoice($"{x + 1}", ref choiceOfCogs);
                int S = GU.GetIntWithinBound($"Enter start position for cog {C}", 0, defaultArraySize); // Start Position
                int R = GU.GetIntWithinBound($"Enter ring position for cog {C}", 0, defaultArraySize); // Ring Position

                int shift;
                if (R > S) { shift = defaultArraySize + (S - R); } // Calculates the relative positive shift (e.g. with an index size of 10. A shift of -2 becomes +8. A circular array)
                else { shift = S - R; }
                machineCogs[x] = new CogArray(shift, defaultArraySize, $"cog{x + 1}.txt");
            }
            GU.Print("");
        }

        private void InitialiseSwitchboard()
        {
            GU.Print("");
            bool reply = GU.GetBoolFromUser("Would you like to load the switchboard config from a file?");

            if(reply == false)
            {
                double maxBinds = Math.Round(Convert.ToDouble(defaultArraySize) / 2, 0) - 1;
                GU.Print("Enter characters that are connected by typing \"AB\" to illustrate that A & B are joined.");
                GU.Print("Type \"!\" to end and pass through the configuration <");
                GU.Print("Type \"#\" to restart the process.                   <");
                GU.Print("  -- -- -- -- -- -- -- --");
                GU.Print($"You can have a total of {maxBinds} pairs, exceeding this will auto void the entry");

                string endSetting = Get_SB_Settings(maxBinds);
                GU.Print($"> {endSetting} <");
                switchBoard = new SwitchArray(defaultArraySize, endSetting, true);

                bool saveBoard = GU.GetBoolFromUser("Do you wish to save this Switchboad to memory?");
                if(saveBoard == true)
                {
                    switchBoard.SaveSwitchBoard();
                }
            }
            else
            {
                int address = GU.GetIntWithinBound("Please enter 4-digit ID for switchboard saved preset", 1000, 9999);
                switchBoard = new SwitchArray(defaultArraySize, $"{address}.txt");
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

        
        private string Get_SB_Settings(double maxBinds)
        {
            string endSetting = "";
            char[] blankSettings = new char[defaultArraySize];
            int bindsUsed = 0;

            for(int i = 0; i < defaultArraySize; i++)
            {
                blankSettings[i] = GU.IntIndexToAlphaChar(i); // Loads a blank settings folder where A>A, B>B, C>C etc
            }

            bool check = true;
            while(check == true)
            {
                try
                {
                    char A = GU.GetCharFromUser("Enter first character", true);
                    if(bindsUsed > maxBinds)
                    {
                        throw new Exception("ERROR | You've used all your binds, please re-enter from the begining.");
                    }
                    else if (A == '#')
                    {
                        throw new Exception("RESTARTED PROCESS");
                    }
                    else if (A == '!')
                    {
                        check = false;
                    }
                    else
                    {
                        char B = GU.GetCharFromUser("Enter second character", true);
                        int index = GU.AlphaCharToIntIndex(A);
                        int indexR = GU.AlphaCharToIntIndex(B);
                        blankSettings[index] = B;
                        blankSettings[indexR] = A;
                        bindsUsed++;
                        GU.Print($"-- -- REMAINING PAIRS: {maxBinds - bindsUsed}");
                    }
                    
                }
                catch(Exception e)
                {
                    GU.Print(e.ToString());
                }
            }
            foreach(char x in blankSettings)
            {
                endSetting += x;
            }
            return endSetting;

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
                    GU.Print($"Cog {N} -");
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
