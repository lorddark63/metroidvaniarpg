using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LongRangeWeaponsSettings : MonoBehaviour
{
    public Inventory equipment;
    [Header("Fire weapon atributes")]
    public LongRangeWeaponAttribute[] longRangeWeaponAttribute;
    public GameObject[] ammoPref;
    public float ammoPrefType;
    public float force = 600;
    
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < equipment.GetSlots.Length; i++)
        {
            equipment.GetSlots[i].OnBeforeUpdate += OnBeforeUpdate;
            equipment.GetSlots[i].OnAfterUpdate += OnAfterUpdate;
        }
    }

    public void OnBeforeUpdate(InventorySlot _slot)
    {
        if(_slot.itemObject == null)
            return;

        // longe range weapon
        for (int i = 0; i < _slot.item.weaponStats.Length; i++)
        {
            for (int j = 0; j < longRangeWeaponAttribute.Length; j++)
            {
                if(_slot.AllowedItem[0] == ItemType.longRangeWeapon)
                {
                    if(longRangeWeaponAttribute[j].type == _slot.item.weaponStats[i].attributes)
                        longRangeWeaponAttribute[j].value = 0;

                    ammoPrefType =  0;
                    UiManager.instance.WeaponsAndSpellsHud(1, null);
                    UiManager.instance.WeaponsAndSpellsText(1, null);
                }
            }
        }
    }

    public void OnAfterUpdate(InventorySlot _slot)
    {
        if(_slot.itemObject == null)
            return;

        // longe range weapon
        for (int i = 0; i < _slot.item.weaponStats.Length; i++)
        {
            for (int j = 0; j < longRangeWeaponAttribute.Length; j++)
            {
                if(_slot.AllowedItem[0] == ItemType.longRangeWeapon)
                {
                    if(longRangeWeaponAttribute[j].type == _slot.item.weaponStats[i].attributes)
                        longRangeWeaponAttribute[j].value = _slot.item.weaponStats[i].value;
                    
                    //5: bullet prefab
                    ammoPrefType =  longRangeWeaponAttribute[4].value;

                    UiManager.instance.WeaponsAndSpellsHud(1, _slot.itemObject.slotImg);
                    UiManager.instance.WeaponsAndSpellsText(1, _slot.amount.ToString());
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        UseWeapon();
    }

    public void UseWeapon()
    { 
        if(Input.GetKeyDown(KeyCode.X) && equipment.container.slots[2].amount > 0)
        {
            AudioManager.instance.PlayAudio(AudioManager.instance.shoot);
            GameObject ammoInst = Instantiate(ammoPref[(int)ammoPrefType], transform.position, Quaternion.Euler(0, 0, 90));
            AddWeapon(ammoInst);
            BulletDecrease();

            if(PlayerController.instance.isLookingRight)
            {
                ammoInst.GetComponent<Rigidbody2D>().AddForce(new Vector2(force, 0), ForceMode2D.Force);
            }
            else
            {
                Vector2 ammoScale = ammoInst.transform.localScale;
                ammoScale = new Vector2(-ammoScale.x, -ammoScale.y);
                ammoInst.GetComponent<Rigidbody2D>().AddForce(new Vector2(-force, 0), ForceMode2D.Force);
            }
        }


    }

    void BulletDecrease()
    {
        InventorySlot weapon = equipment.container.slots[2];
        weapon.amount--;
        if( weapon.amount < 0)
        {
             weapon.amount = 0;
        }

        weapon.parent.UpdateSlots();
        UiManager.instance.WeaponsAndSpellsText(1, weapon.amount.ToString());
    }

    void AddWeapon(GameObject weapon)
    {
        weapon.GetComponent<LongRangeWeaponStats>().SetLongRangeWeapon(
            longRangeWeaponAttribute[0].value,
            longRangeWeaponAttribute[1].value,
            longRangeWeaponAttribute[2].value,
            longRangeWeaponAttribute[3].value
        );
    }

    
}


[System.Serializable]
public class LongRangeWeaponAttribute
{
    public WeaponAttributes type;
    public float value;
}


