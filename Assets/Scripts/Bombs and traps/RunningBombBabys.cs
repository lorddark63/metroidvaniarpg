using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunningBombBabys : MonoBehaviour
{ 
    private float velMove = 2;
    public bool isFaceRight;
    [SerializeField] private float explodetime;

    private Rigidbody2D rb;
    private Animator anim;

    public bool isGrounded;
    public Transform groundCheck;
    public float detectRadius;
	public LayerMask groundLayer;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        Invoke("Explode", explodetime);
    }

    // Update is called once per frame
    void Update()
    {
        GroundDetect();

        if(isGrounded)
            DetectDirection();
    }

    void Explode()
    {
        rb.velocity = Vector2.zero;
        velMove = 0;
        anim.SetBool("explode", true);
    }

    void GroundDetect()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, detectRadius, groundLayer);
    }

    void DetectDirection()
    {
        Vector3 temp = transform.localScale;

        if(isFaceRight)
        {
            temp.x = -Mathf.Abs(temp.x);
            rb.velocity = new Vector2(velMove, rb.velocity.y);
        }
        else
        {
            temp.x = Mathf.Abs(temp.x);
            rb.velocity = new Vector2(-velMove, rb.velocity.y);
        }

        transform.localScale = temp;
    }

    void GoodjobMyMan()
    {
        Destroy(gameObject);
    }
}
