using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Brad_s_enigma_Machine
{
    class Machine
    {
        private string initialMessage;
        private string endMessage;
        private string switchboardLog;
        private char ukwChosen;

        private int defaultArraySize;

        static private Random rand = new Random();

        private CogArray[] machineCogs = new CogArray[3];
        private SwitchArray switchBoard;
        private ReverserArray ukw;

        private LogFile Logging = new LogFile();

        public Machine(int defaultArraySize)
        {
            this.defaultArraySize = defaultArraySize;
            

        }

        public void PowerOn()
        { //Primary function of the class, it is the only public method.
            Logging.Write("Machine.powerOn()", "First boot of enigma machine");
            
            InitialiseCogs();
            GU.Print("");
            Logging.Write("Machine.InitialseCogs()", "Got cog settings from user" );

            InitialiseSwitchboard();
            GU.Print("");
            Logging.Write("Machine.InitialseSwitchboard()", "Got switchboard settings from user");

            InitialiseUKW();
            GU.Print("");
            Logging.Write("Machine.InitialseUKW()", "Got reverser settings from user");
            
            GU.Print("");
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
                default: // This state can never be reached due to a guard clause for this variable above, although it is here anyways as good practise.
                    GU.DisplayErrorState(new Exception("False Entry of value"), "A value not within 1 - 3 has been entered to access a menu option", "PowerOn()");
                    break;
            }

            GU.Print("----");
            GU.Print("");
            bool save = GU.GetBoolFromUser("Would you like to save this answer to a file?");
            if(save == true)
            {
                int savingId = rand.Next(1000, 9999); // The 4-digit ID
                FileSys.WriteStringToTxtFile(endMessage, FileLocationHandler.readout_R + $"{savingId}.txt");
                GU.Print($"{savingId} has been saved to memory!");
            }


            // Logging of the process ---------------------------------------------------------

            Logging.Write("", "");
            Logging.Write("-- --", "");


            Logging.Write("--", "COG SETTINGS --"); // Logs Cog Settings
            int count = 0;
            foreach (CogArray Y in machineCogs)
            {
                Logging.Write("", $"|Position: {count} |   |Cog: {Y.GetCogLocation()} |   |Initial Char: {Y.GetInitialChar()} |   |Initial RingPos: {Y.GetInitialRingPos()}|  ");
                count++;
            }

            Logging.Write("--", $"REFLECTOR (UKW) SELECTED : {ukwChosen}");
            Logging.Write("--", "");
            Logging.Write("--", "SWITCHBOARD SETTINGS --"); // Logs, quite complicatedly, the switchboard config

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
            } // Also ensures that AC and CA aren't both shown as this is redundant as the user is aware the relationship is bidirectional.
            Logging.Write("", line);

            Logging.Write("-- Start message --", $"{initialMessage}");
            Logging.Write("", "");
            Logging.Write("-- End Message   --", $"{endMessage}");
            Logging.Write("", "");
            Logging.Write("-", "");
            Logging.Close();
            GU.Print("-- -- -- -- -- --");
        }


        private void InitialiseCogs()
        {
            string[] postions = { "FIRST/FAST", "MIDDLE", "LAST/SLOW" };
            List<int> choiceOfCogs = new List<int> { 1, 2, 3, 4, 5 };
            for (int x = 0; x < 3; x++)
            {
                GU.Print("Enter your cogs from right to left from the options below: "); // Gathers essential settings
                int C = GetCogChoice($"{x + 1} | {postions[x]}", ref choiceOfCogs);

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

                int Position = GU.AlphaCharToIntIndex(S_,-64); // Start Position
                int Ring = GU.GetIntWithinBound($"Enter ring position for cog {C}", 1, defaultArraySize); // Ring Position
                GU.Print("--");

                int shift;
                shift = Position - Ring;
                if (Position < Ring)
                {
                    shift = (defaultArraySize + shift) % defaultArraySize;
                }

                machineCogs[x] = new CogArray(shift, defaultArraySize, $"cog{C}.txt", S_, Ring); // Initialises the machineCogs[] array
            }
            GU.Print("");
        }
        private void InitialiseSwitchboard()
        {
            GU.Print("");
            bool reply = GU.GetBoolFromUser("Would you like to load the switchboard config from a file?");

            if(reply == false)
            {
                double maxBinds = Math.Round(Convert.ToDouble(defaultArraySize) / 2, 0) - 3; // masxBinds is variable, it is dependant on the size of the alphabet used.
                GU.Print("Enter characters that are connected, typing \"AB\" means that A & B are joined.");
                GU.Print("> Type \"!\" to end and pass through the configuration <");
                GU.Print("> Type \"#\" to restart the process.                   <");
                GU.Print("   -- -- -- -- -- -- -- --");
                GU.Print($"You can have a total of {maxBinds} pairs,");
                GU.Print("going up to this will automatically pass the selection through");

                string endSetting = Get_SB_Settings(maxBinds); // Call to get endSetting from seperate function
                GU.Print($"> {endSetting} <");
                switchBoard = new SwitchArray(defaultArraySize, endSetting, true);
                GU.Print("--");
                bool saveBoard = GU.GetBoolFromUser("Do you wish to save this Switchboad to memory?");
                if(saveBoard == true)
                {
                    switchBoard.SaveSwitchBoard(endSetting); // Prints 4 digit ID
                    GU.Print($"{switchBoard.GetID()} has been saved to memory!");
                }
            }
            else
            {
                int address = 0;
                bool validID = false;
                while(validID == false)
                {
                    GU.PrintContentsOfDirectory(FileLocationHandler.switchboard_R, true);
                    address = GU.GetIntWithinBound("Please enter the relevant 4-digit ID", 1000, 9999);
                    if (File.Exists(FileLocationHandler.switchboard_R + $"{address}.txt") == true) // Ensures the user can only enter a 4 digit number, and that this number correlates to a file
                    {
                        validID = true;
                    }
                    else
                    {
                        GU.Print("ERROR | Invalid file location, it doesn't exsist.");
                    }
                }
                switchBoard = new SwitchArray(defaultArraySize, $"{address}.txt"); // loads the switchboard with the pre saved data
                switchboardLog = FileSys.GetStringFromFile(FileLocationHandler.switchboard_R + $"{address}.txt");
            }
            GU.Print("");
        }
        private void InitialiseUKW()
        {
            bool check = true;
            char choice = ' ';
            while(check == true)
            {
                choice = GU.GetCharFromUser("Enter UKW / Reverser / Reflector of choice, A - B - C"); // There where three choices during the history of enigma M3.
                if(choice.ToString().ToUpper() is not("A" or "B" or "C"))
                {
                    GU.Print("ERROR | Please enter either A, B or C");
                }
                else
                {
                    check = false;
                }
            }
            ukwChosen = choice;
            ukw = new ReverserArray(defaultArraySize, $"reflector{choice}.txt"); // Initialies the reflector.

        }


        private void KeyByKeyEntry()
        { // Allows the user to enter a message a character at a time and updates a live feed.
            GU.Print("");
            GU.Print(": KeyByKey Entry :");

            int[] loggedShifts = { machineCogs[0].GetShift(), machineCogs[1].GetShift(), machineCogs[2].GetShift() };

            string startmessage = "";
            bool check = true;
            int menucount = 0;
            string message = "";
            while (check == true)
            {
                try
                {
                    if(menucount % 10 == 0)
                    {
                        GU.Print("Enter ! to stop and end"); // The user can stop 
                        GU.Print("Enter # to clear messaage"); // The user can clear the message
                        GU.Print("[EXPERIMENTAL] Enter @ to restart it to it's origional cog positions entered"); // The user can restart the machine
                        GU.Print("[EXPERIMENTAL] Enter $ to go back one character entry, to back parse the machine"); // The user can backspace
                    }
                    menucount++;

                    char input = GU.GetCharFromUser("Enter a character to parse through the machine > ", true);

                    if (input == '!')
                    {
                        GU.Print(" - END - ");
                        Console.WriteLine("");
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
                        Logging.Write("KeyByKeyEntry()", "User cleared entry and reset cogs to initial settings");

                    }

                    else if (input == '$')
                    {
                        // backparsing behaviour untested with real engima machines and relatively unknown, although it does work by principle here.
                        if (machineCogs[0].IsAtTurnover() == false && machineCogs[1].IsAtTurnover() == false)
                        {
                            machineCogs[0].DecrementCog();
                        }
                        else if (machineCogs[0].IsAtTurnover() == true && machineCogs[1].IsAtTurnover() == false)
                        {
                            machineCogs[0].DecrementCog();
                            machineCogs[1].DecrementCog();
                        }
                        else if (machineCogs[0].IsAtTurnover() == false && machineCogs[1].IsAtTurnover() == true)
                        {
                            foreach (CogArray C in machineCogs) { C.DecrementCog(); } //turns all
                        }
                        else if (machineCogs[0].IsAtTurnover() == true && machineCogs[1].IsAtTurnover() == true)
                        {
                            foreach (CogArray C in machineCogs) { C.DecrementCog(); } //turns all
                        }

                        message = message.Substring(0, message.Length-1);
                        startmessage = startmessage.Substring(0, startmessage.Length - 1);
                        GU.Print("");
                        GU.Print($"Current Message : {message}");
                        GU.Print("");
                    }

                    else if (CheckIfTraditionalCompatible(input) == false)
                    {
                        GU.Print("ERROR | Enter a valid character");
                    }

                    else
                    {
                        startmessage += input;
                        message += FullPassThrough(input);
                        GU.Print("");
                        GU.Print($"Current Message : {message}");
                        GU.Print("");
                    }
                       
                }
                catch // Unexpected to error, but there is a catch block as this is a long and complicated procedure.
                {
                    GU.DisplayErrorState(new Exception("Logic Error"), "Failure in keyBykey entry", "keyByKey()");
                }
            }
            endMessage = message;
            initialMessage = startmessage;
        }
        private void LiveRead()
        { // Takes a string, and puts it charater by character through the machine and gives the user the total result message.
            string input = "";
            GU.Print("");
            GU.Print(": Live Read :");
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
                    GU.Print("ERROR | Please enter a string that contains genuine enigma characters!");
                    GU.Print("");
                }
            }
            GU.Print("");

            string ret = "";
            int count = 0; 
            foreach(char y in input)
            {
                char got = FullPassThrough(Convert.ToChar(Convert.ToString(y).ToUpper()));
                ret += got;
                count++;
            }
            GU.Print("");
            GU.Print($"{ret}");
            initialMessage = input;
            endMessage = ret;

        }
        private void FileRead()
        { // Takes a string from a file and essentially does a liveread on the string derived from the file.
            GU.Print("");
            GU.Print(": File Read :");
            string writtenText = "";
            string locationOfText = "";
            bool acceptable = false;
            while (acceptable == false)
            {
                GU.PrintContentsOfDirectory(FileLocationHandler.messages_R);
                locationOfText = GU.GetStringFromUser("Please enter the file name of message (IF NOT IN \\messages enter \"!\")");
                
                if (locationOfText == "!")
                {
                    locationOfText = GU.GetStringFromUser("Enter the full address");
                }
                else
                {

                    locationOfText = $"{FileLocationHandler.messages_R}{locationOfText}.txt";
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
            
            // operations on files can take an extended period of time, whereass this would be unexpected with string entry,
            // as such there is a system here that will produce an EST if the operation is expected to take longer than 5 seconds.

            writtenText = FileSys.GetStringFromFile(locationOfText);

            GU.Print("");
            const double knownCharPerSecond = 11627.906976744185; // The speed of this operation.
            const double updateSum = knownCharPerSecond * 5; // 5 seconds of time
            string ret = "";
            TimeSpan estTime = TimeSpan.FromSeconds(1 / knownCharPerSecond).Multiply(writtenText.Length);
            if(writtenText.Length > updateSum)
            {
                GU.Print($"Est Time : {estTime.TotalSeconds} seconds");
            }

            foreach (char y in writtenText)
            {
                char got = FullPassThrough(Convert.ToChar(Convert.ToString(y).ToUpper()));
                ret += got;

            }

            if (writtenText.Length > updateSum)
            {
                GU.Print($"Text determined too large to print to console! > {updateSum:N0} characters");
            }
            else
            {
                GU.Print(writtenText);
                GU.Print("-- -- -- --");
                GU.Print("");
                GU.Print(ret);
            }

            initialMessage = writtenText;
            endMessage = ret;
        }


        private char FullPassThrough(char inflow)
        {// Cycles a character through the machine, updates all relative settings and relationships, then returns the updated character
            Logging.Write("------------------", "");
            int log = 0;
            char outflow = ' ';

            int current = GU.AlphaCharToIntIndex(inflow); //Starts by getting the index
            if (current == -33)
            {
                Logging.Write("FullPassThrough()", " - [SPACE] <Character Skipped>");
                return outflow;
            }
            else if (current < 0 || current > 25)
            {
                Logging.Write("FullPassThrough()", $"[{current}] <Non-Alphabetic Character Skipped>");
                return outflow;
            }



            // Brad's much improved enigma stepping code (flawless :) )


            if (machineCogs[0].IsAtTurnover() == false && machineCogs[1].IsAtTurnover() == false)
            {
                machineCogs[0].IncrementCog();
            }
            else if( machineCogs[0].IsAtTurnover() == true && machineCogs[1].IsAtTurnover() == false)
            {
                machineCogs[0].IncrementCog();
                machineCogs[1].IncrementCog();
            }
            else if(machineCogs[0].IsAtTurnover() == false && machineCogs[1].IsAtTurnover() == true)
            {
                foreach(CogArray C in machineCogs) { C.IncrementCog(); } //turns all
            }
            else if (machineCogs[0].IsAtTurnover() == true && machineCogs[1].IsAtTurnover() == true)
            {
                foreach (CogArray C in machineCogs) { C.IncrementCog(); } //turns all
            }


            int countA = 0;
            log = current;

            current = switchBoard.ForwardParse(current); // Goes through SwitchBoard first
            Logging.Write("FullPassThrough()", $" - {log}({GU.IntIndexToAlphaChar(log)}) > [Switchboard-F] > {current}({GU.IntIndexToAlphaChar(current)})");

            foreach (CogArray C in machineCogs) // Then cogs
            {
                log = current;
                current = C.ForwardParse(current);
                Logging.Write("FullPassThrough()", $" - {log}({GU.IntIndexToAlphaChar(log)}) > [COG {countA} -F] > {current}({GU.IntIndexToAlphaChar(current)})");
                countA++;
            }

            log = current;
            current = ukw.ForwardParse(current); // Then the Reverser
            Logging.Write("FullPassThrough()", $" - {log}({GU.IntIndexToAlphaChar(log)}) > [UKW] > {current}({GU.IntIndexToAlphaChar(current)})");

            for (int i = 2; i >= 0; i--) // Back through the cogs
            {
                log = current;
                current = machineCogs[i].ReverseParse(current); 
                Logging.Write("FullPassThrough()", $" - {log}({GU.IntIndexToAlphaChar(log)}) > [COG {i} -R] > {current}({GU.IntIndexToAlphaChar(current)})");
            }

            log = current;
            current = switchBoard.ReverseParse(current); // Back through the switchboard
            Logging.Write("FullPassThrough()", $" - {log}({GU.IntIndexToAlphaChar(log)}) > [Switchboard-R] > {current}({GU.IntIndexToAlphaChar(current)})");

            outflow = GU.IntIndexToAlphaChar(current); // Prints (this would be the lampboard on a physical machine)

            return outflow;
        }
   
        
        private string Get_SB_Settings(double maxBinds)
        {
            maxBinds--;
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
                        GU.Print("UPDATE | This process has been restarted!");
                        GU.Print("");
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
            {       // Prints to the user only the cogs within a list of unchosen cogs.
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
