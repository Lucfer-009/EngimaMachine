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
        protected int __shift = 0;
        protected int[] __pointerArray;
        protected int[] __RpointerArray;
        protected string[] __keys;
        protected int __size;
        protected string __cogFileLocation;

        public LookupArray(int size, string cogFileLocation = null)
        {
            this.__size = size;
            this.__cogFileLocation = cogFileLocation;

        }

        public int ForwardParse(int letterIndex)
        {
            int x = (__pointerArray[(letterIndex + __shift) % __size] + letterIndex) % __size; // Gets shift with __shiftArray[letterIndex] then applies to intitial index, then ensures doesn't go out of bounds
            return x;
        }

        public int ReverseParse(int letterIndex)
        {
            int x = (__RpointerArray[(letterIndex + __shift) % __size] + letterIndex) % __size;
            return x;
        }

        protected virtual string[] LoadKey()
        {
            string[] localKeys = new string[2];
            StreamReader sr = new StreamReader(__cogFileLocation);
            string key_A = "";
            key_A = sr.ReadLine();
            sr.Close();

            localKeys[0] = key_A; // Sets Standard pointer direction to localKey[0]
            localKeys[1] = InvertKey(key_A); // Sets inverted pointer direction to localKey[1]
            

            return localKeys;
        }

        protected string InvertKey(string standard)
        {
            string inverted = "";
            SortedDictionary<char, char> Rlive = new SortedDictionary<char, char>();  // Inverts the relationship between the pointers and then sorts them so they're in the same format ready.
            int count = 0;
            foreach (char y in standard)
            {
                Rlive.Add(y, Convert.ToChar(count + 65));
                count++;
            }
            foreach (KeyValuePair<char, char> y in Rlive)
            {
                inverted += $"{y.Value}";
            }
            return inverted;
        }

        protected virtual int[] LoadShifts(string manual)
        {
            int[] current = new int[__size];
            int count = 0;
            foreach (char y in manual)
            {
                current[count] = Convert.ToInt32(y) - 65 - count;
                count++;
            }

            return current;
        }
    }
}
