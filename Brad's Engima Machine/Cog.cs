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
            cogFileLocation += $"{cogChosen}.txt";
            int startShift = (currentPos - ringPos) % size;
            this.size = size;
            cypher = new CogArray(this, startShift);
        }


        public int GetSize() { return size; }
        public int GetTurnover() { return turnover; }
        public string GetCogFileLocation() { return cogFileLocation; }

    }

    class CogArray : LookupArray
    {
        private int shift;
        private int turnover;
        public CogArray(Cog click, int shift) : base(click.GetSize(), click.GetCogFileLocation())
        {
            this.shift = shift;
            shiftArray = FileToShiftArray();
        }

        protected override int forwardParse(int letterIndex)
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
            string key = "";
            StreamReader sr = new StreamReader(fileLocation);
            key = sr.ReadLine();
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
