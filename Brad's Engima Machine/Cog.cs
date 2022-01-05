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
        private int turnover;
        private string cogFileLocation;
        private char initialChar;
        private int initialRingPos;

        public CogArray(int shift, int size, string cogFileLocation, char initialChar, int initialRingPos) : base(size, $"{FileLocationHandler.cogs_R}{cogFileLocation}")
        {
            __shift = shift;
            this.cogFileLocation = cogFileLocation;
            this.initialChar = initialChar;
            this.initialRingPos = initialRingPos;
            this.__keys = LoadKey();
            __pointerArray = LoadShifts(__keys[0]);
            __RpointerArray = LoadShifts(__keys[1]);
        }

        public bool IncrementCog()
        {
            __shift++;
            if(__shift == turnover+1 ) // Checks if the net shift (rotation) of the cog has reached the turnover latch,
            {                        // If it has it returns True, otherwise it returns false.
                return true;
            }
            else if(__shift % __size == 0)  // If the shift is above the size of the array it then resets it back to 0, despite the fact that there are %
            {                               // functions spread around indexing code to prevent such an error.
                __shift = 0;
            }
            return false;
        }

        public bool DecrementCog() // Experimental , likely not to be used
        {
            __shift--;
            if (__shift == turnover - 1)
            {       
                return true;
            }
            else if (__shift % __size == 0)  
            {                               
                __shift = 0;
            }
            return false;
        }

        protected override string[] LoadKey()
        {
            string[] localKeys = new string[2];
            StreamReader sr = new StreamReader(__fileLocation);
            string key_A = "";
            key_A = sr.ReadLine();
            sr.Close();

            // -- -- -- -- Custom for Cog, cog data requires that a turnover char be loaded in with the settings, as such the LoadKey is slightly different.
            string[] slice = key_A.Split(','); // Cog file needs to read the turnover from a file
            turnover = GU.AlphaCharToIntIndex(Convert.ToChar(slice[0]));
            key_A = slice[1];
            // -- -- -- -- -- -- -- -- -- -- --

            localKeys[0] = key_A; // Sets Standard pointer direction to localKey[0]
            localKeys[1] = InvertKey(key_A); // Sets inverted pointer direction to localKey[1]


            return localKeys;
        }

        public string GetCogLocation() { return cogFileLocation; }
        public string GetInitialRingPos() { return $"{initialRingPos}"; }
        public string GetInitialChar() { return $"{initialChar}"; }

    }
}
