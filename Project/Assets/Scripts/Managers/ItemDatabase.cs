using System.Collections.Generic;
using System.IO;
using ItemSlots;
using Sirenix.OdinInspector;
using UnityEngine;
using Newtonsoft.Json;
using Sirenix.Serialization;
using UnityEditor;

public class ItemDatabase : MonoBehaviour
{
    public List<Item> itemList = new List<Item>();

    public static ItemDatabase Instance;

    private void Awake()
    {
        Instance = this;
        // DontDestroyOnLoad(Instance);
        
        itemList = new List<Item>();
        //AddDefaultItems();
        LoadItemDatabase();
    }

    // [Button("Add defaults")]
    private void AddDefaultItems()
    {
        itemList.Add(new Item(1, "Bald", "", "bald_head", Slot.Head));
        itemList.Add(new Item(2, "", "", "empty_neck", Slot.Neck));
        itemList.Add(new Item(3, "", "", "empty_shoulder_l", Slot.Left_Shoulder));
        itemList.Add(new Item(4, "", "", "empty_shoulder_r", Slot.Right_Shoulder));
        itemList.Add(new Item(5, "Naked", "", "naked_body", Slot.Body));
        itemList.Add(new Item(6, "", "", "empty_hand_l", Slot.Left_Hand));
        itemList.Add(new Item(7, "", "", "empty_hand_r", Slot.Right_Hand));
        itemList.Add(new Item(8, "", "", "empty_belt", Slot.Belt));
        itemList.Add(new Item(9, "", "", "empty_legs", Slot.Legs));
        itemList.Add(new Item(10, "", "", "empty_feet", Slot.Feet));
    }

    // [Button("Add Test Set")]
    private void AddTestSet()
    {
        itemList.Add(new Item(101, "", "", "buffon_head", Slot.Head, "Gear/Buffoon/Buffoon_Hat"));
        itemList.Add(new Item(601, "", "", "buffon_shoulder_l", Slot.Left_Hand, "Gear/Buffoon/L_Buffoon_Sleeve"));
        itemList.Add(new Item(701, "", "", "buffon_shoulder_r", Slot.Right_Hand, "Gear/Buffoon/R_Buffoon_Sleeve"));
        //itemList.Add(new Item(501, "", "", "buffon_body", Slot.Body, (GameObject) Resources.Load("Gear/Buffoon/Buffoon_Body")));
        itemList.Add(new Item(901, "", "", "buffon_legs", Slot.Legs, "Gear/Buffoon/Buffoon_Skirt"));
        itemList.Add(new Item(102, "", "", "knight_head", Slot.Head, "Gear/Knight/Knight_Helmet"));
    }

    // [Button("Clear List")]
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

    // [Button("Export List")]
    public void ExportList()
    {
        var list = JsonConvert.SerializeObject(itemList, Formatting.Indented);
        StreamWriter stream = new StreamWriter("Assets/Resources/itemDatabase.json");
        stream.Write(list);
        stream.Close();
    }

    [Button("Load Database")]
    public void LoadItemDatabase()
    {
        StreamReader stream;
        var itemsString = "";
        try
        {
            stream = new StreamReader("Assets/Resources/itemDatabase.json");
            itemsString = stream.ReadToEnd();
            stream.Close();
        }
        catch (FileNotFoundException e)
        {
            Debug.LogError("Item database file not found in Resources folder.");
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