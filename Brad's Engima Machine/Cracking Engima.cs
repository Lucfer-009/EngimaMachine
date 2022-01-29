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
        private const double englishIOC = 0.0667;

        private List<KnownPlainText> WordsKnown = new List<KnownPlainText>();
        
        private readonly string[][,] ngramFreqs_R = new string[5][,]; // stores known English Frequencies
        private double[] ngramIndex = new double[5];  // stores the results of our own analysis

        private string message;

        public Fitness(string message)
        {
            this.message = message;
            ngramFreqs_R[0] = FileSys.GetFrequenciesFromFile(FileLocationHandler.commonWords_R); // = 0 in array, n = 1 when refrenced
            ngramFreqs_R[1] = FileSys.GetFrequenciesFromFile(FileLocationHandler.unigramFrequencies_R);
            ngramFreqs_R[2] = FileSys.GetFrequenciesFromFile(FileLocationHandler.bigramFrequencies_R); // = 1 in array, n = 2 when refrenced 
            ngramFreqs_R[3] = FileSys.GetFrequenciesFromFile(FileLocationHandler.trigramFrequencies_R); // etc...
            ngramFreqs_R[4] = FileSys.GetFrequenciesFromFile(FileLocationHandler.quadgramFrequencies_R);

            //---
            UpdateIndexOfCoincidence();
            for(int i = 0; i < 5; i++)
            {
                UpdateFreqAnalysis(i);
            }
        }

        public void UpdateIndexOfCoincidence() // WORKS !
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
            //indexOfCoincidence = temp;
            indexOfCoincidence = Math.Abs(((temp-englishIOC)/temp)*100); // Difference to English IOC
        }


        public void UpdateFreqAnalysis(int sizeOfNgram) // Will be explained soon
        {
            string[,] frequencies = ngramFreqs_R[sizeOfNgram];
            double messageLength = message.Length;
            int frequenciesLength = frequencies.GetLength(0);

            double collectiveDif = 0;

            for(int j = 0; j < frequenciesLength; j++)
            {
                int noOfOccurances = 1;
                double expectedFreq = Convert.ToDouble(frequencies[j, 1]);

                for(int v = 0; v < messageLength-sizeOfNgram; v++)
                {
                    string currentN = message.Substring(v, sizeOfNgram);
                    if(currentN == frequencies[j, 0])
                    {
                        noOfOccurances++;
                    }

                }
                double frequency = (noOfOccurances / ((messageLength - sizeOfNgram) + 1)) * 100;
                collectiveDif += Math.Abs(((expectedFreq-frequency) / frequency ) *100);

            }
            ngramIndex[sizeOfNgram] = collectiveDif / (messageLength-sizeOfNgram) + 1;

        }

        public double GetNGramFreq(int n)
        {
            return ngramIndex[n];
        }


        internal class KnownPlainText
        {
            public string word { get; set; }
            public int startIndex { get; set; }
        }
    }
   

}
