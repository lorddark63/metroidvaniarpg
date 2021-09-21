using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectiles : MonoBehaviour
{
    public GameObject projectiles;

    public float projectileVel = 300;
    public float timeShoot;
    public float shootCooldown;

    public bool freqShooter;
    public bool watcher;

    // Start is called before the first frame update
    void Start()
    {
        shootCooldown = timeShoot;
    }

    // Update is called once per frame
    void Update()
    {
        if(freqShooter)
        {
            shootCooldown -= Time.deltaTime;
            if(shootCooldown < 0)
            {
                Shoot();
            }
        }

        if(watcher)
        {

        }
    }

    public void Shoot()
    {   
        GameObject cross = Instantiate(projectiles, transform.position, Quaternion.identity);

        if(transform.localScale.x < 0)
        {
            cross.GetComponent<Rigidbody2D>().AddForce(new Vector2(projectileVel, 0), ForceMode2D.Force);
        }
        else
        {
            cross.GetComponent<Rigidbody2D>().AddForce(new Vector2(-projectileVel, 0), ForceMode2D.Force);
        }

        shootCooldown = timeShoot;
    }
}
