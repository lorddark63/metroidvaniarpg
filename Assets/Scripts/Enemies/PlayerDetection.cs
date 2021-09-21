using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDetection : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D coll)
    {
       if(coll.CompareTag("Player") && transform.GetComponentInParent<EnemyProjectiles>().watcher)
       {
           print("collidiu");
           transform.GetComponentInParent<EnemyProjectiles>().Shoot();
       }
    }
}
