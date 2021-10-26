using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ItemSlots
{
    // ReSharper disable InconsistentNaming

   // [System.Flags]
    [Serializable]
    public enum Slot
    {
        None = 0,
        Head = 1,
        Neck = 2,
        Left_Shoulder = 3,
        Right_Shoulder = 4,
        Body = 5,    
        Left_Hand = 6,
        Right_Hand = 7,
        Belt = 8,
        Skirt = 9,
        Left_Leg = 10,
        Right_Leg = 11,
        Left_Foot = 12,
        Right_Foot = 13,
    }
}