using System;
using System.Collections;
using System.Collections.Generic;
using ItemSlots;
using UnityEngine;

[System.Serializable]
public class Item
{
    public int itemID;
    public string itemName;
    public string itemDescription;
    public string itemSlug;
    public Slot itemSlot;
    public GameObject itemPrefab;
    [SerializeField] public string resourcePath;
    
    public Item(int itemID, string itemName, string itemDescription, string itemSlug, Slot itemSlot)
    {
        this.itemID = itemID;
        this.itemName = itemName;
        this.itemDescription = itemDescription;
        this.itemSlug = itemSlug;
        this.itemSlot = itemSlot;
        // this.itemPrefab = null;
    }
    public Item(int itemID, string itemName, string itemDescription, string itemSlug, Slot itemSlot, GameObject itemPrefab)
    {
        this.itemID = itemID;
        this.itemName = itemName;
        this.itemDescription = itemDescription;
        this.itemSlug = itemSlug;
        this.itemSlot = itemSlot;
        this.itemPrefab = itemPrefab;
    }
    
    public Item(int itemID, string itemName, string itemDescription, string itemSlug, Slot itemSlot, string resourcePath)
    {
        this.itemID = itemID;
        this.itemName = itemName;
        this.itemDescription = itemDescription;
        this.itemSlug = itemSlug;
        this.itemSlot = itemSlot;
        this.resourcePath = resourcePath;
        //this.itemPrefab = Resources.Load(resourcePath) as GameObject;
    }

    public void Load()
    {
        this.itemPrefab = Resources.Load(resourcePath) as GameObject;
    }

    public Item()
    {
        this.itemID = -1;
    }
}
