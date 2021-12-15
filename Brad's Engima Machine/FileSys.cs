﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Brad_s_Engima_Machine
{
    static class FileSys
    {
        public static void WriteArrayToTxtFile(string[] array, string location)
        {
            StreamWriter sw = new StreamWriter(location);
            foreach(string y in array)
            {
                sw.WriteLine(y);
            }
            sw.Close();
        }

        public static void WriteArrayToTxtFile(int[] array, string location)
        {
            StreamWriter sw = new StreamWriter(location);
            foreach (int y in array)
            {
                sw.WriteLine(y);
            }
            sw.Close();
        }

        public static void WriteStringToTxtFile(string message, string location)
        {
            StreamWriter sw = new StreamWriter(location);
            sw.WriteLine(message);
            sw.Close();
        }

        public static string GetStringFromFile(string location)
        {
            StreamReader sr = new StreamReader(location);
            string x = sr.ReadToEnd();
            sr.Close();
            return x;
        }

        public static int[] GetIntArrayFromFile(string location)
        {
            string[] outputX = File.ReadAllLines(location);
            int[] outputY = new int[outputX.Length];
            int count = 0;
            foreach (string c in outputX)
            {
                outputY[count] = Convert.ToInt32(c);
                count++;
            }
            return outputY;

        }

        public static string[] GetStringArrayFromFile(string location)
        {
            string[] outputX = File.ReadAllLines(location);
            return outputX;
        }
    }
}
