using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossBehavior : MonoBehaviour
{
    public Transform[] transforms;
    public GameObject flame;

    public float timeShoot, countDown;
    public float timeTeleport, countDownTeleport;

    public float bossHealth, currentHealth;
    public Image healthImg;

    void Start()
    {
        transform.position = transforms[1].position;
        countDown = timeShoot;
        countDownTeleport = timeTeleport;
    }

    void Update()
    {
        CountDowns();
        DamageBoss();
        Flip();
    }

    public void CountDowns()
    {
        countDown -= Time.deltaTime;
        countDownTeleport -= Time.deltaTime;
        if(countDown < 0)
        {
             ShootPlayer();
             countDown = timeShoot;
        }

        if(countDownTeleport < 0)
        {
            countDownTeleport = timeTeleport;
            Teleport();
        }
    }

    private void ShootPlayer()
    {
        GameObject spell = Instantiate(flame, transform.position, Quaternion.identity);
    }

    public void Teleport()
    {
        var initialPos = Random.Range(0, transforms.Length);
        transform.position = transforms[initialPos].position;
    }

    public void DamageBoss()
    {
        currentHealth = GetComponent<Enemy>().health;
        healthImg.fillAmount = currentHealth / bossHealth;
    }

    void OnDestroy()
    {
        //BossUi.instance.BossDeactivation();
    }

    public void Flip()
    {
        if(transform.position.x > PlayerController.instance.transform.position.x)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
        else
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
    }
}
