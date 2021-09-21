using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using UnityEngine.UI;

public enum ItemType
{
    potion,
    armor,
    sword,
    longRangeWeapon,
    spell,
    ammo,
    bomb,
    defalt
}

public enum Attributes
{
    agility,
    intellect,
    stamina,
    stregth
}

//new code
public enum WeaponAttributes
{
    weaponAttack,
    criticalChance,
    weaponPrecision,
    maxCriticalDamage,
    ammoPrefType
    //colldown,
    //spellId
}

public enum SpellAtributes
{
    colldown,
    spellId,
    manaCost
}

public enum BombAtributes
{
    bombPrefabId
}

public class ItemObject : ScriptableObject
{
    public string itemName;
    public bool stackable;
    public int id;
    public Sprite slotImg;
    public ItemType type;
    [TextArea(15, 20)]
    public string description;
    [TextArea(2, 20)]
    public string message;
    public Item data = new Item();

    public Item CreateItem()
    {
        Item newItem = new Item(this);
        return newItem;
    }
}

[System.Serializable]
public class Item
{
    public string name;
    public string reskin;
    public ItemPrices prices;
    public int ID = -1;
    public ItemBuff[] buffs;

    //new code
    public Rarity rarity;

    public ItemWeaponStats[] weaponStats;

    public ItemSpellStats[] spellStats;

    public ItemBombStats[] bombStats;

    string hexcolor;
    public string ColoredName
    {
        get
        { 
            switch(rarity.Name)
            {
                case "comon":
                    hexcolor = "0DCF00";
                    break;
                case "uncomon":
                    hexcolor = "CFCC00";
                    break;
                case "rare":
                    hexcolor = "00BCCF";
                    break;
                case "epic":
                    hexcolor = "3306EA";
                    break;
                case "legendary":
                    hexcolor = "8D06EA";
                    break;
                case "consumable":
                    hexcolor = "FFFFFF";
                    break;
            }

            return $"<color=#{hexcolor}>{name}</color>";
        }
    }

    public string GetToolTipInfoText()
    {
        StringBuilder builder = new StringBuilder();

        builder.Append("<size=5>").Append(rarity.Name).AppendLine().Append("</size>");
        builder.Append("<br>");
        builder.Append("<color=yellow>sell price: ").Append(prices.sellPrice).Append(" gold").Append("</color>").AppendLine();

        for (int i = 0; i < weaponStats.Length; i++)
        {
            builder.Append(weaponStats[i].attributes).Append(": ").Append(weaponStats[i].value).AppendLine();
        }
        
        return builder.ToString();
    }

    public Item()
    {
        name = "";
        ID = -1;
    }

    public Item(ItemObject item)
    {
        rarity = item.data.rarity;
        ID = item.data.ID;
        name = item.data.name;
        buffs = new ItemBuff[item.data.buffs.Length];

        prices = item.data.prices;


        bombStats = new ItemBombStats[item.data.bombStats.Length];
        for (int i = 0; i < bombStats.Length; i++)
        {
            bombStats[i] = new ItemBombStats(item.data.bombStats[i].value)
            {
                attributes = item.data.bombStats[i].attributes
            };
        }
        
        spellStats = new ItemSpellStats[item.data.spellStats.Length];
        for (int i = 0; i < spellStats.Length; i++)
        {
            spellStats[i] = new ItemSpellStats(item.data.spellStats[i].value)
            {
                attributes = item.data.spellStats[i].attributes
            };
        }

         weaponStats = new ItemWeaponStats[item.data.weaponStats.Length];
         for (int i = 0; i < weaponStats.Length; i++)
         {
             weaponStats[i] = new ItemWeaponStats(item.data.weaponStats[i].value)
             {
                 attributes = item.data.weaponStats[i].attributes
             };
         }

        for (int i = 0; i < buffs.Length; i++)
        {
            buffs[i] = new ItemBuff(item.data.buffs[i].min, item.data.buffs[i].max)
            {
                //its the same of buffs[i].attributes = item.buffs[i].attributes
                attributes = item.data.buffs[i].attributes
            };
        }
    }
}

[System.Serializable]
public class ItemBuff : Imodifier
{
    public Attributes attributes;
    public int value;
    public int min;
    public int max;

    public ItemBuff(int _min, int _max)
    {
        min = _min;
        max = _max;
        GenerateValue();
    }

    public void AddValue(ref int baseValue)
    {
        baseValue += value;
    }

    public void GenerateValue()
    {
        value = UnityEngine.Random.Range(min, max);
    }
}

//new code
[System.Serializable]
public class ItemWeaponStats 
{
    public WeaponAttributes attributes;
    public float value;

    public ItemWeaponStats(float _value)
    {
        value = _value;
        GenerateValue();
    }

    public void AddValue(ref float baseValue)
    {
        baseValue += value;
    }
    public void GenerateValue()
    {
        
    }
}

[System.Serializable]
public class ItemSpellStats
{
    public SpellAtributes attributes;
    public float value;

    public ItemSpellStats(float _value) //construtor
    {
        value = _value;
        GenerateValue();
    }

    public void AddValue(ref float baseValue)
    {
        baseValue += value;
    }
    public void GenerateValue()
    {
        
    }
}

[System.Serializable]
public class ItemBombStats
{
    public BombAtributes attributes;
    public int value;

    public ItemBombStats(int _value) //construtor
    {
        value = _value;
        GenerateValue();
    }

    public void AddValue(ref int baseValue)
    {
        baseValue += value;
    }
    public void GenerateValue()
    {
        
    }
}

[System.Serializable]
public class ItemPrices
{
    public int buyPrice;
    public int sellPrice;
    public int totalPrice;

   
    public int TotalValue(int _amount)
    {
        totalPrice = buyPrice * _amount;
        return totalPrice;
    }

    public int SellPriceCalc()
    {
        sellPrice = totalPrice / 2 + (totalPrice * 25 / 100);
        return sellPrice;
    }
}
