using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Brad_s_enigma_Machine
{
    class LookupArray
    {
        protected int __shift = 0;
        protected int[] __pointerArray;
        protected int[] __RpointerArray;
        protected string[] __keys;
        protected int __size;
        protected string __fileLocation;

        public LookupArray(int size, string cogFileLocation = null)
        {
            __size = size;
            __fileLocation = cogFileLocation;

        }

        public int ForwardParse(int letterIndex)
        {
            if (letterIndex < 0)
            {
                letterIndex = __size - (Math.Abs(letterIndex) % 26);
            }
            int x = 0;
            int cCeaserShift = __pointerArray[(letterIndex + __shift) % __size];
            x = __size + cCeaserShift;


            return (letterIndex + x) % __size;
        }

        public int ReverseParse(int letterIndex)
        {
            if (letterIndex < 0)
            {
                letterIndex = __size - (Math.Abs(letterIndex) % 26);
            }
            int x = 0;
            int cCeaserShift = __RpointerArray[(letterIndex + __shift) % __size];
            x = __size + cCeaserShift;


            return (letterIndex + x) % __size;
        }

        protected virtual string[] LoadKey()
        {
            string[] localKeys = new string[2];
            StreamReader sr = new StreamReader(__fileLocation);
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
                Rlive.Add(y, GU.IntIndexToAlphaChar(count));
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

        public int GetSize() { return __size; }
        public int GetShift() { return __shift; }
        public void SetShift(int y) { __shift = y; }
        public int[] GetPointers() { return __pointerArray; }
    }
}
