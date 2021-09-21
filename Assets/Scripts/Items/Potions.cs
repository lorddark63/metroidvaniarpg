using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Potions : MonoBehaviour
{
    public float healthAmount;

    void OnTriggerEnter2D(Collider2D coll)
    {
        if(coll.CompareTag("Player"))
        {
            PlayerHealth player = coll.GetComponent<PlayerHealth>();
            player.health += healthAmount;
            //UiManager.instance.PlayerHealthUi(player.health, player.maxHealth);
            Destroy(gameObject);
        }
    }
}
