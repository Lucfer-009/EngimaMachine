using System;
using System.Collections.Generic;
using System.IO;


namespace Brad_s_Engima_Machine
{
    class Program
    {
        static void Main(string[] args)
        {
            //GU.Print("Hello World!");
            //Machine engima = new Machine("Test Machine", 26);
            //engima.PowerOn(); // Starts the machine

            TurnFileIntoFreq(FileLocationHandler.MSF_R + "english_bigrams.txt"      , , 2, 300, 4);
            TurnFileIntoFreq(FileLocationHandler.MSF_R + "english_trigrams.txt"     , , 3, 500, 6);
            TurnFileIntoFreq(FileLocationHandler.MSF_R + "english_quadgrams.txt"    , , 4, 700, 8);
            TurnFileIntoFreq(FileLocationHandler.MSF_R + "english_quintgrams.txt"   , , 5, 900, 10);
            Testing();

        }
        static void Start()
        {
            
            bool end = false;
            while(end == false)
            {
                GU.Print("1. Engima Machine");
                GU.Print("2. Engima Decryption Program");
                GU.Print("3. End Program");
                GU.Print("--");
                char choice = GU.GetCharFromUser("Enter your choice", true);
                if(choice == '1')
                {
                    Machine engima = new Machine("Test Machine", 26);
                    engima.PowerOn(); // Starts the machine
                    Console.WriteLine("\n\n\n\n\n\n\n\n----------------------------------------\n\n");
                }
                else if(choice == '2')
                {
                    Testing();
                    Console.WriteLine("\n\n\n\n\n\n\n\n----------------------------------------\n\n");
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

        static void Testing()
        {
            string[,] testText =
            {
                {"Bible", FileSys.GetStringFromFile(FileLocationHandler.bible30chapters_R) },
                {"Scrambled Bible", FileSys.GetStringFromFile(FileLocationHandler.bible30chaptersRand_R)},
                {"Lissa", "Lissa is the best"}

            };
            Fitness[] testInstances = new Fitness[3];
            for (int i = 0; i < testText.GetLength(0); i++)
            {
                testInstances[i] = new Fitness(testText[i, 1], testText[i, 0]);
            }
            foreach(Fitness y in testInstances)
            {
                y.PrintAllValues();
            }
        }



        static void TurnFileIntoFreq(string file, string endFileLocation, double ngramSize, int noOfElements, int round)
        {
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
        {
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
