using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Brad_s_Engima_Machine
{

    class SwitchArray : LookupArray
    {
        public SwitchArray(int size, string fileLocation) : base(size, fileLocation)
        {
            this.__keys = LoadKey();
            __pointerArray = LoadShifts(__keys[0]);
            __RpointerArray = LoadShifts(__keys[1]);

        }
        public SwitchArray(int size, string manualSettings, bool x) : base(size)
        {
            __pointerArray = LoadShifts(manualSettings);
            __RpointerArray = LoadShifts(InvertKey(manualSettings));

        }
    }

}
