using System;
using System.IO;

namespace Brad_s_Engima_Machine
{
    class Program
    {
        static void Main(string[] args)
        {
            GU.Print("Hello World!");
            //Machine engima = new Machine("Test Machine", 26);
            //engima.PowerOn(); // Starts the machine






            //string exampleText_1 = FileSys.GetStringFromFile(FileLocationHandler.bible30chapters_R);
            string exampleText_1 = "ONCEWASAMANNAMEDDAVIDWHOLIVEDALONGROADNEARTHENEARESTSHOPPINGHALLHEREALLYLIKEDITWHENPEOPLETOOKHISYELLOWCOAT";
            Fitness test = new Fitness(exampleText_1);

            GU.Print($"BIGRAM = {test.GetNGramFreq(2)}");
            GU.Print($"UNIGRAM = {test.GetNGramFreq(1)}");
            GU.Print($"IOC = {Math.Round(test.indexOfCoincidence, 4)}% difference");
            GU.Print($"------");

        }
    }
    
}
