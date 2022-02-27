using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Brad_s_enigma_Machine
{

    class ReverserArray : LookupArray
    {
        public ReverserArray(int size, string fileLocation) : base(size, $"{FileLocationHandler.reversers_R}{fileLocation}")
        {
            this.__keys = LoadKey();
            __pointerArray = LoadShifts(__keys[0]);
            //__RpointerArray = LoadShifts(__keys[1]);  A Reverser is not symmetric, as such does not require a second, reversed, pass through
        }
    }
}
