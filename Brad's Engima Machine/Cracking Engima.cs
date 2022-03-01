using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

namespace Brad_s_enigma_Machine
{
    class Fitness
    {
        private double indexOfCoincidence = 0;
        private const double englishIOC = 0.0667;
    
        private readonly string[][,] ngramFreqs_R = new string[6][,]; // stores known English Frequencies
        private double[] ngramIndex = new double[6];  // stores the results of our own analysis

        private string message;

        public Fitness(string message, bool fast = true)
        {
            this.message = message;
            message = GU.ConvertToScriptco(message); // Ensures the message only contains A-Z and is capitalised.
            Console.WriteLine(message);
            GU.Print("");

            ngramFreqs_R[1] = FileSys.GetFrequenciesFromFile(FileLocationHandler.unigramFrequencies_R);

            if(fast == false) // accurate (but slower) configs
            {
                ngramFreqs_R[2] = FileSys.GetFrequenciesFromFile(FileLocationHandler.accurateBiGram_R);
                ngramFreqs_R[3] = FileSys.GetFrequenciesFromFile(FileLocationHandler.accurateTriGram_R);
                ngramFreqs_R[4] = FileSys.GetFrequenciesFromFile(FileLocationHandler.accurateQuadGram_R);
                ngramFreqs_R[5] = FileSys.GetFrequenciesFromFile(FileLocationHandler.accurateQuintGram_R);
            }
            else
            {
                ngramFreqs_R[2] = FileSys.GetFrequenciesFromFile(FileLocationHandler.fastBiGram_R);
                ngramFreqs_R[3] = FileSys.GetFrequenciesFromFile(FileLocationHandler.fastTriGram_R);
                ngramFreqs_R[4] = FileSys.GetFrequenciesFromFile(FileLocationHandler.fastQuadGram_R);
                ngramFreqs_R[5] = FileSys.GetFrequenciesFromFile(FileLocationHandler.fastQuintGram_R);
            }

            UpdateIndexOfCoincidence();
            for(int i = 0; i < 5; i++)
            {
                UpdateFreqAnalysis(i);
            }
        }

        private void UpdateIndexOfCoincidence()
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
            indexOfCoincidence = Math.Abs(((temp-englishIOC)/temp)*100); // Difference to English IOC
        }

        private void UpdateFreqAnalysis(int sizeOfNgram) // Will be explained with depth in algoirhtm section
        {
            sizeOfNgram += 1;
            string[,] frequencies = ngramFreqs_R[sizeOfNgram];
            double messageLength = message.Length;
            double frequenciesLength = frequencies.GetLength(0);

            double collectiveDif = 0;

            GU.Print($" < Ngram({sizeOfNgram}) started >");
            TimeSpan avgPeriod = new TimeSpan();
            for (int j = 0; j < frequenciesLength; j++)
            {
                DateTime start = DateTime.UtcNow;

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

                // Time keeping ------------------------------------------------------------------------
                // *please note this isn't particularly nessecary just that it's very useful when operating testing to know an approximation.
                TimeSpan period = DateTime.UtcNow - start; // one operation completed in this period
                avgPeriod += period; // the avg period it takes to complete one operation
                TimeSpan timeRemaining = (frequenciesLength - j) * (avgPeriod.Divide(j+1));
                timeRemaining = timeRemaining.Multiply(1.2); // To account for inaccuracies in time keeping


                const int noOfUpdatesToUser = 3;
                if (j % ((int)(frequenciesLength) / noOfUpdatesToUser) == 0 && j != 0) //  26 is sudo prime as it's divisible by only 13 and 2, two primes. 
                {

                    GU.Print($" < Est Time Remaining on Ngram({sizeOfNgram}) = {timeRemaining.TotalSeconds:N0} sec > |");
                }
                // -------------------------------------------------------------------------------------

            }
            GU.Print($" < Ngram({sizeOfNgram}) finished! >");
            GU.Print($"---");
            ngramIndex[sizeOfNgram-1] = collectiveDif / (messageLength-sizeOfNgram) + 1;

        }

        private double GetNGramFreq(int n)
        {
            return ngramIndex[n-1];
        }

        public void PrintAllValues()
        {
            GU.Print("------");
            GU.Print($"UNIGRAM      = {GetNGramFreq(1)}");
            GU.Print($"BIGRAM       = {GetNGramFreq(2)}");
            GU.Print($"TRIGRAM      = {GetNGramFreq(3)}");
            GU.Print($"QUADGRAM     = {GetNGramFreq(4)}");
            GU.Print($"QUINTGRAM    = {GetNGramFreq(5)}");
            GU.Print($"IOC          = {Math.Round(indexOfCoincidence, 4)}% difference");
            GU.Print($"------");
            GU.Print("");
        }

    }
   

}
