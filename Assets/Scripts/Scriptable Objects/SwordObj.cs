using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New sword Object", menuName = "Inventory system/Items/Sword")]
public class SwordObj : ItemObject
{
    private void Awake() 
    {
        type = ItemType.sword;
    }
}
