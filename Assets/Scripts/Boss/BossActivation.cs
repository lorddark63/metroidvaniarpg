using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossActivation : MonoBehaviour
{
    public GameObject boss;

    void Start()
    {
        boss.SetActive(false);
    }

    void OnTriggerEnter2D(Collider2D coll)
    {
        if(coll.CompareTag("Player"))
        {
            BossUi.instance.BossActivation();
            StartCoroutine(WaitForBoss());
        }
    }

    IEnumerator WaitForBoss()
    {
        var currentSpeed = PlayerController.instance.speed;
        PlayerController.instance.speed = 0;
        boss.SetActive(true);
        yield return new WaitForSeconds(3);
        PlayerController.instance.speed = currentSpeed;
        Destroy(gameObject);
    }
}
