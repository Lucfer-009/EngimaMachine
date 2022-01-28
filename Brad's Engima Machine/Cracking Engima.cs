using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

namespace Brad_s_Engima_Machine
{
    class Fitness
    {
        public double indexOfCoincidence = 0;
        public double indexOfBigrams = 0;
        public double indexOfTrigrams = 0;
        public double indexOfQuadgrams = 0;
        private List<KnownPlainText> WordsKnown = new List<KnownPlainText>();

        public readonly string[,] knownBigramFreqs;
        public readonly string[,] knownTrigramFreqs;
        public readonly string[,] knownQuadgramFreqs;
        public readonly string[,] knownCommonWordFreqs;

        public Fitness()
        {
            knownBigramFreqs = FileSys.GetFrequenciesFromFile(FileLocationHandler.bigramFrequencies_R);
            knownTrigramFreqs = FileSys.GetFrequenciesFromFile(FileLocationHandler.trigramFrequencies_R);
            knownQuadgramFreqs = FileSys.GetFrequenciesFromFile(FileLocationHandler.quadgramFrequencies_R);
            knownCommonWordFreqs = FileSys.GetFrequenciesFromFile(FileLocationHandler.commonWords_R);
        }

        public void UpdateIndexOfCoincidence(string message)
        {
            double length = message.Length;
            List<char> load = message.ToList();
            load.Sort();

            int noOfoccurances = 0;
            double temp = 0;
            for(int y = 0; y < 26; y++) // Cycles through all letters of the alphabet
            {
                noOfoccurances = 0;
                for (int x = 0; x < length; x++)
                {
                    if(load[x] == GU.IntIndexToAlphaChar(y)) // if the character is == A, then there has been 1x A.
                    {
                        noOfoccurances++;
                    }
                }
                temp += noOfoccurances / length * (noOfoccurances - 1) / (length - 1);
            }
            indexOfCoincidence = temp;
        }

        public double GetFrequencyAnalysis(string message, int n, string[,] frequencies)
        {
            double length = message.Length;
            List<char> load = message.ToList();
            load.Sort();

            int noOfoccurances = 0;
            double diff = 0;
            for (int y = 0; y < 26; y++) // Cycles through all letters of the alphabet
            {
                noOfoccurances = 0;
                for (int x = 0; x < length-n+1; x++)
                {
                    if ($"{message.Substring(x, n)}" == frequencies[y, 0] ) 
                    {
                        noOfoccurances++;
                    }
                }
                double frequencyV = ( noOfoccurances / ( (length - n)  + 1 ) ) * 100;
                if(frequencyV != 0)
                {
                    diff += (frequencyV / Convert.ToDouble(frequencies[y, 1])) * 100;
                }
                
            }
            return diff;
        }

        internal class KnownPlainText
        {
            public string word { get; set; }
            public int startIndex { get; set; }
        }
    }
   

}
