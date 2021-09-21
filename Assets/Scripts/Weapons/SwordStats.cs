using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordStats : BaseDamageConfig
{
    public void SetSword(float _damage, float _critChance, float _critDamage, float _precision)
    {
        weaponAttack =          _damage;
        criticalChance =        _critChance;
        maxCriticalDamage =     _critDamage;
        weaponPrecision =       _precision;
    }

    

}
