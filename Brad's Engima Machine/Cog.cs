using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Brad_s_Engima_Machine
{
 
    class CogArray : LookupArray
    {
        private int shift;
        private int turnover;
        private const string defaultPath = @"C:\Users\Brad\Documents\GitHub\EngimaMachine\Brad's Engima Machine\cogs\";

        public CogArray(int shift, int size, string cogFileLocation) : base(size, $"{defaultPath}{cogFileLocation}")
        {
            this.shift = shift;
            __shiftArray = FileToShiftArray();
        }

        public override int ForwardParse(int letterIndex)
        {
            int x = (__shiftArray[(letterIndex + shift) % __size] + letterIndex) % __size;
            return x;
        }

        public int ReverseParse(int letterIndex)
        {
            int x = (__shiftArray[letterIndex - shift % __size] + letterIndex) % __size;
            return x;
        }

        public bool IncrementCog()
        {
            shift++;
            if(shift % __size == 0 )
            {
                shift = 0;
                return true;
            }
            else { return false; }
        }

        protected override int[] FileToShiftArray()
        {
            int[] current = new int[__size];
            StreamReader sr = new StreamReader(__cogFileLocation);
            string key = sr.ReadLine();
            sr.Close();

            string[] slice = key.Split(','); // Cog file needs to read the turnover from a file
            key = slice[1];
            turnover = GU.AlphaCharToIntIndex(Convert.ToChar(slice[0]));

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
