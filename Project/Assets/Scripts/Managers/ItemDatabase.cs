using System;
using System.Collections;
using System.Collections.Generic;
using ItemSlots;
using Sirenix.OdinInspector;
using UnityEngine;

public class ItemDatabase : MonoBehaviour
{
    public List<Item> itemList = new List<Item>();

    public static ItemDatabase Instance;

    private void Awake()
    {
        Instance = this;
        AddDefaultItems();
    }

    private void AddDefaultItems()
    {
        itemList.Add(new Item(0, "Bald", "", "bald_head", Slot.Head));
        itemList.Add(new Item(1, "", "", "empty_neck", Slot.Neck));
        itemList.Add(new Item(2, "", "", "empty_shoulders", Slot.Left_Shoulder));
        itemList.Add(new Item(3, "", "", "empty_shoulders", Slot.Right_Shoulder));
        itemList.Add(new Item(4, "Naked", "", "naked_body", Slot.Body));
        itemList.Add(new Item(5, "", "", "empty_hands", Slot.Left_Hand));
        itemList.Add(new Item(6, "", "", "empty_hands", Slot.Right_Hand));
        itemList.Add(new Item(7, "", "", "empty_belt", Slot.Belt));
        itemList.Add(new Item(8, "", "", "empty_legs", Slot.Legs));
        itemList.Add(new Item(9, "", "", "empty_feet", Slot.Feet));
    }

    [Button("Add Test Set")]
    private void AddTestSet()
    {
        itemList.Add(new Item(101, "", "", "buffon_head", Slot.Head, (GameObject) Resources.Load("Gear/Buffon/Buffon_Hat")));
        itemList.Add(new Item(301, "", "", "buffon_shoulder_l", Slot.Left_Shoulder, (GameObject) Resources.Load("Gear/Buffon/L_Buffon_Sleeve")));
        itemList.Add(new Item(401, "", "", "buffon_shoulder_r", Slot.Right_Shoulder, (GameObject) Resources.Load("Gear/Buffon/R_Buffon_Sleeve")));
        itemList.Add(new Item(501, "", "", "buffon_body", Slot.Body, (GameObject) Resources.Load("Gear/Buffon/Buffon_Body")));
        itemList.Add(new Item(801, "", "", "buffon_legs", Slot.Legs, (GameObject) Resources.Load("Gear/Buffon/Buffon_Skirt")));
        itemList.Add(new Item(102, "", "", "knight_head", Slot.Head, (GameObject)Resources.Load("Gear/Knight/Knight_Helmet")));
    }

    [Button("Clear List")]
    public void ClearList()
    {
        itemList = new List<Item>();
    }

    public Item GetItemByID(int id)
    {
        return itemList.Find(x => x.itemID == id);
    }

    public Item GetItemBySlug(string slugName)
    {
        return itemList.Find(x => x.itemSlug == slugName);
    }
}