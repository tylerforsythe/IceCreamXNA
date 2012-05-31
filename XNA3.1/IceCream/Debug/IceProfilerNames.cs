using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IceCream.Debug
{
    public enum IceProfilerNames : int
    {
        GAME_INITIALIZE,
        GAME_LOADCONTENT,
        GAME_UNLOADCONTENT,
        ICE_CORE_MAIN_UPDATE,
        ICE_CORE_RENDER,
        ICE_CORE_DRAW,
        ICE_CORE_INPUT_UPDATE
    }
}
