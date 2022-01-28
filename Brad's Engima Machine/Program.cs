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

            Fitness test = new Fitness();

            string exampleText_1 = GU.ConvertToScriptco(FileSys.GetStringFromFile(FileLocationHandler.knownEnglishWords_R));
            double bigramFreq = test.GetFrequencyAnalysis(exampleText_1, 2, test.knownBigramFreqs);
            double trigramFeq = test.GetFrequencyAnalysis(exampleText_1, 3, test.knownTrigramFreqs);
            double quadgramFreq = test.GetFrequencyAnalysis(exampleText_1, 4, test.knownQuadgramFreqs);

            GU.Print($"BI = {bigramFreq}");
            GU.Print($"TRI = {trigramFeq}");
            GU.Print($"QUAD = {quadgramFreq}");
            GU.Print($"TOTAL = {bigramFreq+trigramFeq+quadgramFreq}");
            GU.Print($"------");

        }
    }
    
}
