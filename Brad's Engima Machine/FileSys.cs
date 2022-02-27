using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Brad_s_enigma_Machine
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

        public static string[,] GetFrequenciesFromFile(string location)
        {
            string[] lines = GetStringArrayFromFile(location);
            string[,] output = new string[lines.Length, 2];
            for(int i = 0; i < lines.Length; i++)
            {
                string[] temp = lines[i].Split('#');
                output[i, 0] = temp[0];
                output[i, 1] = temp[1];
            }
            return output;
        }
    }
}
