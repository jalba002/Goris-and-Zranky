using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices.WindowsRuntime;
using Items;
using ItemSlots;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEditor;
using UnityEngine;

public class AccessoryManager : MonoBehaviour
{
    private GameObject avatar;
    public bool loadPresetOnStart = false;

    [SerializeField] public TextAsset preset;

   [Title("All items")] public List<EquippableItem> equippedItems = new List<EquippableItem>();

    private int totalEquipmentSlots;

    public Stitcher Stitcher;

    private const string presetPaths = "Assets/Resources/Presets/";
    private IEnumerator EquippingCoroutine;

    public void Awake()
    {
        avatar = this.gameObject;
        Stitcher = new Stitcher();
    }

    public void Start()
    {
        if(loadPresetOnStart)
            LoadPreset();
    }

    [Button("Equip Item with ID")]
    public void EquipItem(int id)
    {
        if (id < 0) return;
        
        // Find if this item exists in the database.
        // Recover the exact item
        // Check if there is an existing item
        //     If so, call it to destroy the old one and set the new one (.SetNewItem() method)
        //     If not and there is no slot, create a new one.
        // Stitch the new gameobject to the correct place.

        var listedItem = ItemDatabase.Instance.GetItemByID(id);
        if (listedItem == null) return;

        var existingItem = equippedItems.Find(x => x.GetSlot() == listedItem.itemSlot);
        if (existingItem == null)
        {
            // Create a new one with the correspondant slot.
            existingItem = new EquippableItem(listedItem);
            equippedItems.Add(existingItem);
        }
        GameObject newGameObject = existingItem.SetNewItem(listedItem);
        if (newGameObject == null) return;
        
        Stitcher.Stitch(newGameObject, avatar);
    }

    private string GetAllEquippedID()
    {
        string allIds = "";
        foreach (var item in equippedItems)
        {
            allIds += item.GetID().ToString();
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