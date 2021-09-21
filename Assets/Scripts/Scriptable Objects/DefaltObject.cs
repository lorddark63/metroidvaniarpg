using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Defalt Object", menuName = "Inventory system/Items/Defalt")]
public class DefaltObject : ItemObject
{
    public void Awake()
    {
        type = ItemType.defalt;
    }
}
