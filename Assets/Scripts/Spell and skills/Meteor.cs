using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Meteor : MonoBehaviour
{
    public float xSpeed = 55;
    public int ySpeedMin = 60;
    public int ySpeedMax = 100;
    public GameObject explosion;
    public Transform explosionPlace;
    Rigidbody2D rb;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        int randomY = Random.Range(ySpeedMin, ySpeedMax);
        rb.velocity = new Vector2(-xSpeed * Time.deltaTime, -randomY * Time.deltaTime);
    }

    void OnTriggerEnter2D(Collider2D coll)
    {
        if(coll.CompareTag("ground"))
        {
            Instantiate(explosion, explosionPlace.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}
