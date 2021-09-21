using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flame : MonoBehaviour
{
    float moveSpeed;
    Rigidbody2D rb;
    Vector2 moveDir;
    PlayerController target;

    // Start is called before the first frame update
    void Start()
    {
        moveSpeed = GetComponent<Enemy>().speed;
        rb = GetComponent<Rigidbody2D>();
        target = PlayerController.instance;

        moveDir = (target.transform.position - transform.position).normalized * moveSpeed;
        rb.velocity = new Vector2(moveDir.x, moveDir.y);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
