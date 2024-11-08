﻿using System;
using System.Collections.Generic;
using System.IO;
using Sirenix.OdinInspector;
using UnityEngine;
using Newtonsoft.Json;

public class ItemDatabase : MonoBehaviour
{
    public List<Item> itemList = new List<Item>();

    private static ItemDatabase _instance;

    public static ItemDatabase Instance
    {
        get { return _instance; }
        set
        {
            if (_instance != null)
            {
                Destroy(value);
                return;
            }

            _instance = value;
        }
    }

    private void Awake()
    {
        Instance = this;
        this.gameObject.transform.parent = null;
        DontDestroyOnLoad(Instance);

        itemList = new List<Item>();
        //AddDefaultItems();
        LoadItemDatabase();
    }

    // [Button("Add defaults")]
    // private void AddDefaultItems()
    // {
    //     itemList.Add(new Item(1, "Bald", "", "bald_head", Slot.Head));
    //     itemList.Add(new Item(2, "", "", "empty_neck", Slot.Neck));
    //     itemList.Add(new Item(3, "", "", "empty_shoulder_l", Slot.Left_Shoulder));
    //     itemList.Add(new Item(4, "", "", "empty_shoulder_r", Slot.Right_Shoulder));
    //     itemList.Add(new Item(5, "Naked", "", "naked_body", Slot.Body));
    //     itemList.Add(new Item(6, "", "", "empty_hand_l", Slot.Left_Hand));
    //     itemList.Add(new Item(7, "", "", "empty_hand_r", Slot.Right_Hand));
    //     itemList.Add(new Item(8, "", "", "empty_belt", Slot.Belt));
    //     itemList.Add(new Item(9, "", "", "empty_skirt", Slot.Skirt));
    //     itemList.Add(new Item(10, "", "", "empty_leg_l", Slot.Left_Leg));
    //     itemList.Add(new Item(11, "", "", "empty_leg_r", Slot.Right_Leg));
    //     itemList.Add(new Item(12, "", "", "empty_foot_l", Slot.Left_Foot));
    //     itemList.Add(new Item(13, "", "", "empty_foot_r", Slot.Right_Foot));
    // }
    //
    // // [Button("Add Test Set")]
    // private void AddTestSet()
    // {
    //     itemList.Add(new Item(101, "", "", "buffon_head", Slot.Head, "Gear/Buffoon/Buffoon_Hat"));
    //     itemList.Add(new Item(601, "", "", "buffon_shoulder_l", Slot.Left_Hand, "Gear/Buffoon/L_Buffoon_Sleeve"));
    //     itemList.Add(new Item(701, "", "", "buffon_shoulder_r", Slot.Right_Hand, "Gear/Buffoon/R_Buffoon_Sleeve"));
    //     itemList.Add(new Item(901, "", "", "buffon_legs", Slot.Skirt, "Gear/Buffoon/Buffoon_Skirt"));
    //     itemList.Add(new Item(102, "", "", "knight_head", Slot.Head, "Gear/Knight/Knight_Helmet"));
    // }
    //
    // // [Button("Clear List")]
    // public void ClearList()
    // {
    //     itemList = new List<Item>();
    // }

    public Item GetItemByID(int id)
    {
        return itemList.Find(x => x.itemID == id);
    }

    public Item GetItemBySlug(string slugName)
    {
        return itemList.Find(x => x.itemSlug == slugName);
    }

    public void ExportList()
    {
        var list = JsonConvert.SerializeObject(itemList, Formatting.Indented);
        StreamWriter stream = new StreamWriter("Assets/Resources/itemDatabase.json");
        stream.Write(list);
        stream.Close();
    }

    [Button("Force Load Database")]
    public void LoadItemDatabase()
    {
        var itemsString = "";
        string path = "itemDatabase.json";
        try
        {
#if UNITY_EDITOR
            StreamReader stream = new StreamReader("Assets/Resources/" + path);
            itemsString = stream.ReadToEnd();
            stream.Close();
#else
            // AssetDatabase.ImportAsset(path.Replace(".json", "");
            TextAsset preset = Resources.Load(path.Replace(".json", "")) as TextAsset;
            itemsString = preset.text;
#endif
        }
        catch (Exception e)
        {
            Debug.LogError(e.Message);
            return;
        }

        // Automatically generates a list for every item indented in the json file.
        // Handy!
        itemList = JsonConvert.DeserializeObject(itemsString, typeof(List<Item>)) as List<Item>;

        foreach (var item in itemList)
        {
            item.Load(); // Loads every Resource into a prefab object so it can be further instantiated.
        }
    }
}