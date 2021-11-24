using System;
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

        public static void WriteStringToTxtFile(string message, string location)
        {
            StreamWriter sw = new StreamWriter(location);
            sw.WriteLine(message);
            sw.Close();
        }

    }
}
