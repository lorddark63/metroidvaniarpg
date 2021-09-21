using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Potion Object", menuName = "Inventory system/Items/Potion")]
public class PotionsObj : ItemObject
{
    
    public void Awake()
    {
        type = ItemType.potion;
    }
}
