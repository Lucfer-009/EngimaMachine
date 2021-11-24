using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Brad_s_Engima_Machine
{
    class Cog
    {
        private int size;
        private string cogFileLocation = @"F:\VS Projects\Project\Lucfer-009\EngimaMachine\Brad's Engima Machine\cogs\";
        private CogArray cypher;

        public Cog(int size, int cogChosen, int currentPos, int ringPos)
        {
            cogFileLocation += $"cog{cogChosen}.txt";
            int startShift = (currentPos - ringPos) % size;
            this.size = size;
            cypher = new CogArray(startShift, size, cogFileLocation);
        }

        public char Parse(char _in, bool forward)
        {
            char _out = ' ';
            int v;
            if (forward == true)
            {
                v = cypher.ForwardParse(GU.AlphaCharToIntIndex(_in));
            }
            else
            {
                v = cypher.reverseParse(GU.AlphaCharToIntIndex(_in));
            }

            _out = GU.IntIndexToAlphaChar(v);
            return _out;
        }


        public int GetSize() { return size; }
        public string GetCogFileLocation() { return cogFileLocation; }

    }

    class CogArray : LookupArray
    {
        private int shift;
        private int turnover;
        public CogArray(int shift, int size, string fileLocation) : base(size, fileLocation)
        {
            this.shift = shift;
        }

        public override int ForwardParse(int letterIndex)
        {
            int x = (shiftArray[(letterIndex + shift) % size] + letterIndex) % size;
            return x;
        }

        public int reverseParse(int letterIndex)
        {
            int x = (shiftArray[letterIndex - shift % size] + letterIndex) % size;
            return x;
        }

        protected override int[] FileToShiftArray()
        {
            int[] current = new int[size];
            StreamReader sr = new StreamReader(cogFileLocation);
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
