using System;
using System.Collections.Generic;
using System.IO;


namespace Brad_s_enigma_Machine
{
    class Program
    {
        static void Main(string[] args)
        {
            Start();
            //For Testing and development use only.
            //TurnFileIntoFreq(FileLocationHandler.MSF_R + "english_bigrams.txt", FileLocationHandler.MSF_R + "bi_64.txt", 2, 64, 4); // Under Development.
            //TurnFileIntoFreq(FileLocationHandler.MSF_R + "english_trigrams.txt", FileLocationHandler.MSF_R + "tri_128.txt", 3, 128, 6);
            //TurnFileIntoFreq(FileLocationHandler.MSF_R + "english_quadgrams.txt", FileLocationHandler.MSF_R + "quad_256.txt", 4, 256, 8);
            //TurnFileIntoFreq(FileLocationHandler.MSF_R + "english_quintgrams.txt", FileLocationHandler.MSF_R + "quint_512.txt", 5, 512, 10);



        }
        static void Start()
        {
            
            bool end = false;
            while(end == false)
            {
                GU.Print("1. Enigma Machine");
                GU.Print("2. Enigma Decryption Program");
                GU.Print("3. End Program");
                GU.Print("--");
                char choice = GU.GetCharFromUser("Enter your choice", true);
                if(choice == '1')
                {
                    Machine enigma = new Machine(26); // Creates new instance of the machine
                    enigma.PowerOn(); // Starts the machine
                    Console.WriteLine("\n\n\n----------------------------------------\n");
                }
                else if(choice == '2')
                {
                    LoadCypherBreaking();
                    Console.WriteLine("\n\n\n----------------------------------------\n");
                }
                else if ( choice == '3')
                {
                    end = true;
                }
                else
                {
                    GU.Print("ERROR | Enter a valid choice, 1-3");
                }
                
            }


        }

        static void LoadCypherBreaking()
        {

            Fitness currentBreaker;
            int loadingChoice = -1;
            int speedChoice = -1;
            bool valid = false;
            while(valid == false)
            {
                GU.Print("-- -- ");
                GU.Print("1. Load from file");
                GU.Print("2. Enter as string");
                GU.Print("");
                GU.Print("Please Note: The less characters your string posses,");
                GU.Print("the less accurate the result will be.");
                GU.Print("-- -- ");

                loadingChoice = GU.GetIntFromUser("Enter choice [1-2]");
                if(loadingChoice == 1 || loadingChoice == 2)
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
            while(valid == false)
            {
                GU.Print("-- -- ");
                GU.Print("1. Accuracy Focus"); 
                GU.Print("2. Speed Focus");
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
            if(loadingChoice == 1)
            {
                writtenText = "";
                string locationOfText = "";
                bool acceptable = false;
                while (acceptable == false)
                {
                    GU.PrintContentsOfDirectory(FileLocationHandler.cyphertextMessages_R);
                    locationOfText = GU.GetStringFromUser("Please enter the file name of message (do NOT indlude the .txt)\n (IF NOT IN \\cracking\\messages enter !)");

                    if (locationOfText == "!")
                    {
                        locationOfText = GU.GetStringFromUser("Enter the full address");
                    }
                    else
                    {

                        locationOfText = $"{FileLocationHandler.cyphertextMessages_R}{locationOfText}.txt";
                    }

                    if (File.Exists(locationOfText) == true)
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

            if(speedChoice == 1)
            {
                currentBreaker = new Fitness(writtenText, false);
            }
            else
            {
                currentBreaker = new Fitness(writtenText);
            }
            currentBreaker.PrintAllValues();
        }

        static void TurnFileIntoFreq(string file, string endFileLocation, double ngramSize, int noOfElements, int round)
        { // This is purely for testing use, it's a needed program to create the load files.
            double noOfNgrams = GetNoOfNGramsFromGivenFile(file);
            string[] text = FileSys.GetStringArrayFromFile(file);
            string[] final = new string[noOfElements];

            int count = 0;
            foreach(string line in text)
            {
                if(count == noOfElements) { break; }

                string[] temp = line.Split(" ");
                string ngram = temp[0];
                double noOfOccurances = Convert.ToDouble(temp[1]);

                final[count] = ngram + "#" + Convert.ToString(Math.Round((noOfOccurances / noOfNgrams) * 100, round));

                count++;
            }

            FileSys.WriteArrayToTxtFile(final, endFileLocation);
        }
        static double GetNoOfNGramsFromGivenFile(string file)
        { // This is also a testing 
            double total = 0;
            string[] text = FileSys.GetStringArrayFromFile(file);

            foreach(string line in text)
            {
                string[] temp = line.Split(" ");
                total += Convert.ToDouble(temp[1]);
            }
            return total;
        }
    }
    
}
