﻿using System;
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

            //TurnFileIntoFreq(FileLocationHandler.knownEnglishBigrams_R, FileLocationHandler.newBiGram_R, 2, 500, 4);
            //TurnFileIntoFreq(FileLocationHandler.knownEnglishTrigrams_R, FileLocationHandler.newTriGram_R, 3, 1000, 6);
            //TurnFileIntoFreq(FileLocationHandler.knownEnglishQuadgrams_R, FileLocationHandler.newQuadGram_R, 4, 2000, 8);
            //TurnFileIntoFreq(FileLocationHandler.knownEnglishQuintgrams_R, FileLocationHandler.newQuintGram_R, 5, 4000, 10);
            Testing();





        }
        static void Testing()
        {
            string[,] testText =
            {
                {"Bible", FileSys.GetStringFromFile(FileLocationHandler.bible30chapters_R) },
                {"Scrambled Bible", FileSys.GetStringFromFile(FileLocationHandler.bible30chaptersEnigma_R)},
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
