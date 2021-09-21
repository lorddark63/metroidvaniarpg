using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Armor Object", menuName = "Inventory system/Items/Armor")]
public class ArmorObj : ItemObject
{
    public float defense;

    public void Awake() 
    {
        type = ItemType.armor;
    }
}
