using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Longe Range Weapon Object", menuName = "Inventory system/Items/Longe Range Weapon")]
public class LongRangeObj : ItemObject
{
    
   public void Awake() 
   {
       type = ItemType.longRangeWeapon;
   }
}
