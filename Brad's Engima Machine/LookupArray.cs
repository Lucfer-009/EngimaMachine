using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Brad_s_Engima_Machine
{
    class LookupArray
    {
        protected int[] shiftArray;
        protected int size;
        protected string fileLocation;

        public LookupArray(int size, string fileLocation)
        {
            shiftArray = new int[size];
            shiftArray = FileToShiftArray();
            this.size = size;
            this.fileLocation = fileLocation;
        }

        protected virtual int forwardParse(int letterIndex)
        {
            int x = (shiftArray[(letterIndex % size)] + letterIndex) % size;
            return x;
        }

        protected virtual int[] FileToShiftArray()
        {
            int[] current = new int[size];
            string key = "";
            StreamReader sr = new StreamReader(fileLocation);
            key = sr.ReadLine();
            sr.Close();

            int count = 0;
            foreach (char y in key)
            {
                current[count] = Convert.ToInt32(y) - 65 - count;
                count++;
            }

            return current;
        }
    }
}
