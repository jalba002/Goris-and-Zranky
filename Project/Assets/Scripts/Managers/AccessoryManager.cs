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

    [SerializeField] public TextAsset presetAsset;

    [Title("All items")] public List<EquippableItem> equippedItems = new List<EquippableItem>();

    public Stitcher Stitcher;

    private const string presetPaths = "Presets/";
    private IEnumerator EquippingCoroutine;

    public void Awake()
    {
        avatar = this.gameObject;
        Stitcher = new Stitcher();
    }

    public void Start()
    {
        if (loadPresetOnStart)
            LoadPreset();
    }

    [Button("Equip Set with ID")]
    public void EquipSet(int id)
    {
        string strID = id.ToString();
        if (id < 10)
            strID = "0" + id.ToString();

        string[] itemIDs = new string[14];
        for (int i = 1; i < 14; i++)
        {
            itemIDs[i] = i.ToString() + strID;
        }

        if (EquippingCoroutine != null)
            StopCoroutine(EquippingCoroutine);

        EquippingCoroutine = EquipItemsByFrame(itemIDs);
        StartCoroutine(EquippingCoroutine);
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

    // THIS NEEDS TO BE UPDATED SO IT WORKS WITH THE BUILDS
#if UNITY_EDITOR
    [ButtonGroup("Store Preset")]
    public void StorePreset()
    {
// THIS IN GAME IS THE EQUIVALENT OF LOADING THE CLOTHING PREFERENCES.
        string path = presetPaths + "/" + presetAsset.name + ".txt";
        StreamWriter writer = new StreamWriter(@path);
        writer.WriteLine(GetAllEquippedID());
        writer.Close();
    }
#endif

    [ButtonGroup("Load Preset")]
    public void LoadPreset()
    {
        string text = "";
        string path = presetPaths + this.presetAsset.name + ".txt";

#if UNITY_EDITOR
        StreamReader reader = new StreamReader("Assets/Resources/Presets/" + this.presetAsset.name + ".txt");
        text = reader.ReadToEnd();
        reader.Close();
#else
        // AssetDatabase.ImportAsset(path.Replace(".txt", ""));
        TextAsset preset = Resources.Load(path.Replace(".txt", "")) as TextAsset;
        text = preset.text;
#endif
        var stringSeparated = text.Split(new[] {',', ' '});
        //EquipMultipleItems(stringSeparated);

        if (EquippingCoroutine != null)
            StopCoroutine(EquippingCoroutine);

        EquippingCoroutine = EquipItemsByFrame(stringSeparated);
        StartCoroutine(EquippingCoroutine);
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