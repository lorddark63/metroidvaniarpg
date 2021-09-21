using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BossUi : MonoBehaviour
{
    public static BossUi instance;
    public GameObject bossPanel;
    public GameObject walls;

    void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        bossPanel.SetActive(false);
        walls.SetActive(false);
    }

    public void BossActivation()
    {
        bossPanel.SetActive(true);
        walls.SetActive(true);
    }

    public void BossDeactivation()
    {
        bossPanel.SetActive(false);
        walls.SetActive(false);
        StartCoroutine(BossDefeated());
    }

    IEnumerator BossDefeated()
    {
        var vel = PlayerController.instance.GetComponent<Rigidbody2D>().velocity;
        var originalSpeed = vel;
        vel = new Vector2(0, vel.y);

        PlayerController.instance.enabled = false;
        yield return new WaitForSeconds(4);
        PlayerController.instance.enabled = true;

        PlayerController.instance.GetComponent<Rigidbody2D>().velocity = originalSpeed;
    }
}
