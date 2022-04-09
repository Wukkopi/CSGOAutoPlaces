using System;
using System.Collections.Generic;
using System.Text;

namespace CSGOAutoPlaces.Nav
{
    enum HidingSpotAttribute
    {
        InCover         = 1,
        GoodSniperSpot  = 2,
        IdealSniperSpot = 4,
        Exposed         = 8
    }

    enum NavAreaAttributeBit : uint
    {
        Blank       = 0x00000000,
        Crouch      = 0x00000001,
        Jump        = 0x00000002,
        Precise     = 0x00000004,
        NoJump      = 0x00000008,
        Stop        = 0x00000010,
        Run         = 0x00000020,
        Walk        = 0x00000040,
        Avoid       = 0x00000080,
        Transient   = 0x00000100,
        DontHide    = 0x00000200,
        Stand       = 0x00000400,
        NoHostages  = 0x00000800,
        Stairs      = 0x00001000,
        NoMerge     = 0x00002000,
        ObstacleTop = 0x00004000,
        Cliff       = 0x00008000,
        FirstCustom = 0x00010000,
        LastCustom  = 0x04000000,
        FuncCost    = 0x20000000,
        HasElevator = 0x40000000,
        NavBlocker  = 0x80000000,
    }
}
