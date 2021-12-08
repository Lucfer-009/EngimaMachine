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

            log = new LogFile();
        }

        public void PowerOn()
        {
            LogFile.Write("Machine.powerOn()", "First boot of engima machine");
            
            InitialiseCogs();
            LogFile.Write("Machine.InitialseCogs()", "Got cog settings from user" );
            InitialiseSwitchboard();
            LogFile.Write("Machine.InitialseSwitchboard()", "Got switchboard settings from user");
            InitialiseUKW();
            LogFile.Write("Machine.InitialseUKW()", "Got reverser settings from user");

            FileRead();
            LogFile.Close();
            GU.Print("-- -- -- -- -- --");
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

        private void InitialiseUKW()
        {
            bool check = true;
            char choice = ' ';
            while(check == true)
            {
                choice = GU.GetCharFromUser("Enter UKW / Reverser of choice, A - B - C");
                if(choice is not('A' or 'B' or 'C'))
                {
                    GU.Print("ERROR | Please enter either A, B or C");
                }
                else
                {
                    check = false;
                }
            }
            ukw = new ReverserArray(defaultArraySize, $"reflector{choice}.txt");

        }

        private void KeyByKeyEntry()
        {
            GU.Print("\n\n: KeyByKey Entry :");

            bool check = true;
            string message = "";
            while (check == true)
            {
                try
                {
                    GU.Print("Enter ! to stop and end");
                    GU.Print("Enter # to restart");
                    char input = GU.GetCharFromUser("Enter a character to parse through the machine > ", true);
                    if(input == '!')
                    {
                        GU.Print("\n - END - \n");
                        check = false;
                    }
                    else if (input == '#')
                    {
                        message = "";
                        GU.Print("Message cleared!");
                    }
                    else if (CheckIfTraditionalCompatible(input) == false)
                    {
                        GU.Print("ERROR | Enter a valid character");
                    }
                    else
                    {
                        message += FullPassThrough(input);
                        GU.Print($"\nCurrent Message : {message}\n");
                    }
                       
                }
                catch
                {

                }
            }
        }

        private void LiveRead()
        {
            string input = "";
            GU.Print("\n\n: Live Read :");
            bool acceptable = false;
            while (acceptable == false)
            {
                input = GU.GetStringFromUser("Please enter your message: ").ToUpper();
                if(CheckIfTraditionalCompatible(input) == true)
                {
                    acceptable = true;
                }
                else
                {
                    GU.Print("ERROR | Please enter a string that contains genuine engima characters!\n");
                }
            }
            GU.Print("");

            string ret = "";
            foreach(char y in input)
            {
                char got = FullPassThrough(y);
                ret += got;
            }
            Console.WriteLine("\n");
            GU.Print($"{ret}");

        }

        private void FileRead()
        {
            GU.Print("\n\n: File Read :");
            string writtenText = "";
            const string baseLocationOfText = @"F:\VS Projects\Project\Lucfer-009\EngimaMachine\Brad's Engima Machine\messages\";
            string locationOfText = "";
            bool acceptable = false;
            while (acceptable == false)
            {
                locationOfText = GU.GetStringFromUser("Please enter the file name of message (IF NOT IN \\messages enter \"!\")");
                
                if (locationOfText == "!")
                {
                    locationOfText = GU.GetStringFromUser("Enter the full address");
                }
                else
                {

                    locationOfText = $"{baseLocationOfText}{locationOfText}.txt";
                }

                if(File.Exists(locationOfText) == true)
                {
                    acceptable = true;
                }
                else
                {
                    GU.Print("ERROR | Please enter a valid path, file doesn't exsist.");
                }
            }
            

            writtenText = FileSys.GetStringFromFile(locationOfText);

            GU.Print("\n");
            string ret = "";
            foreach(char y in writtenText)
            {
                char got = FullPassThrough(y);
                ret += got;
            }
            Console.WriteLine("\n");
            GU.Print(writtenText);
            GU.Print(ret);
        }

        private char FullPassThrough(char inflow)
        {
            string log = "";
            char outflow = ' ';

            int current = GU.AlphaCharToIntIndex(inflow); //Starts by getting the index
            if(current == -33) 
            {
                LogFile.Write("FullPassThrough()"," - [SPACE] <Character Skipped>");
                return outflow; 
            }
            LogFile.Write("FullPassThrough()", " - Cog in position 1 Incremented");
            if (machineCogs[0].IncrementCog() == true) // Increments through cogs upon entry of character
            {
                LogFile.Write("FullPassThrough()", " - Cog in position 1 reached turnover, Cog in position 2 Incremented");
                if (machineCogs[1].IncrementCog() == true)
                {
                    LogFile.Write("FullPassThrough()", " - Cog in position 2 reached turnover, Cog in position 3 Incremented");
                    machineCogs[2].IncrementCog();
                }
            }

            log = $"{current}";
            current = switchBoard.ForwardParse(current); //Goes through SwitchBoard
            LogFile.Write("FullPassThrough()", $" - {log} > [Switchboard-F] > {current}");
            foreach (CogArray C in machineCogs)
            {
                current = C.ForwardParse(current);
            }

            current = ukw.ForwardParse(current);

            for (int i = 2; i >= 0; i--)
            {
                current = machineCogs[i].ReverseParse(current);
            }

            current = switchBoard.ReverseParse(current);

            outflow = GU.IntIndexToAlphaChar(current);


            return outflow;
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
            char A = ' ';
            char B = ' ';
            while(check == true)
            {
                try
                {
                    A = GU.GetCharFromUser("Enter first character", true);
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
                    else if(CheckIfTraditionalCompatible(A) == false || CheckIfTraditionalCompatible(B) == false) // This could be removed at a later edition to allow for a larger cog to be used.
                    {
                        throw new Exception("Invalid input");
                    }
                    else
                    {
                        B = GU.GetCharFromUser("Enter second character", true);
                        int index = GU.AlphaCharToIntIndex(A); // A > E
                        int indexR = GU.AlphaCharToIntIndex(B); // E > A
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
            if (flag == false)
            {
                test = true; 
            }
            return test;
        }

        private bool CheckIfTraditionalCompatible(char input)
        { // Ensures that a user only enters letters or spaces. Will ask them to re-enter the message otherwise.
            bool test = false;
            bool flag = false;

            int index = GU.AlphaCharToIntIndex(input);
            if ((index < 0 || index > 25) & index != -33) // -33 is the index value for SPACE, allows spaces to be allowed through
            {
                flag = true;
            }
            if (flag == false)
            { 
                test = true; 
            }
            return test;
        }



    }

}
