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
        private string initialMessage;
        private string endMessage;
        private string switchboardLog;

        private int defaultArraySize;

        private static Random rand = new Random();

        private CogArray[] machineCogs = new CogArray[3];
        private SwitchArray switchBoard;
        private ReverserArray ukw;

        public Machine(string name, int defaultArraySize)
        {
            this.defaultArraySize = defaultArraySize;
        }

        public string ForceUse(string message, CogArray[] forced_machineCogs, SwitchArray forced_switchBoard, ReverserArray forced_ukw) // Used to instantly return a string from a given engima setting.
        {
            for(int i = 0; i < forced_machineCogs.Length; i++)
            {
                machineCogs[i] = forced_machineCogs[i];
            }
            switchBoard = forced_switchBoard;
            ukw = forced_ukw;

            string ret = "";
            foreach(char y in message)
            {
                if(CheckIfTraditionalCompatible(y) == false)
                {
                    throw new Exception($"Issue with force loading through the engima machine, a not compatiable value | {y} | was attempted to be decoded.");
                }
                ret += FullPassThrough(y);
            }
            return ret;
        }


        public void PowerOn()
        {
            LogFile.Write("Machine.powerOn()", "First boot of engima machine");
            
            InitialiseCogs();
            GU.Print("");
            LogFile.Write("Machine.InitialseCogs()", "Got cog settings from user" );

            InitialiseSwitchboard();
            GU.Print("");
            LogFile.Write("Machine.InitialseSwitchboard()", "Got switchboard settings from user");

            InitialiseUKW();
            GU.Print("");
            LogFile.Write("Machine.InitialseUKW()", "Got reverser settings from user");


            GU.Print("-- -- -- -- --");
            GU.Print("1 . Live Entry");
            GU.Print("2 . KeyByKey Entry");
            GU.Print("3 . File Read");
            GU.Print("");
            GU.Print("-- -- -- -- --");

            int response = GU.GetIntWithinBound("Enter your selection", 1, 3);
            switch(response)
            {
                case 1:
                    LiveRead();
                    break;
                case 2:
                    KeyByKeyEntry();
                    break;
                case 3:
                    FileRead();
                    break;
                default:
                    GU.DisplayErrorState(new Exception("False Entry of value"), "A value not within 1 - 3 has been entered to access a menu option", "PowerOn()");
                    break;
            }

            GU.Print("----\n");
            bool save = GU.GetBoolFromUser("Would you like to save this answer to a file?");
            if(save == true)
            {
                FileSys.WriteStringToTxtFile(endMessage, FileLocationHandler.readout_R + $"{rand.Next(1000, 9999)}.txt");
            }


            // Logging of the process

            LogFile.Write("", "");
            LogFile.Write("-- --", "");


            LogFile.Write("--", "COG SETTINGS --"); // Logs Cog Settings
            int count = 0;
            foreach (CogArray Y in machineCogs)
            {
                LogFile.Write("", $"|Position: {count} |     |Cog: {Y.GetCogLocation()} |     |Intital Char: {Y.GetInitialChar()} |     |Initial RingPos: {Y.GetInitialRingPos()}|  ");
                count++;
            }

            LogFile.Write("--", "SWITCHBOARD SETTINGS --"); // Logs, quite complicatedly, the switchboard config

            string line = " ";
            count = 0;
            List<char> usedSwitchPairs = new List<char>();
            foreach(char y in switchboardLog) // shows parings that aren't that of themselves, aka - not AA but will show AC
            {
                if( (  Convert.ToString(GU.IntIndexToAlphaChar(count)) != Convert.ToString(y).ToUpper() ) && usedSwitchPairs.Contains(y) == false )
                {
                    usedSwitchPairs.Add(GU.IntIndexToAlphaChar(count));
                    line += $"{GU.IntIndexToAlphaChar(count)}/{Convert.ToString(y).ToUpper()} ";
                }             
                count++;           
            }
            LogFile.Write("", line);

            LogFile.Write("-- Start message --\n", $"{initialMessage}");
            LogFile.Write("-- End Message   --\n", $"{endMessage}");

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

                char S_ = '#';
                bool test = false;
                while (test == false)
                {
                    S_ = GU.GetCharFromUser($"Enter start character for cog {C}", true);
                    if(CheckIfTraditionalCompatible(S_) == false || S_ == ' ')
                    {
                        GU.Print("ERROR | Please enter a valid character A-Z");
                    }
                    else
                    {
                        test = true;
                    }
                }

                int S = GU.AlphaCharToIntIndex(S_,-64); // Start Position
                int R = GU.GetIntWithinBound($"Enter ring position for cog {C}", 0, defaultArraySize); // Ring Position

                int shift;
                if (R > S) { shift = (defaultArraySize + (S - R) ) % defaultArraySize; } // Calculates the relative positive shift (e.g. with an index size of 10. A shift of -2 becomes +8. A circular array)
                else { shift = (S - R) % defaultArraySize; }
                machineCogs[x] = new CogArray(shift, defaultArraySize, $"cog{C}.txt", S_, R);
            }
            GU.Print("");
        }
        private void InitialiseSwitchboard()
        {
            GU.Print("");
            bool reply = GU.GetBoolFromUser("Would you like to load the switchboard config from a file?");

            if(reply == false)
            {
                double maxBinds = Math.Round(Convert.ToDouble(defaultArraySize) / 2, 0) - 3;
                GU.Print("Enter characters that are connected, typing \"AB\" means that A & B are joined.");
                GU.Print("> Type \"!\" to end and pass through the configuration <");
                GU.Print("> Type \"#\" to restart the process.                   <");
                GU.Print("   -- -- -- -- -- -- -- --");
                GU.Print($"You can have a total of {maxBinds} pairs,");
                GU.Print("going up to this will automatically pass the selection through");

                string endSetting = Get_SB_Settings(maxBinds);
                GU.Print($"> {endSetting} <");
                switchBoard = new SwitchArray(defaultArraySize, endSetting, true);

                bool saveBoard = GU.GetBoolFromUser("Do you wish to save this Switchboad to memory?");
                if(saveBoard == true)
                {
                    switchBoard.SaveSwitchBoard(endSetting);
                    GU.Print($"{switchBoard.GetID()} has been saved to memory!");
                }
            }
            else
            {
                int address = GU.GetIntWithinBound("Please enter 4-digit ID for switchboard saved preset", 1000, 9999);
                switchBoard = new SwitchArray(defaultArraySize, $"{address}.txt");
                switchboardLog = FileSys.GetStringFromFile(FileLocationHandler.switchboard_R + $"{address}.txt");
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

            int[] loggedShifts = { machineCogs[0].GetShift(), machineCogs[1].GetShift(), machineCogs[2].GetShift() };

            string startmessage = "";
            bool check = true;
            int menucount = 5;
            string message = "";
            while (check == true)
            {
                try
                {
                    if(menucount == 12)
                    {
                        GU.Print("Enter ! to stop and end");
                        GU.Print("Enter # to restart");
                        GU.Print("[EXPERIMENTAL] Enter @ to restart it to it's origional cog positions entered");
                        GU.Print("[EXPERIMENTAL] Enter $ to go back one character entry, to back parse the machine");
                        menucount = 0;
                    }
                    menucount++;

                    char input = GU.GetCharFromUser("Enter a character to parse through the machine > ", true);

                    if (input == '!')
                    {
                        GU.Print("\n - END - \n");
                        check = false;
                    }

                    else if (input == '#')
                    {
                        message = "";
                        startmessage = "";
                        GU.Print("Message cleared!");
                    }

                    else if (input == '@')
                    {
                        int count = 0;
                        foreach (CogArray V in machineCogs)
                        {
                            V.SetShift(loggedShifts[count]);
                            count++;
                        }
                        message = "";
                        startmessage = "";
                        GU.Print("Message cleared! & Cogs reset to initial settings");
                        LogFile.Write("KeyByKeyEntry()", "User cleared entry and reset cogs to initial settings");

                    }

                    else if (input == '$')
                    {
                        // need to put backparsing in here
                        message = message.Substring(0, message.Length-1);
                        startmessage = startmessage.Substring(0, startmessage.Length - 1);
                        GU.Print($"\nCurrent Message : {message}\n");
                    }

                    else if (CheckIfTraditionalCompatible(input) == false)
                    {
                        GU.Print("ERROR | Enter a valid character");
                    }

                    else
                    {
                        startmessage += input;
                        message += FullPassThrough(input);
                        GU.Print($"\nCurrent Message : {message}\n");
                    }
                       
                }
                catch
                {

                }
            }
            endMessage = message;
            initialMessage = startmessage;
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
            int count = 0; // for breakpoint reasons
            foreach(char y in input)
            {
                char got = FullPassThrough(y);
                ret += got;
                count++;
            }
            Console.WriteLine("\n");
            GU.Print($"{ret}");
            initialMessage = input;
            endMessage = ret;

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

            initialMessage = writtenText;
            endMessage = ret;
        }


        private char FullPassThrough(char inflow)
        {
            LogFile.Write("------------------", "");
            int log = 0;
            char outflow = ' ';

            int current = GU.AlphaCharToIntIndex(inflow); //Starts by getting the index
            if(current == -33) 
            {
                LogFile.Write("FullPassThrough()"," - [SPACE] <Character Skipped>");
                return outflow; 
            }



            // Brad's much improved engima stepping code

            machineCogs[0].IncrementCog();
            LogFile.Write("FullPassThrough()", " - Cog in position 1 Incremented");
            if (machineCogs[0].IsAtTurnover(1) || machineCogs[1].IsAtTurnover(0))
            {
                machineCogs[1].IncrementCog();
                LogFile.Write("FullPassThrough()", " - Cog in position 2 Incremented");
            }
            if(machineCogs[1].IsAtTurnover(0))
            {
                machineCogs[2].IncrementCog();
                LogFile.Write("FullPassThrough()", " - Cog in position 3 Incremented");
            }


            int countA = 0;
            log = current;

            current = switchBoard.ForwardParse(current); //Goes through SwitchBoard
            LogFile.Write("FullPassThrough()", $" - {log}({GU.IntIndexToAlphaChar(log)}) > [Switchboard-F] > {current}({GU.IntIndexToAlphaChar(current)})");

            foreach (CogArray C in machineCogs)
            {
                log = current;
                current = C.ForwardParse(current);
                LogFile.Write("FullPassThrough()", $" - {log}({GU.IntIndexToAlphaChar(log)}) > [COG {countA} -F] > {current}({GU.IntIndexToAlphaChar(current)})");
                countA++;
            }

            log = current;
            current = ukw.ForwardParse(current);
            LogFile.Write("FullPassThrough()", $" - {log}({GU.IntIndexToAlphaChar(log)}) > [UKW] > {current}({GU.IntIndexToAlphaChar(current)})");

            for (int i = 2; i >= 0; i--)
            {
                log = current;
                current = machineCogs[i].ReverseParse(current);
                LogFile.Write("FullPassThrough()", $" - {log}({GU.IntIndexToAlphaChar(log)}) > [COG {i} -R] > {current}({GU.IntIndexToAlphaChar(current)})");
            }

            log = current;
            current = switchBoard.ReverseParse(current);
            LogFile.Write("FullPassThrough()", $" - {log}({GU.IntIndexToAlphaChar(log)}) > [Switchboard-R] > {current}({GU.IntIndexToAlphaChar(current)})");

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
            List<char> usedChar = new List<char>();
            while (check == true) // Rather complex array of statements that essentially prevent the user from entering anything other than an alphabetical
            {                     // character or a char to restart/end the process (!, #). Not to mention it prevents the user from binding the same char to two
                                  // different other characters.
                try
                {
                    
                    A = GU.GetCharFromUser("Enter first character", true);
                    B = GU.GetCharFromUser("Enter second character", true);
                    if (A == '#')
                    {
                        GU.Print("UPDATE | This process has been restarted!\n\n");
                        bindsUsed = 0; //resests the binds used to 0
                        for (int i = 0; i < defaultArraySize; i++) // wipes the array to defualt
                        {
                            blankSettings[i] = GU.IntIndexToAlphaChar(i);
                        }

                    }
                    else if (usedChar.Contains(A) == true || usedChar.Contains(B) == true) 
                    {
                        GU.Print($"ERROR | You've already bound {A} or {B}! this entry has been ignored.");
                    }
                    else if (A == '!')
                    {
                        check = false;
                    }
                    else if (bindsUsed == maxBinds) 
                    {
                        GU.Print("You've used all your binds, continuing with current selection.");
                        check = false;
                    }
                    else if(CheckIfTraditionalCompatible(A) == false || CheckIfTraditionalCompatible(B) == false) // This could be removed at a later edition to allow for a larger cog to be used.
                    {
                        GU.Print($"ERROR | Etiher {A} or {B} is not an acceptable character!");
                    }
                    else
                    {
                        usedChar.Add(A); usedChar.Add(B); // Adds the two characters to a log of used characters so they can't be re-entered.

                        int index = GU.AlphaCharToIntIndex(A); // A > E
                        int indexR = GU.AlphaCharToIntIndex(B); // E > A
                        blankSettings[index] = B;
                        blankSettings[indexR] = A;
                        bindsUsed++;
                        
                        

                    }
                    GU.Print($"-- -- REMAINING PAIRS: {maxBinds - bindsUsed}");
                }
                catch(Exception e)
                {
                    usedChar.Clear();
                    GU.Print(e.ToString());
                }
            }
            foreach(char x in blankSettings)
            {
                endSetting += x;
            }

            switchboardLog = endSetting;
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
