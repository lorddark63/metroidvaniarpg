using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LongRangeWeapons : MonoBehaviour
{
    public int ammoAmount;

    void OnTriggerEnter2D(Collider2D coll)
    {
        if(coll.CompareTag("Player"))
        {
            SubItems.instance.SubItemCost(ammoAmount);
            Destroy(gameObject);
        }
    }
}
