using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices.WindowsRuntime;
using ItemSlots;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEditor;
using UnityEngine;

public class AccessoryManager : MonoBehaviour
{
    private GameObject avatar;

    [SerializeField] public TextAsset preset;

    // AQUI SE GUARDAN LAS INSTANCIAS ACTUALES DE CADA OBJETO. 
    // DESTRUIR ANTES DE EQUIPAR UNO NUEVO.
    [Title("Individual Slots")] public GameObject wornHead;
    public GameObject wornNeck;
    public GameObject wornShoulderLeft;
    public GameObject wornShoulderRight;
    public GameObject wornBody;
    public GameObject wornHandLeft;
    public GameObject wornHandRight;
    public GameObject wornBelt;
    public GameObject wornLegs;
    public GameObject wornFeet;

    [Title("All items")] public List<Item> equippedItems = new List<Item>();

    private int totalEquipmentSlots;

    private string presetPaths = "Assets/Resources/Presets/";

    // stitcher?

    public Stitcher Stitcher;

    private IEnumerator EquippingCoroutine;

    public void Start()
    {
        avatar = this.gameObject;
        Stitcher = new Stitcher();
        InitEquippedItemsList();
    }

    [Button("Init default")]
    public void InitEquippedItemsList()
    {
        // 
        totalEquipmentSlots = 10;

        for (int i = 0; i < totalEquipmentSlots; i++)
        {
            Item item = new Item();
            item.itemSlot = (Slot) (1 << i);
            equippedItems.Add(item);
            SetBase(i+1);
        }

        // Add a bunch of default items
        // SetBase(1);
        // SetBase(2);
        // SetBase(3);
        // SetBase(4);
        // SetBase(5);
        // SetBase(6);
        // SetBase(7);
        // SetBase(8);
        // SetBase(9);
        // SetBase(10);
    }

    public void SetBase(int id)
    {
        int idx = -1;
        Item item = ItemDatabase.Instance.GetItemByID(id);

        if (item == null) return;
        
        idx = equippedItems.FindIndex(x => x.itemSlot == item.itemSlot);
        if (idx < 0) return;
        
        RemoveEquipment(equippedItems[idx]);
        equippedItems[idx] = ItemDatabase.Instance.GetItemByID(id);
       
        // for (int i = 0; i < equippedItems.Count; i++)
        // {
        //     equippedItems[i] = ItemDatabase.Instance.GetItemByID(id);
        //     break;
        // }
    }
    public bool AddEquipmentToList(int id, ref Item equippableItem)
    {
        int idx = -1;
        Item item = ItemDatabase.Instance.GetItemByID(id);

        if (item == null) return false;
        idx = equippedItems.FindIndex(x => x.itemSlot == item.itemSlot);
        if (idx < 0) return false;
        RemoveEquipment(equippedItems[idx]);
        equippedItems[idx] = ItemDatabase.Instance.GetItemByID(id);
        equippableItem = equippedItems[idx];
        return true;
        // for (int i = 0; i < equippedItems.Count; i++)
        // {
        //     equippedItems[i] = ItemDatabase.Instance.GetItemByID(id);
        //     break;
        // }
    }

    void AddEquipment(Item eqToAdd)
    {
        if (eqToAdd == null) return;

        switch (eqToAdd.itemSlot)
        {
            case Slot.Head:
                AddEquipmentHelper(ref wornHead, eqToAdd);
                break;
            case Slot.Neck:
                AddEquipmentHelper(ref wornNeck, eqToAdd);
                break;
            case Slot.Left_Shoulder:
                AddEquipmentHelper(ref wornShoulderLeft, eqToAdd);
                break;
            case Slot.Right_Shoulder:
                AddEquipmentHelper(ref wornShoulderRight, eqToAdd);
                break;
            case Slot.Body:
                AddEquipmentHelper(ref wornBody, eqToAdd);
                break;
            case Slot.Left_Hand:
                AddEquipmentHelper(ref wornHandLeft, eqToAdd);
                break;
            case Slot.Right_Hand:
                AddEquipmentHelper(ref wornHandRight, eqToAdd);
                break;
            case Slot.Belt:
                AddEquipmentHelper(ref wornBelt, eqToAdd);
                break;
            case Slot.Legs:
                AddEquipmentHelper(ref wornLegs, eqToAdd);
                break;
            case Slot.Feet:
                AddEquipmentHelper(ref wornFeet, eqToAdd);
                break;
            default:
                Debug.Log("Incorrect slot!");
                throw new ArgumentOutOfRangeException();
        }
    }

