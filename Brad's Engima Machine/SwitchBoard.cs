using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Brad_s_Engima_Machine
{

    class SwitchArray : LookupArray
    {
        private const string defaultPath = @"F:\VS Projects\Project\Lucfer-009\EngimaMachine\Brad's Engima Machine\switchboard saves\";
        private static string ID;
        private static Random rand = new Random();

        public SwitchArray(int size, string fileLocation) : base(size, $"{defaultPath}{fileLocation}")
        {
            this.__keys = LoadKey();
            __pointerArray = LoadShifts(__keys[0]);
            __RpointerArray = LoadShifts(__keys[1]);
            ID = $"{rand.Next(1000, 9999)}"; // ID system. Assigns each instance of Machine a number.
        }
        public SwitchArray(int size, string manualSettings, bool x) : base(size)
        {
            __pointerArray = LoadShifts(manualSettings);
            __RpointerArray = LoadShifts(InvertKey(manualSettings));
            ID = $"{rand.Next(1000, 9999)}"; // ID system. Assigns each instance of Machine a number.
            __fileLocation = $"{defaultPath}{ID}.txt";
        }

        public void SaveSwitchBoard()
        {
            FileSys.WriteArrayToTxtFile(__pointerArray, __fileLocation);
        }
    }

}
