using System;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using ItemSlots;
using Sirenix.OdinInspector;
using UnityEngine;

public class AcessoryManager_V2 : MonoBehaviour
{
    public GameObject avatar;

    // AQUI SE GUARDAN LAS INSTANCIAS ACTUALES DE CADA OBJETO. 
    // DESTRUIR ANTES DE EQUIPAR UNO NUEVO.
    public GameObject wornHead;
    public GameObject wornNeck;
    public GameObject wornShoulderLeft;
    public GameObject wornShoulderRight;
    public GameObject wornBody;
    public GameObject wornHandLeft;
    public GameObject wornHandRight;
    public GameObject wornBelt;
    public GameObject wornLegs;
    public GameObject wornFeet;

    public List<Item> equippedItems = new List<Item>();

    private int totalEquipmentSlots;

    // stitcher?

    public Stitcher Stitcher;

    public void Start()
    {
        Stitcher = new Stitcher();
        InitEquippedItemsList();
    }

    public void InitEquippedItemsList()
    {
        // 
        totalEquipmentSlots = 10;

        for (int i = 0; i < totalEquipmentSlots; i++)
        {
            Item item = new Item();
            item.itemSlot = (Slot) (1 << i);
            equippedItems.Add(item);
        }

        // Add a bunch of default items
        AddEquipmentToList(0);
        AddEquipmentToList(1);
        AddEquipmentToList(2);
        AddEquipmentToList(3);
        AddEquipmentToList(4);
        AddEquipmentToList(5);
        AddEquipmentToList(6);
        AddEquipmentToList(7);
        AddEquipmentToList(8);
        AddEquipmentToList(9);
    }

    public Item AddEquipmentToList(int id)
    {
        int idx = -1;
        Item item = ItemDatabase.Instance.GetItemByID(id);
        if (item == null) return null;
        idx = equippedItems.FindIndex(x => x.itemSlot == item.itemSlot);
        if (idx < 0) return null;
        RemoveEquipment(equippedItems[idx]);
        equippedItems[idx] = ItemDatabase.Instance.GetItemByID(id);
        return equippedItems[idx];
        // for (int i = 0; i < equippedItems.Count; i++)
        // {
        //     equippedItems[i] = ItemDatabase.Instance.GetItemByID(id);
        //     break;
        // }
    }

    public void AddEquipment(Item eqToAdd)
    {
        if (eqToAdd == null) return;
        
        switch (eqToAdd.itemSlot)
        {
            case Slot.Head:
                wornHead = AddEquipmentHelper(wornHead, eqToAdd);
                break;
            case Slot.Neck:
                wornNeck = AddEquipmentHelper(wornNeck, eqToAdd);
                break;
            case Slot.Left_Shoulder:
                wornShoulderLeft = AddEquipmentHelper(wornShoulderLeft, eqToAdd);
                break;
            case Slot.Right_Shoulder:
                wornShoulderRight = AddEquipmentHelper(wornShoulderRight, eqToAdd);
                break;
            case Slot.Body:
                wornBody = AddEquipmentHelper(wornBody, eqToAdd);
                break;
            case Slot.Left_Hand:
                wornHandLeft = AddEquipmentHelper(wornHandLeft, eqToAdd);
                break;
            case Slot.Right_Hand:
                wornHandRight = AddEquipmentHelper(wornHandRight, eqToAdd);
                break;
            case Slot.Belt:
                wornBelt = AddEquipmentHelper(wornBelt, eqToAdd);
                break;
            case Slot.Legs:
                wornLegs = AddEquipmentHelper(wornLegs, eqToAdd);
                break;
            case Slot.Feet:
                wornFeet = AddEquipmentHelper(wornFeet, eqToAdd);
                break;
            default:
                Debug.Log("Incorrect slot!");
                throw new ArgumentOutOfRangeException();
        }
    }
    
    public void RemoveEquipment(Item eqToAdd)
    {
        if (eqToAdd == null) return;
        
        switch (eqToAdd.itemSlot)
        {
            case Slot.Head:
                wornHead = RemoveEquipmentHelper(wornHead, 0);
                break;
            case Slot.Neck:
                wornNeck = RemoveEquipmentHelper(wornNeck, 1);
                break;
            case Slot.Left_Shoulder:
                wornShoulderLeft = RemoveEquipmentHelper(wornShoulderLeft, 2);
                break;
            case Slot.Right_Shoulder:
                wornShoulderRight = RemoveEquipmentHelper(wornShoulderRight, 3);
                break;
            case Slot.Body:
                wornBody = RemoveEquipmentHelper(wornBody, 4);
                break;
            case Slot.Left_Hand:
                wornHandLeft = RemoveEquipmentHelper(wornHandLeft, 5);
                break;
            case Slot.Right_Hand:
                wornHandRight = RemoveEquipmentHelper(wornHandRight, 6);
                break;
            case Slot.Belt:
                wornBelt = RemoveEquipmentHelper(wornBelt, 7);
                break;
            case Slot.Legs:
                wornLegs = RemoveEquipmentHelper(wornLegs, 8);
                break;
            case Slot.Feet:
                wornFeet = RemoveEquipmentHelper(wornFeet, 9);
                break;
            default:
                Debug.Log("Incorrect slot!");
                throw new ArgumentOutOfRangeException();
        }
    }

    [Button("Equip Test Set")]
    public void EquipTestSet()
    {
        EquipItem(102);
    }
    
    
    [Button("Equip Item with ID")]
    public void EquipItem(int id)
    {
        try
        {
            AddEquipment(AddEquipmentToList(id));
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
        }
    }

    public GameObject AddEquipmentHelper(GameObject wornItem, Item itemToAddToWornItem)
    {
        wornItem = Wear(itemToAddToWornItem.itemPrefab, wornItem);
        wornItem.name = itemToAddToWornItem.itemSlug; // Superficial!
        return wornItem;
    }

    public GameObject RemoveEquipmentHelper(GameObject wornItem, int nakedItemIndex)
    {
        wornItem = RemoveWorn(wornItem);
        equippedItems[nakedItemIndex] = ItemDatabase.Instance.GetItemByID(nakedItemIndex);
        return wornItem;
    }
    
    private GameObject RemoveWorn(GameObject wornClothing)
    {
        if (wornClothing == null) return null;
        GameObject.Destroy(wornClothing);
        return null;
    }

    private GameObject Wear(GameObject clothing, GameObject wornClothing)
    {
        if (clothing == null) return null;
        clothing = (GameObject) Instantiate(clothing);
        wornClothing = Stitcher.Stitch(clothing, avatar);
        //GameObject.Destroy(clothing);
        return wornClothing;
    }
}