    void RemoveEquipment(Item eqToAdd)
    {
        if (eqToAdd == null) return;

        switch (eqToAdd.itemSlot)
        {
            case Slot.Head:
                RemoveEquipmentHelper(ref wornHead, 0);
                break;
            case Slot.Neck:
                RemoveEquipmentHelper(ref wornNeck, 1);
                break;
            case Slot.Left_Shoulder:
                RemoveEquipmentHelper(ref wornShoulderLeft, 2);
                break;
            case Slot.Right_Shoulder:
                RemoveEquipmentHelper(ref wornShoulderRight, 3);
                break;
            case Slot.Body:
                RemoveEquipmentHelper(ref wornBody, 4);
                break;
            case Slot.Left_Hand:
                RemoveEquipmentHelper(ref wornHandLeft, 5);
                break;
            case Slot.Right_Hand:
                RemoveEquipmentHelper(ref wornHandRight, 6);
                break;
            case Slot.Belt:
                RemoveEquipmentHelper(ref wornBelt, 7);
                break;
            case Slot.Legs:
                RemoveEquipmentHelper(ref wornLegs, 8);
                break;
            case Slot.Feet:
                RemoveEquipmentHelper(ref wornFeet, 9);
                break;
            default:
                Debug.Log("Incorrect slot!");
                throw new ArgumentOutOfRangeException();
        }
    }

    //[Button("Equip Test Set")]
    public void EquipTestSet()
    {
        EquipItem(102);
    }


    [Button("Equip Item with ID")]
    public void EquipItem(int id)
    {
        if (id < 0) return;
        // try
        // {
        Item newItem = new Item();
        var validOperation = AddEquipmentToList(id, ref newItem);
        
        if (!validOperation || newItem.itemPrefab == null) return;
        AddEquipment(newItem);
        // }
        // catch (Exception e)
        // {
        //     Debug.Log(e.Message);
        // }
    }

    public void AddEquipmentHelper(ref GameObject wornItem, Item itemToAddToWornItem)
    {
        Wear(itemToAddToWornItem.itemPrefab, ref wornItem);
        wornItem.name = itemToAddToWornItem.itemSlug; // Superficial!
    }

    public void RemoveEquipmentHelper(ref GameObject wornItem, int nakedItemIndex)
    {
        RemoveWorn(wornItem);
        wornItem = null;
        equippedItems[nakedItemIndex] = ItemDatabase.Instance.GetItemByID(nakedItemIndex);
    }

    private void RemoveWorn(GameObject wornClothing)
    {
        try
        {
#if UNITY_EDITOR
            if (wornClothing != null)
                wornClothing.SetActive(false);
#else
            GameObject.Destroy(wornClothing);
#endif
        }
        catch (NullReferenceException)
        {
            return;
        }
    }

    private void Wear(GameObject prefab, ref GameObject wornClothing)
    {
        try
        {
            prefab = Instantiate(prefab);
            wornClothing = Stitcher.Stitch(prefab, avatar);
        }
        catch (NullReferenceException)
        {
            // ignored
        }
    }

    private string GetAllEquippedID()
    {
        string allIds = "";
        foreach (var item in equippedItems)
        {
            allIds += item.itemID.ToString();
            allIds += ",";
        }

        return allIds;
    }

    private void EquipMultipleItems(int[] list)
    {
        foreach (var item in list)
        {
            EquipItem(item);
        }
    }

    private void EquipMultipleItems(string[] list)
    {
        foreach (var item in list)
        {
            int.TryParse(item, out int result);
            EquipItem(result);
        }
    }

    [ButtonGroup("Store Preset")]
    public void StorePreset()
    {
        string path = presetPaths + "/" + preset.name + ".txt";
        StreamWriter writer = new StreamWriter(@path);
        //writer.WriteLine("Testing");
        writer.WriteLine(GetAllEquippedID());
        writer.Close();
    }

    [ButtonGroup("Load Preset")]
    public void LoadPreset()
    {
        string path = presetPaths + "/" + preset.name + ".txt";
        //AssetDatabase.ImportAsset(@path);
        StreamReader reader = new StreamReader(@path);
        //preset = Resources.Load(path) as TextAsset;
        string text = reader.ReadToEnd();

        var stringSeparated = text.Split(new[] {',', ' '});
        //EquipMultipleItems(stringSeparated);

        if (EquippingCoroutine != null)
            StopCoroutine(EquippingCoroutine);

        EquippingCoroutine = EquipItemsByFrame(stringSeparated);
        StartCoroutine(EquippingCoroutine);

        reader.Close();
    }

    IEnumerator EquipItemsByFrame(string[] list)
    {
        int i = 0;
        while (i < list.Length)
        {
            int.TryParse(list[i], out int result);
            EquipItem(result);
            i++;
            yield return null;
        }

        EquippingCoroutine = null;
    }
}