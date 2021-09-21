using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item Database", menuName = "Inventory system/Items/Database")]
public class DataBase : ScriptableObject, ISerializationCallbackReceiver
{
    public ItemObject[] itemObjects;
    public Dictionary<int, ItemObject> GetItem = new Dictionary<int, ItemObject>();

    public void OnAfterDeserialize()
    {
        for (int i = 0; i < itemObjects.Length; i++)
        {
            itemObjects[i].data.ID = i;
            GetItem.Add(i, itemObjects[i]);
        }
    }

    public void OnBeforeSerialize()
    {
        GetItem = new Dictionary<int, ItemObject>();
    }
}
