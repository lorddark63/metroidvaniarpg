using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeteorSpawner : MonoBehaviour
{
    public GameObject meteors;
    public float spawnTime = 1;
    public Collider2D coll;

    public bool canSpawn;

    private float nextFire;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(canSpawn)
        {
            if(Time.time > nextFire)
            {
                nextFire = Time.time + spawnTime;
                SpawnMeteor();
            }
        }
    }

    void SpawnMeteor()
    {
        GameObject temp = Instantiate(meteors) as GameObject;
        temp.transform.position = new Vector2(Random.Range(coll.bounds.min.x, coll.bounds.max.x), coll.bounds.center.y);
        temp.transform.Rotate(0, 0, -45);
    }

}
