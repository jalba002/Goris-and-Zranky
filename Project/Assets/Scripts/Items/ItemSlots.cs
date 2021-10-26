using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ItemSlots
{
    [System.Flags]
    [Serializable]
    public enum Slot
    {
        None = 0,
        Head = 1,
        Neck = 1<<1,
        Left_Shoulder = 1 << 2,
        Right_Shoulder = 1 << 3,
        Body = 1 << 4,
        Left_Hand = 1 << 5,
        Right_Hand = 1 << 6,
        Belt = 1 << 7,
        Legs = 1 << 8,
        Feet = 1 << 9,
    }
}