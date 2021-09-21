using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Respaw : MonoBehaviour
{
    public float timeToRespaw;
    public GameObject enemyToRespaw;

    public bool isRespawning;

    // Update is called once per frame
    void Start()
    {
        isRespawning = false;
        enemyToRespaw = transform.GetChild(0).gameObject;
    }

    public IEnumerator RespawEnemy()
    {
        enemyToRespaw.SetActive(false);
        yield return new WaitForSeconds(timeToRespaw);
        enemyToRespaw.SetActive(true);
        enemyToRespaw.GetComponent<Enemy>().health = enemyToRespaw.GetComponent<EnemyDeath>().originalHealth;
        enemyToRespaw.GetComponent<SpriteRenderer>().material = enemyToRespaw.GetComponent<Blink>().original;
        enemyToRespaw.GetComponent<EnemyDeath>().isDamaged = false;

        yield return RespawAnim();
    }

    IEnumerator RespawAnim()
    {
        isRespawning = true;
        enemyToRespaw.GetComponent<Animator>().SetBool("isSpawning", true);
        yield return new WaitForSeconds(0.4F);
        enemyToRespaw.GetComponent<Animator>().SetBool("isSpawning", false);
        isRespawning = false;
    }
}
