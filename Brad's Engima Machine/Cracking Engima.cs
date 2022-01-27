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

        public void UpdateIndexOfBigrams(string message)
        {

        }
    }
   
    class KnownPlainText
    {
        public string word { get; set; }
        public int startIndex { get; set; }
    }
}
