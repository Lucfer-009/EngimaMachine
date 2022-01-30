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

            //Testing();



            

        }
        static void Testing()
        {
            string[,] tests =
            {
                {"Bible", FileSys.GetStringFromFile(FileLocationHandler.bible30chapters_R) },
                //{"Scrambled Bible", FileSys.GetStringFromFile(FileLocationHandler.bible30chaptersEnigma_R)},
                {"Lissa", "Lissa is the best"}

            };
            for (int i = 0; i < tests.GetLength(0); i++)
            {
                GU.Print($"{tests[i, 0]} ---------------");
                Fitness test = new Fitness(tests[i, 1]);

                GU.Print("------");
                GU.Print($"UNIGRAM  = {test.GetNGramFreq(1)}");
                GU.Print($"BIGRAM   = {test.GetNGramFreq(2)}");
                GU.Print($"TRIGRAM  = {test.GetNGramFreq(3)}");
                GU.Print($"QUADGRAM = {test.GetNGramFreq(4)}");
                GU.Print($"IOC = {Math.Round(test.indexOfCoincidence, 4)}% difference");
                GU.Print($"------\n\n");
            }

        }
    }
    
}
