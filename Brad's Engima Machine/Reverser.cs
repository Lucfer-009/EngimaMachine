using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Brad_s_Engima_Machine
{

    class ReverserArray : LookupArray
    {
        private const string defaultPath = @"F:\VS Projects\Project\Lucfer-009\EngimaMachine\Brad's Engima Machine\ukws\";
        public ReverserArray(int size, string fileLocation) : base(size, $"{defaultPath}{fileLocation}")
        {
            this.__keys = LoadKey();
            __pointerArray = LoadShifts(__keys[0]);
            //__RpointerArray = LoadShifts(__keys[1]);  A Reverser is not symmetric, as such does not require a second, reversed, pass through
        }
    }
}
