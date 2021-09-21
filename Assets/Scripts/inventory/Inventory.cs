using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Runtime.Serialization;
using System;

public enum InterfaceType
{
    Inventory, 
    Equipment,
    chest,
    storageChest,
    shop
}


[CreateAssetMenu(fileName = "New Inventory", menuName = "Inventory system/Inventory")]
public class Inventory : ScriptableObject
{
    public string savePath;
    public DataBase dataBase;
    public InterfaceType type;
    public InventoryContainer container;
    public InventorySlot[] GetSlots{get {return container.slots;}}

    public bool AddItem(Item _item, int _amount)
    {
        if(EmptySlotCount <= 0)
            return false;
      
        InventorySlot slot = FindItemOnInventory(_item);
        if(!dataBase.GetItem[_item.ID].stackable || slot == null)
        {
            SetEmptySlot(_item, _amount);
            return true;
        }
        slot.AddAmount(_amount);
        return true;
    }

    public void Remove(Item _item, int _amount)
    {
        InventorySlot slot = FindItemOnInventory(_item);
        slot.DecreaseAmount(_amount);
    }

    public int EmptySlotCount
    {
        get
        {
            int counter = 0;
            for (int i = 0; i < GetSlots.Length; i++)
            {
                if(GetSlots[i].item.ID <= -1)
                {
                    counter++;
                }
            }
            return counter;
        }
    }

    public InventorySlot FindItemOnInventory(Item _item)
    {
        for (int i = 0; i < GetSlots.Length; i++)
        {
            if(GetSlots[i].item.ID == _item.ID)
            {
                return GetSlots[i];
            }
        }
        return null;
    }

    public InventorySlot SetEmptySlot(Item _item, int _amount)
    {
        for (int i = 0; i < GetSlots.Length; i++)
        {
            if(GetSlots[i].item.ID <= -1)
            {
                GetSlots[i].UpdateSlot(_item, _amount);
                return GetSlots[i];   
            }
        }
        //set up functionality for full inventory
        return null;
    }

    public void SwapItem(InventorySlot item1, InventorySlot item2)
    {    
        if(item2.CanPlaceInSlot(item1.itemObject) && item1.CanPlaceInSlot(item2.itemObject))
        {
            InventorySlot temp = new InventorySlot(item2.item, item2.amount);
            item2.UpdateSlot(item1.item, item1.amount);
            item1.UpdateSlot(temp.item, temp.amount);
        } 
    }

    public void RemoveItem(Item _item)
    {
        for (int i = 0; i < GetSlots.Length; i++)
        {
            if(GetSlots[i].item == _item)
            {
                GetSlots[i].UpdateSlot(null, 0);
            }
        }
    }

    [ContextMenu("save")]
    public void Save()
    {
        // string saveData = JsonUtility.ToJson(this, true);
        // BinaryFormatter bf = new BinaryFormatter();
        // FileStream file = File.Create(string.Concat(Application.persistentDataPath, savePath));
        // bf.Serialize(file, saveData);
        // file.Close();

         IFormatter  formatter = new BinaryFormatter();
         Stream stream = new FileStream(string.Concat(Application.persistentDataPath, savePath), FileMode.Create, FileAccess.Write);
         formatter.Serialize(stream, container);
         stream.Close();
    }

    [ContextMenu("load")]
    public void Load()
    {
        if(File.Exists(string.Concat(Application.persistentDataPath, savePath)))
        {
            // BinaryFormatter bf = new BinaryFormatter();
            // FileStream file = File.Open(string.Concat(Application.persistentDataPath, savePath), FileMode.Open);
            // JsonUtility.FromJsonOverwrite(bf.Deserialize(file).ToString(), this);
            // file.Close();

             BinaryFormatter formatter = new BinaryFormatter();
             Stream stream = new FileStream(string.Concat(Application.persistentDataPath, savePath), FileMode.Open, FileAccess.Read);
             InventoryContainer newContainer = (InventoryContainer)formatter.Deserialize(stream);
             for (int i = 0; i < GetSlots.Length; i++)
             {
                 GetSlots[i].UpdateSlot(newContainer.slots[i].item, newContainer.slots[i].amount);
             }
             stream.Close();
        }
    }

    [ContextMenu("clear")]
    public void Clear()
    {
        container.Clear();
    }
}

[System.Serializable]
public class InventoryContainer
{
    public InventorySlot[] slots = new InventorySlot[20];
    public void Clear()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            slots[i].RemoveItem();
        }
    }
}

public delegate void OnSlotUpdate(InventorySlot _slot);

[System.Serializable]
public class InventorySlot
{
    public ItemType[] AllowedItem = new ItemType[0];
    [System.NonSerialized]
    public UserInterface parent;
    [System.NonSerialized]
    public GameObject slot;
    [System.NonSerialized]
    public OnSlotUpdate OnAfterUpdate;
    [System.NonSerialized]
    public OnSlotUpdate OnBeforeUpdate;
    public Item item = new Item();
    public int amount;

    public ItemObject itemObject
    {
        get
        {
            if(item.ID >= 0)
            {
                return parent.inventory.dataBase.GetItem[item.ID];
            }
            return null;
        }
    }
    public InventorySlot()
    {
        UpdateSlot(new Item(), 0);
    }
    public InventorySlot(Item _item, int _amount)
    {
        UpdateSlot(_item, _amount);
    }

    public void UpdateSlot(Item _item, int _amount)
    {
        if(OnBeforeUpdate != null)
            OnBeforeUpdate.Invoke(this);
        item = _item;
        amount = _amount;
        if(OnAfterUpdate != null)
            OnAfterUpdate.Invoke(this);
    }

    public void RemoveItem()
    {
        UpdateSlot(new Item(), 0);
    }

    public void AddAmount(int value)
    {
        UpdateSlot(item, amount += value);
    }

    public void DecreaseAmount(int value)
    {
        UpdateSlot(item, amount -= value);
    }

    public bool CanPlaceInSlot(ItemObject _itemObject)
    {
        if(AllowedItem.Length <= 0 || _itemObject == null || _itemObject.data.ID < 0)
            return true;

        for (int i = 0; i < AllowedItem.Length; i++)
        {
            if(_itemObject.type == AllowedItem[i])
                return true;
        }
        return false;
    }
}
