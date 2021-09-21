using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BaseDamageConfig : MonoBehaviour
{
    float attack;
    float totalAtack;
    
    public float weaponAttack;
    public float criticalChance;
    public float weaponPrecision;
    public float maxCriticalDamage;
    public float ammoAmount;

    public GameObject damagetxt;

    bool isCritAttack;


    // Start is called before the first frame update
    void Start()
    {
        attack = PlayerStats.instance.attack;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public float DamageInput(float enemyDefense, Transform hit)
    {
        totalAtack = attack + weaponAttack + (100/ (100 + enemyDefense));
        float finalAttackPower = Mathf.Round(Random.Range(totalAtack - 5, totalAtack + 5));

        finalAttackPower += CalculateCrtitical((int)finalAttackPower);

        GameObject damage = Instantiate(damagetxt, hit.transform.position, Quaternion.identity);
        TextMeshPro tmp = damage.GetComponentInChildren<TextMeshPro>();
        tmp.SetText(finalAttackPower.ToString());
        tmp.color = Color.white;

        if(isCritAttack)
        {
            tmp.SetText("CRITICAL \n" + finalAttackPower.ToString());
            tmp.color = Color.yellow;
        }

        if(AttackMissed())
        {
            finalAttackPower = 0;
            tmp.SetText("MISS");
            tmp.color = Color.red;
        }

        print(finalAttackPower);
        return finalAttackPower;
    }

    private int CalculateCrtitical(int damage)
    {
        if(Random.value <= criticalChance)
        {
            int critDamage = (int)(damage + Random.Range(0, maxCriticalDamage));
            isCritAttack = true;
            return critDamage;
        }
        isCritAttack = false;
        return 0;
    }

    private bool AttackMissed()
    {
        if(Random.value >= weaponPrecision)
        {
            return true;
        }

        return false;
    }
}
