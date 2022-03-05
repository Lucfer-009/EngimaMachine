using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Brad_s_enigma_Machine
{

    class SwitchArray : LookupArray
    {
        private string ID;
        private Random rand = new Random();

        public SwitchArray(int size, string fileLocation) : base(size, $"{FileLocationHandler.switchboard_R}{fileLocation}")
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
            __fileLocation = $"{FileLocationHandler.switchboard_R}{ID}.txt";
        }

        public void SaveSwitchBoard(string save)
        {
            FileSys.WriteStringToTxtFile(save, __fileLocation);
        }
        
        public string GetID()
        {
            return ID;
        }
    }

}
