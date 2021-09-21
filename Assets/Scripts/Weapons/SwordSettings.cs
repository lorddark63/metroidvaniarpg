using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordSettings : MonoBehaviour
{
    public Inventory equipment;
    public SwordAttribute[] swordAttributes;


    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < equipment.GetSlots.Length; i++)
        {
            equipment.GetSlots[i].OnBeforeUpdate += OnBeforeUpdate;
            equipment.GetSlots[i].OnAfterUpdate += OnAfterUpdate;
        }   
    }

    // Update is called once per frame
    void Update()
    {
        AddSword();
        //equipment.
    }

    public void OnBeforeUpdate(InventorySlot _slot)
    {
        if(_slot.itemObject == null)
            return;

        for (int i = 0; i < _slot.item.weaponStats.Length; i++)
        {
            for (int j = 0; j < swordAttributes.Length; j++)
            {   
                if(_slot.AllowedItem[0] == ItemType.sword)
                {
                    if(swordAttributes[j].type == _slot.item.weaponStats[i].attributes)
                        swordAttributes[j].value = 0;

                    UiManager.instance.WeaponsAndSpellsHud(0, null);
                }   
            }

        }  
    }

    public void OnAfterUpdate(InventorySlot _slot)
    {
        if(_slot.itemObject == null)
            return;

        for (int i = 0; i < _slot.item.weaponStats.Length; i++)
        {
            for (int j = 0; j < swordAttributes.Length; j++)
            {   
                if(_slot.AllowedItem[0] == ItemType.sword)
                {
                    if(swordAttributes[j].type == _slot.item.weaponStats[i].attributes)
                        swordAttributes[j].value = _slot.item.weaponStats[i].value;
                    
                    UiManager.instance.WeaponsAndSpellsHud(0, _slot.itemObject.slotImg);
                }   
            }

        }
    }

    public void AddSword()
    {
        GetComponentInChildren<SwordStats>().SetSword(
            swordAttributes[0].value,
            swordAttributes[1].value,
            swordAttributes[2].value,
            swordAttributes[3].value
        );
    }
}

[System.Serializable]
public class SwordAttribute
{
    public WeaponAttributes type;
    public float value;
}
