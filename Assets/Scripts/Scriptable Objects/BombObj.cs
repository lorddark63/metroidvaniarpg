using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Bomb/trap Object", menuName = "Inventory system/Items/Bomb")]
public class BombObj : ItemObject
{
    private void Awake() 
    {
        type = ItemType.bomb;
    }
}
