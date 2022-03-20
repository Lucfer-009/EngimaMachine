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
            if (letterIndex < 0) //Takes the index of the character i.e A = 0,
            {
                letterIndex = __size - (Math.Abs(letterIndex) % __size); //Enesures the index is within 0 - __size.
            }
            int x = 0;
            int cCeaserShift = __pointerArray[(letterIndex + __shift) % __size]; // Takes the Ceasershift found at the pointer array of the index + the shift
            x = __size + cCeaserShift; // Applies this Ceasershift to the circular array.


            return (letterIndex + x) % __size; // returns the value found at the postiion of the shift.
        }

        public int ReverseParse(int letterIndex)
        { // Identical to ForwardParse but it calls a reversed dictionary
            if (letterIndex < 0)
            {
                letterIndex = __size - (Math.Abs(letterIndex) % __size);
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
            foreach (char y in manual) //Tkaes a string i.e XVFEBGI and turns them into an array of shitfs
            {
                current[count] = Convert.ToInt32(y) - 65 - count; // A = 0, Z = 25
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
