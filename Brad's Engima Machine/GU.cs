using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Brad_s_enigma_Machine
{
    static class GU // Short for General Usage
    {
        // This servers as a collection of commonly used operations to allow for; consistent
        // error handling and data usage, removing the likelhood of differences in how common
        // operations are exceuted, as well as reducing the level of code reuse.
        // --
        // Names of procedures should be explination enough as to their purpose. Although, further annotaion is given if needed.

        public static readonly double version = 1.1;

        public static void DisplayErrorState(Exception e, string error, string section, string note = "N/A")
        {
            Console.WriteLine("|| {0, -80} {1, 3}", $"A CRITICAL ERROR HAS OCCURED : {error}", "||");
            Console.WriteLine("|| {0, -80} {1, 3}", $"PROCEDURE OF ERROR : {section}", "||");
            Console.WriteLine("|| {0, -80} {1, 3}", $"NOTE WITHIN CODE : {note}", "||");
            Console.WriteLine("--\n  {0, -80} {1, 3}", $"EXCEPTION DATA : {e}", "\n--");
        }



        public static void Print(string message)
        {
            Console.WriteLine("|| {0, -95} ||", message);
        }

        public static void Header(string message = "")
        {
            Console.Write($"|| [{message}] >> ");
        }


        // For ease of programer use, X is always the name of the return variable in these procedures. y is the secondary variable, if used.
        public static string GetStringFromUser(string message)
        {
            string X = "";

            bool check = true;
            while (check == true)
            {
                try
                {
                    Header(message);
                    X = Console.ReadLine();
                    check = false;
                }
                catch(Exception e)
                {
                    DisplayErrorState(e, "Console.ReadLine()", "GeneralUse.GetStringFromUser()");
                }
            }
            return X;
        }

        public static char GetCharFromUser(string message, bool upper = false)
        {
            char X = ' ';

            bool check = true;
            while (check == true)
            {
                try
                {
                    string y = "";
                    if(upper == false)
                    {
                        y = GetStringFromUser(message);
                    }
                    else
                    {
                        y = GetStringFromUser(message).ToUpper();
                    }
                    
                    X = Convert.ToChar(y);
                    check = false;
                }
                catch
                {
                    Print("ERROR | Please ensure you've entered a character, not a string or an integer");
                }
            }
            return X;
        }

        public static int GetIntFromUser(string message)
        {
            int X = 0;

            bool check = true;
            while (check == true)
            {
                try
                {
                    Header(message);
                    X = Convert.ToInt32(Console.ReadLine());
                    check = false;
                }
                catch
                {
                    Print("ERROR | Please ensure you enter an integer value!");
                }
            }
            return X;
        }

        public static double GetDoubleFromUser(string message, int decimalP = -1)
        {
            double X = 0;

            bool check = true;
            while (check == true)
            {
                try
                {
                    Header(message);
                    X = Convert.ToDouble(Console.ReadLine());
                    check = false;
                }
                catch
                {
                    Print("ERROR | Please ensure you enter a double value!");
                }
            }


            if(decimalP == -1) 
            { 
                return X; 
            }
            else
            {
                return Math.Round(X, decimalP);
            }
            
        }

        public static bool GetBoolFromUser(string message)
        {
            bool X = false;
            string[] yesValues = { "yes", "y", "confirm", "affirmative", "1", "true" };
            string[] noValues = { "no", "n", "deny", "unaffirmative", "0", "false" };


            bool check = true;
            while (check == true)
            {
                try
                {
                    string y = GetStringFromUser(message).ToLower();
                    if(y == "-readout" || y == "- readout")
                    {
                        Print("------ ------");
                        Print("       YES / TRUE values ;");
                        PrintStrArrayToConsole(yesValues);
                        Print("------ ------");
                        Print("       NO / FALSE values ;");
                        PrintStrArrayToConsole(noValues);
                        Print("------ ------");
                    }

                    if(yesValues.Contains(y) == true) 
                    { 
                        X = true;
                        check = false;
                    }
                    else if (noValues.Contains(y) == true) 
                    {
                        X = false;
                        check = false;
                    }
                    else 
                    {
                        Print("ERROR | Please ensure you enter an acceptable boolean response (i.e. Y/N)!");
                        Print("      | entering \"-readout\" will proivde you with a list of releveant responses");
                    }

                    
                }
                catch
                {
                    Print("ERROR | Please ensure you enter an acceptable boolean response (i.e. Y/N)!");
                    Print("      | entering \"-readout\" will proivde you with a list of releveant responses");
                }
            }
            return X;
        }

        public static int GetIntWithinBound(string message, int upperB, int lowerB)
        {
            int X = 0;

            bool check = true;
            while (check == true)
            {
                try
                {
                    int y = GetIntFromUser(message);
                    if(!(y >= upperB & y <= lowerB))
                    {
                        Print("ERROR | Please ensure you enter an integer value within the specified bounds !");
                        Print($"      | {upperB} >= y >= {lowerB}");
                    }
                    else
                    {
                        X = y;
                        check = false;
                    }
                   
                }
                catch
                {
                    Print("ERROR | Please ensure you enter an INTEGER value within the specified bounds !");
                    Print($"      | {upperB} >= y >= {lowerB}");
                }
            }
            return X;
        }



        public static void PrintStrArrayToConsole(string[] x)
        {
            foreach(string element in x)
            {
                Print($"- {element}");
            }
        }
        public static void PrintStrArrayToConsole(string[,] x)
        {
            for(int i = 0; i < x.GetLength(0); i++)
            {
                string temp = "";
                for(int j = 0; j < x.GetLength(1);j++)
                {
                    temp += $" - {x[i, j]}";
                }
                Print(temp);
            }
        }


        public static void PrintIntArrayToConsole(int[] x)
        {
            foreach (int element in x)
            {
                Print($"- {element}");
            }
        }



        public static int AlphaCharToIntIndex(char x, int indexingShift = -65)
        {
            int X = 0;
            try
            {
                X = Convert.ToInt32(x);
                X = X + indexingShift; // Sets 'A' to 0 and 'Z' to 25 
            }                          // Defualt indexshift of -65.
            catch(Exception e)
            {
                DisplayErrorState(e, "Error in converting number to indexing figure", "GeneralUse.AlphaCharToIntIndex()");
            }

            return X;
        }

        public static char IntIndexToAlphaChar(int y, int indexingShiftR = 65)
        {
            char X = ' ';
            try
            {
                y = y + indexingShiftR;
                X = Convert.ToChar(y);
            }                          
            catch (Exception e)
            {
                DisplayErrorState(e, "Error in converting indexing figure to character", "GeneralUse.IntIndexToAlphaChar()");
            }

            return X;
        }

        public static string ConvertToScriptco(string message)
        {
            string X = "";
            foreach(char y in message)
            {
                int asciiValue = Convert.ToInt32(y);
                if(asciiValue >= 97 && asciiValue <= 122) // checks for lowercase characters and makes them uppercase
                {
                    asciiValue -= 32;
                }

                if(asciiValue >= 65 && asciiValue <= 90)
                {
                    X += Convert.ToChar(asciiValue);
                }
            }
            return X;
        }

        public static void PrintContentsOfDirectory(string directoryPath, bool preview = false)
        {
            GU.Print("-- Contents of directory --");
            foreach (var path in Directory.GetFiles(directoryPath))
            {
                string fileName = Path.GetFileName(path);
                if(fileName.Contains(".git") == false )
                {
                    string[] division = fileName.Split('.');
                    if( preview == true)
                    {
                        string content = FileSys.GetStringFromFile(path);
                        GU.Print($">  {division[0]} (.{division[1]}) | {content}");
                    }
                    else
                    {
                        GU.Print($">  {division[0]} (.{division[1]})");
                    }
                    
                }

            }
            GU.Print("-- -- --");
        }
    }
}
