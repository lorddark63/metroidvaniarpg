using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New sword Object", menuName = "Inventory system/Items/Spell")]
public class SpellsObj : ItemObject
{
    private void Awake() 
    {
        type = ItemType.spell;
    }
}
