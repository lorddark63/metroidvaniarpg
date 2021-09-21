using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gold : MonoBehaviour
{
    public int goldAmount;

    void OnTriggerEnter2D(Collider2D coll)
    {
        if(coll.CompareTag("Player"))
        {
            BankAccount.instance.Money(goldAmount);
            AudioManager.instance.PlayAudio(AudioManager.instance.gem);
            Destroy(gameObject);
        }
    }
}
