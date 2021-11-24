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
        protected int[] __shiftArray;
        protected int __size;
        protected string __cogFileLocation;

        public LookupArray(int size, string cogFileLocation)
        {
            __shiftArray = new int[size];
            this.__size = size;
            this.__cogFileLocation = cogFileLocation;

        }

        public virtual int ForwardParse(int letterIndex)
        {
            int x = (__shiftArray[(letterIndex % __size)] + letterIndex) % __size;
            return x;
        }

        protected virtual int[] FileToShiftArray()
        {
            int[] current = new int[__size];
            string key = "";
            StreamReader sr = new StreamReader(__cogFileLocation);
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
