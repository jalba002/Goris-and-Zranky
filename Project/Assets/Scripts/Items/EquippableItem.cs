using System;
using ItemSlots;
using UnityEngine;

// ReSharper disable once AccessToStaticMemberViaDerivedType
// ReSharper disable IdentifierTypo

namespace Items
{
    [System.Serializable]
    public class EquippableItem
    {
        public Item item;
        public GameObject instance;

        public EquippableItem()
        {
            item = new Item();
            instance = null;
        }

        public EquippableItem(Item item)
        {
            this.item = item;
        }

        public GameObject Create()
        {
            try
            {
                instance = GameObject.Instantiate(item.itemPrefab);
                instance.name = item.itemSlug;
            }
            catch (NullReferenceException)
            {
                // ignoradito
            }
            return instance;
        }

        public GameObject SetNewItem(Item newItem)
        {
            if (newItem.itemPrefab == null) return null;

            GameObject.Destroy(instance);

            item = newItem;
            return Create();
        }

        public void SetNewItem(int id)
        {
            // Or else
        }

        public int GetID()
        {
            return item.itemID;
        }

        public Slot GetSlot()
        {
            return item.itemSlot;
        }
    }
}
