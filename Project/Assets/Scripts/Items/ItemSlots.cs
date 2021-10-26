using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ItemSlots
{
    // ReSharper disable InconsistentNaming

    [System.Flags]
    [Serializable]
    public enum Slot
    {
        None = 0,                   // 0
        Head = 1,                   // 1
        Neck = 1<<1,                // 2
        Left_Shoulder = 1 << 2,     // 4
        Right_Shoulder = 1 << 3,    // 8
        Body = 1 << 4,              // 16
        Left_Hand = 1 << 5,         // 32
        Right_Hand = 1 << 6,        // 64
        Belt = 1 << 7,              // 128
        Legs = 1 << 8,              // 256
        Feet = 1 << 9,              // 512
    }
}