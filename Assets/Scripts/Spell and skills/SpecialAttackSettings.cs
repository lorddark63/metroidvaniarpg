using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialAttackSettings : MonoBehaviour
{
    public int damage;

    public void DamageTxt(Transform hit)
    {
        UiManager.instance.DamageText(hit, damage);
    }
}
