using System;
using System.Collections.Generic;
using System.IO;


namespace Brad_s_enigma_Machine
{
    class Program
    {
        static void Main(string[] args)
        {
            Instance A = new Instance();
            A.Start();
        }
        
    }
    class Instance
    { // Main overarching class that handles the loading of both the Machine class and the Fitness class.
        private static int next_ID = 0;
        public int ID;

        public Instance()
        {
            ID = next_ID; // Loads the ID
            next_ID++;
        }

        public void Start()
        {
            bool end = false;
            while (end == false) // Ensures this will loop if an invalid entry is given.
            {
                GU.Print("1. Enigma Machine");
                GU.Print("2. Enigma Decryption Program");
                GU.Print("3. End Program");
                GU.Print("--");
                char choice = GU.GetCharFromUser("Enter your choice", true);
                if (choice == '1')
                {
                    Machine enigma = new Machine(26); // Creates new instance of the machine
                    enigma.PowerOn(); // Starts the machine
                    Console.WriteLine("\n\n\n----------------------------------------\n");
                }
                else if (choice == '2')
                {
                    LoadCypherBreaking();
                    Console.WriteLine("\n\n\n----------------------------------------\n");
                }
                else if (choice == '3')
                {
                    end = true; // Ends the program
                }
                else
                {
                    GU.Print("ERROR | Enter a valid choice, 1-3"); // Entry error, anything with "ERROR |" means it will loop back to a user input
                }
            }
        }

        private void LoadCypherBreaking()
        { // The current class for handling fitness attacks on text (in early development)
            Fitness currentBreaker;
            int loadingChoice = -1;
            int speedChoice = -1;
            bool valid = false;
            while (valid == false) // Ensures an invalid response loops back to user input.
            {
                GU.Print("-- -- ");
                GU.Print("1. Load from file");
                GU.Print("2. Enter as string");
                GU.Print("");
                GU.Print("Please Note: The less characters your string posses,");
                GU.Print("the less accurate the result will be.");
                GU.Print("-- -- ");

                loadingChoice = GU.GetIntFromUser("Enter choice [1-2]");
                if (loadingChoice == 1 || loadingChoice == 2) // catch clause for invalid responses.
                {
                    valid = true;
                }
                else
                {
                    GU.Print("ERROR | Please enter a valid choice, 1-2.");
                    GU.Print("");
                }
            }
            GU.Print("");
            valid = false;
            while (valid == false) // regardless of file loading or string entry the user must select the focus
            {
                GU.Print("-- -- ");
                GU.Print("1. Accuracy Focus"); // larger dataset, 5-gram = 4000 entries
                GU.Print("2. Speed Focus"); // much smaller dataset, 5-gram = 512 entries
                GU.Print("");
                GU.Print("-- -- ");
                speedChoice = GU.GetIntFromUser("Enter choice [1-2]");
                if (speedChoice == 1 || speedChoice == 2)
                {
                    valid = true;
                }
                else
                {
                    GU.Print("ERROR | Please enter a valid choice, 1-2.");
                    GU.Print("");
                }
            }
            string writtenText;
            if (loadingChoice == 1) // Near identical code to File Read.
            {
                writtenText = "";
                string locationOfText = "";
                bool acceptable = false;
                while (acceptable == false)
                {
                    GU.PrintContentsOfDirectory(FileLocationHandler.cyphertextMessages_R); // Prints the contents of the messages folder
                    locationOfText = GU.GetStringFromUser("Please enter the file name of message (do NOT indlude the .txt)\n (IF NOT IN \\cracking\\messages enter !)");

                    if (locationOfText == "!") // gives the user the option to provide a full address
                    {
                        locationOfText = GU.GetStringFromUser("Enter the full address");
                    }
                    else
                    {
                        locationOfText = $"{FileLocationHandler.cyphertextMessages_R}{locationOfText}.txt";
                    }

                    if (File.Exists(locationOfText) == true) // Ensures the file exsists.
                    {
                        acceptable = true;
                    }
                    else
                    {
                        GU.Print("ERROR | Please enter a valid path, file doesn't exsist.");
                    }
                }

                writtenText = FileSys.GetStringFromFile(locationOfText);
            }
            else
            {
                writtenText = GU.GetStringFromUser("Enter the message");
            }


            if (speedChoice == 1)
            {
                currentBreaker = new Fitness(writtenText, false);
            }
            else
            {
                currentBreaker = new Fitness(writtenText);
            }
            currentBreaker.PrintAllValues();
        }


        // Development Procedures
        public void TurnFileIntoFreq(string file, string endFileLocation, double ngramSize, int noOfElements, int round)
        { // This is purely for testing use, it's a needed program to create the load files.
            double noOfNgrams = GetNoOfNGramsFromGivenFile(file);
            string[] text = FileSys.GetStringArrayFromFile(file);
            string[] final = new string[noOfElements];

            int count = 0;
            foreach (string line in text)
            {
                if (count == noOfElements) { break; }

                string[] temp = line.Split(" ");
                string ngram = temp[0];
                double noOfOccurances = Convert.ToDouble(temp[1]);

                final[count] = ngram + "#" + Convert.ToString(Math.Round((noOfOccurances / noOfNgrams) * 100, round));

                count++;
            }

            FileSys.WriteArrayToTxtFile(final, endFileLocation);
        }
        public double GetNoOfNGramsFromGivenFile(string file)
        {
            double total = 0;
            string[] text = FileSys.GetStringArrayFromFile(file);

            foreach (string line in text)
            {
                string[] temp = line.Split(" ");
                total += Convert.ToDouble(temp[1]);
            }
            return total;
        }


    }
    
}
